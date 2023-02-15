using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;


public abstract class Popup : MonoBehaviour
{

    protected Queue<PopupData> _queue = new Queue<PopupData>();
    private Coroutine _coroutine = null;
    public PopupType type;


    protected virtual void Hide()
    {
        
    }

    public void Next()
    {
        Hide();
        if(! (_coroutine is null))
            StopCoroutine(_coroutine);
        _coroutine = null;
        CheckQueue();
    }

    public void Clear()
    {
        Hide();
        if(! (_coroutine is null))
            StopCoroutine(_coroutine);
        _coroutine = null;
        _queue.Clear();
    }

    protected virtual void Awake()
    {
        Clear();
    }

    public void Enqueue(PopupData popupData)
    {
        if (popupData.force)
        {
            _queue.Clear();
        }
        _queue.Enqueue(popupData);
        if (_coroutine != null || !gameObject.activeSelf)
            return;
        _coroutine = StartCoroutine(Show(_queue.Dequeue()));
    }

    private void OnEnable()
    {
        CheckQueue();
    }

    protected abstract IEnumerator Show(PopupData popupData);

    protected bool CheckQueue()
    {
        if (_queue.Count == 0)
        {
            _coroutine = null;
            return false;
        }

        _coroutine = StartCoroutine(Show(_queue.Dequeue()));
        return true;
    }
}