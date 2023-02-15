using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollPaged : ScrollRect
{
    protected bool m_overflow;


    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        if (m_overflow)
        {
            OnEndDrag(eventData);
            OnBeginDrag(eventData);
        }
    }

    protected override void SetContentAnchoredPosition(Vector2 position)
    {
        m_overflow = false;
        var offset = Vector2.zero;
        Vector2 min = m_ContentBounds.min;
        Vector2 max = m_ContentBounds.max;

        // min/max offset extracted to check if approximately 0 and avoid recalculating layout every frame (case 1010178)

        float maxOffset = viewRect.rect.max.x - max.x;
        float minOffset = viewRect.rect.min.x - min.x;

        //Debug.Log($"{viewRect.rect.min.x},{m_ContentBounds.min}");
        if (minOffset < -0.1f)
        {
            offset.x = -minOffset + maxOffset;
            m_overflow = true;
        }
        else if (maxOffset > 0.1f)
        {
            offset.x = -maxOffset + minOffset;
            m_overflow = true;
        }

        base.SetContentAnchoredPosition(position + offset);
        
    }


    protected override void LateUpdate()
    {
        var vel = velocity;
        base.LateUpdate();

        if (m_overflow) return;
        //horizontalScrollbar.value = 1-horizontalScrollbar.value;
        velocity = vel;


    }
}