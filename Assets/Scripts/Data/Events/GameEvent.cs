using System;
using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.Events;
using Zenject;


public abstract class GameEvent : ScriptableObject
{
    protected UnityEvent OnEvent = new UnityEvent();
    protected List<IDisposable> connections = new List<IDisposable>();
    [Inject] protected EventManager _eventManager;
    [Inject] private PopupManager _popupManager;
    protected abstract IAnnouncement _announcement { get; }

    public virtual void Activate()
    {
        connections.Add(OnEvent.Subscribe(() =>
        {
            _announcement?.Show(_popupManager);
        }));
        _eventManager.Add(this);
    }

    public virtual void Subscribe(Action act)
    {
        connections.Add(OnEvent.Subscribe(act));
    }

    public virtual void SubscribePersistent(Action act, bool once = true)
    {
        if (once)
        {
            OnEvent.SubscribeOnce(act);
            return;
        }

        OnEvent.Subscribe(act);
    }

    public virtual void Deactivate()
    {
        _eventManager.Remove(this);
        connections.Dispose();
    }
}