using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchHeldItem : MonoBehaviour
{
    private InputMaster controls;
    [HideInInspector] public Transform currentHeldTransform;
    public static SwitchHeldItem instance;
    private List<GameObject> throwablesInScene = new List<GameObject> { };

    private void Awake()
    {
        controls = new InputMaster();
        controls.Player.SwitchHeld.performed += _ => SwitchActiveItem();
        instance = this;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (GunThrowing obj in FindObjectsOfType<GunThrowing>())
        {
            throwablesInScene.Add(obj.gameObject);
        }
        currentHeldTransform = GetCurrentThrowables()[0];
    }

    private List<Transform> GetCurrentThrowables()
    {
        List<Transform> transforms = new List<Transform> { };
        for(int i = 0; i < transform.childCount; i++)
        {
            if (throwablesInScene.Contains(transform.GetChild(i).gameObject))
            {
                transforms.Add(transform.GetChild(i));
            }
        }
        return transforms;
    }

    public void OnPickupItem(Transform item)
    {
        if (!currentHeldTransform)
        {
            currentHeldTransform = item;
        }
        else if (currentHeldTransform != item)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void SwitchActiveItem()
    {
        List<Transform> transforms = GetCurrentThrowables();
        if (transforms.Count != 0)
        {
            if (transforms.Contains(currentHeldTransform))
            {
                currentHeldTransform.gameObject.SetActive(false);

                if (transforms.IndexOf(currentHeldTransform) < transforms.Count - 1)
                {
                    currentHeldTransform = transforms[transforms.IndexOf(currentHeldTransform) + 1];
                }
                else
                {
                    currentHeldTransform = transforms[0];
                }
            }
            else
            {
                currentHeldTransform = transforms[0];
            }
            
            currentHeldTransform.gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        controls.Player.SwitchHeld.performed -= _ => SwitchActiveItem();
        controls.Disable();
    }

    public void AddToList(GameObject throwable) 
    {
        throwablesInScene.Add(throwable);
    }
}
