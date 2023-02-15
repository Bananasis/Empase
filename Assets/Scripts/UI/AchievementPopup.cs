using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class AchievementPopup : Popup
{
    private Animator _animator;

    [SerializeField] TextMeshProUGUI _label;
    [SerializeField] Image _image;
    private static readonly int Show1 = Animator.StringToHash("Show");
    private static readonly int Hide1 = Animator.StringToHash("Hide");

    protected override IEnumerator Show(PopupData popupData)
    {
        _label.text = popupData.text;
        _image.sprite = popupData.sprite;
        _animator.SetTrigger(Show1);
        for (float passed = 0; passed < 1; passed += Time.unscaledDeltaTime)
        {
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
        _animator.SetTrigger(Hide1);
        for (float passed = 1; passed > 0; passed -= Time.unscaledDeltaTime)
        {
            if (_queue.Count > 0 && (popupData.vol))
                break;
            yield return null;
        }

        CheckQueue();
    }

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
    }
}