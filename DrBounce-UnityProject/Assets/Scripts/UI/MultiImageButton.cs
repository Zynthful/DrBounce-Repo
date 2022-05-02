// from: https://forum.unity.com/threads/tint-multiple-targets-with-single-button.279820/
// but modified so it allows for unique transition colors per graphic

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MultiImageTargetGraphics))]
public class MultiImageButton : Button
{
    private MultiImageTargetGraphics.GraphicTransition[] graphicTransitions = null;

    private MultiImageTargetGraphics targetGraphics = null;

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        // If it could not get the graphic transitions, return
        if (!GetGraphicTransitions())
            return;

        foreach (MultiImageTargetGraphics.GraphicTransition graphicTransition in graphicTransitions)
        {
            var targetColor =
                state == SelectionState.Disabled ? graphicTransition.disabledColor :
                state == SelectionState.Highlighted ? graphicTransition.highlightedColor :
                state == SelectionState.Normal ? graphicTransition.normalColor :
                state == SelectionState.Pressed ? graphicTransition.pressedColor :
                state == SelectionState.Selected ? graphicTransition.selectedColor : Color.white;

            graphicTransition.targetGraphic.CrossFadeColor(targetColor, instant ? 0 : colors.fadeDuration, true, true);
        }
    }

    private bool GetGraphicTransitions()
    {
        if (!targetGraphics)
            targetGraphics = GetComponent<MultiImageTargetGraphics>();

        graphicTransitions = targetGraphics?.GetTargetGraphics;

        return graphicTransitions != null && graphicTransitions.Length > 0;
    }
}