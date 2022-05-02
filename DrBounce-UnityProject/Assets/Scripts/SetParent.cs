// Can't find a way of calling static functions on a static class from the inspector
// So this is a scriptable object which is the instance used for calling the static functions

using UnityEngine;

[CreateAssetMenu(fileName = "Set Parent", menuName = "ScriptableObjects/Scripts/Set Parent")]
public class SetParent : ScriptableObject
{
    public static void SetNew(Transform obj, Transform parent)
    {
        obj.parent = parent;
    }

    public static void SetPlayerParent(Transform parent)
    {
        GameManager.player.transform.parent = parent;
    }

    public static void Unparent(GameObject obj)
    {
        obj.transform.parent = null;
    }

    public static void UnparentPlayer()
    {
        GameManager.player.transform.parent = null;
    }
}