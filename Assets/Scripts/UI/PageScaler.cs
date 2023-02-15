using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PageScaler : MonoBehaviour
{

    public Vector2 minSizeRatio = new Vector2(1, 1);
    [SerializeField, HideInInspector] RectTransform _rect;
    [SerializeField] private Canvas canvas;
    [SerializeField] private List<LayoutElement> pages = new List<LayoutElement>();
    Vector2 _cachedScreenSize;

    private void OnValidate()
    {
        _rect = GetComponent<RectTransform>();
    }
    

    void Update()
    {

        Vector2 screenSize = canvas.pixelRect.max;
        CanvasScaler cs;
        
        
        if (_cachedScreenSize == screenSize)
            return;
        
        if (minSizeRatio.x >= 0f)
            foreach (var layoutElement in pages)
            {
                if (layoutElement.minWidth >= 0f)
                    layoutElement.minWidth = minSizeRatio.x * screenSize.x;
            }
        
        if (minSizeRatio.y >= 0f)
            foreach (var layoutElement in pages)
            {
                if (layoutElement.minHeight >= 0f)
                    layoutElement.minHeight = minSizeRatio.y * screenSize.y;
            }
        
       // _rect.
        _cachedScreenSize = screenSize;
    }
}