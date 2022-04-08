using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class AAManager : MonoBehaviour
{
    Camera main;
    MouseLook aimScript;
    Transform playerTransform;

    [SerializeField] LayerMask wallLayers;

    public static List<Transform> enemiesOnScreen = new List<Transform> { };
    [field: SerializeField]
    public float assistDistance { get; private set; }
    [SerializeField]
    private float aimAssistPower = 1f;

    // Start is called before the first frame update
    void Start()
    {
        SetupAA();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetupAA();
    }

    void SetupAA()
    {
        playerTransform = PlayerMovement.player;
        enemiesOnScreen.Clear();
        main = Camera.main;
        aimScript = main.GetComponent<MouseLook>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Gamepad.current == null)
        {
            float closest = float.PositiveInfinity;
            Transform closestT = null;
            Vector2 midScreen = new Vector2(main.pixelWidth, main.pixelHeight) / 2;

            Debug.Log("Midscreen: " + midScreen);
            for (int i = 0; i < enemiesOnScreen.Count; i++)
            {
                Vector3 posOnScreen = main.WorldToScreenPoint(enemiesOnScreen[i].position);
                float dist = Vector2.Distance(midScreen, posOnScreen);

                Ray wallCheck = new Ray(playerTransform.position, (enemiesOnScreen[i].position - playerTransform.position));

                if (dist <= assistDistance && dist < closest)
                {
                    RaycastHit hit = new RaycastHit();
                    if(Physics.Raycast(wallCheck, out hit, float.PositiveInfinity, wallLayers))
                    {
                        if(hit.transform.GetComponentInParent<Enemy>())
                        {
                            Debug.DrawRay(wallCheck.origin, wallCheck.direction * 30, Color.magenta, Time.deltaTime);
                            closest = dist;
                            closestT = enemiesOnScreen[i];
                        }
                    }
                }
            }
            if (closestT != null)
            {
                Vector2 posOnScreen = main.WorldToScreenPoint(closestT.position);
                aimScript.aimAssistInfluence = (midScreen - posOnScreen).normalized * Time.deltaTime * aimAssistPower;
                Debug.Log("Enemy position: " + closestT.position, closestT);
                Debug.Log("On Screen Position: " + posOnScreen, closestT);
            }
            else
                aimScript.aimAssistInfluence = Vector2.zero;
        }
        else
            aimScript.aimAssistInfluence = Vector2.zero;
    }

    public static void RemoveTransform(Transform t)
    {
        if(enemiesOnScreen.Contains(t))
            enemiesOnScreen.Remove(t);
    }

    public static void AddTransform(Transform t)
    {
        if (!enemiesOnScreen.Contains(t))
            enemiesOnScreen.Add(t);
    }
}
