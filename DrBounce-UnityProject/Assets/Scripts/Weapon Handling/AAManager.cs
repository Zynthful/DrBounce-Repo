using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class AAManager : MonoBehaviour
{
    Camera main;
    MouseLook aimScript;

    public static List<Transform> enemiesOnScreen = new List<Transform> { };
    [field: SerializeField]
    public float assistDistance { get; private set; }
    [SerializeField]
    private float aimAssistPower = 1f;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        enemiesOnScreen.Clear();
        main = Camera.main;
        aimScript = main.GetComponent<MouseLook>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Gamepad.current != null)
        {
            bool assisting = false;
            for(int i = 0; i < enemiesOnScreen.Count; i++)
            {
                Vector2 posOnScreen = main.WorldToScreenPoint(enemiesOnScreen[i].position);
                Vector2 midScreen = new Vector2(main.pixelHeight, main.pixelWidth) / 2;
                if (Vector2.Distance(midScreen, posOnScreen) <= assistDistance)
                {
                    aimScript.aimAssistInfluence = posOnScreen * Time.deltaTime * aimAssistPower;
                    assisting = true;
                    break;
                }
            }
            if (!assisting)
                aimScript.aimAssistInfluence = Vector2.zero;
        }
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
