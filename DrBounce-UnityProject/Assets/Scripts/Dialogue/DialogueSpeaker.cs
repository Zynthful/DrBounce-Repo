/// This class is used to set the DialogueSpeakerData's source object.
/// It should be attached to the souce object.
using UnityEngine;

public class DialogueSpeaker : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The speaker to set the source object.")]
    private DialogueSpeakerData data = null;

    private void Awake()
    {
        if (data != null)
        {
            data.SetSourceObject(gameObject);
        }
        else
        {
            Debug.LogError($"uh oh, this DialogueSpeaker's DialogueSpeakerData has not been set. You need to assign a DialogueSpeakerData to this DialogueSpeaker ({this.gameObject.name}) (ask jamie for help)");
        }
    }
}