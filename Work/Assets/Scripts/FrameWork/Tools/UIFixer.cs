using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// UI自适应
/// </summary>
/// 
[ExecuteInEditMode]
[RequireComponent(typeof (RectTransform ))]
public class UIFixer : UIBehaviour  {
    private CanvasScaler canvasScaler = null;
    protected override void OnRectTransformDimensionsChange()
    {
        if(canvasScaler == null)
        {
            canvasScaler = this.GetComponent<CanvasScaler>();
            if (canvasScaler == null) return;
        }

        var currentRadio = Screen.width / Screen.height;
        var targetRadio = canvasScaler.referenceResolution.x / canvasScaler.referenceResolution.y;

        if(currentRadio >=targetRadio)
        {
            canvasScaler.matchWidthOrHeight = 1;
        }
        else
        {
            canvasScaler.matchWidthOrHeight = 0;
        }
    }

}
