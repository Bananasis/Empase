using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextPopup : Popup
{
    private TextMeshProUGUI _label;


    private float alpha
    {
        set
        {
            var labelColor = _label.color;
            labelColor.a = value;
            _label.color = labelColor;
        }
    }

   
    protected override void Hide()
    {
        alpha = 0;
    }
    
    protected override void Awake()
    {
        _label = GetComponent<TextMeshProUGUI>();
        base.Awake();
    }

    protected override IEnumerator Show(PopupData popupData)
    {
        _label.text = popupData.text;
        for (float passed = 0; passed < 1; passed += Time.unscaledDeltaTime)
        {
            alpha = passed;
            if (_queue.Count > 0 && (popupData.vol))
                break;
            yield return null;
        }

        for (float passed = 0; passed < popupData.duration - 2; passed += Time.unscaledDeltaTime)
        {
            if (_queue.Count > 0 && (popupData.vol || _queue.Peek().force))
                break;
            yield return null;
        }

        for (float passed = 1; passed > 0; passed -= Time.unscaledDeltaTime)
        {
            alpha = passed;
            if (_queue.Count > 0 && (popupData.vol))
                break;
            yield return null;
        }

        alpha = 0;
        CheckQueue();
    }


}
