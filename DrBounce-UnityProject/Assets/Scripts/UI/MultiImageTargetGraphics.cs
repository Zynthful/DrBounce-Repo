// from: https://forum.unity.com/threads/tint-multiple-targets-with-single-button.279820/
// but modified so it allows for unique transition colors per graphic

using UnityEngine;
using UnityEngine.UI;

public class MultiImageTargetGraphics : MonoBehaviour
{
    [SerializeField]
    private GraphicTransition[] targetGraphics = null;
    public GraphicTransition[] GetTargetGraphics => targetGraphics;

    [System.Serializable]
    public struct GraphicTransition
    {
        public Graphic targetGraphic;
        public Color disabledColor;
        public Color highlightedColor;
        public Color normalColor;
        public Color pressedColor;
        public Color selectedColor;
    }
}