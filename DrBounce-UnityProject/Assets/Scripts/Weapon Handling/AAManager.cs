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
            Vector2 midScreen = new Vector2(main.pixelHeight, main.pixelWidth) / 2;
            for (int i = 0; i < enemiesOnScreen.Count; i++)
            {
                Vector3 posOnScreen = main.WorldToScreenPoint(enemiesOnScreen[i].position);
                Debug.Log("Distance to enemy " + i + ") " + Vector2.Distance(midScreen, posOnScreen));
                Debug.Log((midScreen - (Vector2)posOnScreen) * Time.deltaTime * aimAssistPower);
                if (Vector2.Distance(midScreen, posOnScreen) <= assistDistance)
                {
                    if (posOnScreen.z < closest)
                    {
                        closest = posOnScreen.z;
                        closestT = enemiesOnScreen[i];
                    }
                }
            }
            if (closestT != null)
            {
                aimScript.aimAssistInfluence = ((Vector2)main.WorldToScreenPoint(closestT.position) - midScreen).normalized * Time.deltaTime * aimAssistPower;
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
