using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateTransformPatrol : MonoBehaviour
{
    [SerializeField]
    private Point[] points = null;

    [System.Serializable]
    private struct Point
    {
        public Transform transform;
        public float timeToReach;
    }

    [SerializeField]
    private GameObject[] objsToInstantiate = null;

    [SerializeField]
    private List<AnimatedObject> activeObjs = new List<AnimatedObject>();

    public class AnimatedObject
    {
        public GameObject obj = null;   
        public int lastPoint = 0;

        public void OnReachedPoint()
        {
            lastPoint++;
        }
    }

    [SerializeField]
    private float spawnCooldown = 4.0f;

    private bool coolingDown = false;

    [SerializeField]
    private bool destroyObjs = true;

    [SerializeField]
    private float disableAfterTime = 0;

    private bool noNewObjs = false;

    private void Start() {
        if(disableAfterTime > 0)
            StartCoroutine(StopAfterTime(disableAfterTime));
    }

    private void Update()
    {
        if (!coolingDown && !noNewObjs)
            SpawnNew();
    }

    public IEnumerator Move(AnimatedObject activeObj)
    {
        float elapsedTime = 0;
        while (elapsedTime < points[activeObj.lastPoint + 1].timeToReach)
        {
            activeObj.obj.transform.position = Vector3.Lerp(points[activeObj.lastPoint].transform.position, points[activeObj.lastPoint + 1].transform.position, elapsedTime / points[activeObj.lastPoint + 1].timeToReach);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        activeObj.obj.transform.position = points[activeObj.lastPoint + 1].transform.position;
        activeObj.OnReachedPoint();

        // Check if we've reached the final point
        if (activeObj.lastPoint + 1 >= points.Length)
        {
            DestroyObj(activeObj);
        }
        // Otherwise we travel towards the next one
        else
        {
            StartCoroutine(Move(activeObj));
        }

        yield return null;
    }

        public void DestroyObj(AnimatedObject activeObj)
    {
        if(destroyObjs)
            Destroy(activeObj.obj);
        else
            Destroy(activeObj.obj, 15);
        activeObjs.Remove(activeObj);
    }

    private void SpawnNew()
    {
        coolingDown = true;
        AnimatedObject animateObject = new AnimatedObject();
        animateObject.obj = Instantiate(objsToInstantiate[Random.Range(0, objsToInstantiate.Length - 1)]);
        animateObject.obj.transform.position = points[animateObject.lastPoint].transform.position;
        animateObject.obj.transform.parent = transform;
        activeObjs.Add(animateObject);
        StartCoroutine(Move(animateObject));
        StartCoroutine(SpawnCooldown());
    }

    private IEnumerator SpawnCooldown()
    {
        yield return new WaitForSeconds(spawnCooldown);
        coolingDown = false;
    }

    private IEnumerator StopAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        destroyObjs = true;
        noNewObjs = true;
    }
}