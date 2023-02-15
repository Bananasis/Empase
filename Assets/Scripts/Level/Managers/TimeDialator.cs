using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TimeDialator : RegistryEntity
{
    [Inject,SerializeField,HideInInspector] private TimeDialitionManager _timeDialitionManager;
    protected virtual void OnEnable()
    {
        _timeDialitionManager.Add(this);
    }

    protected virtual  void OnDisable()
    {
        _timeDialitionManager.Remove(this);
    }
    public virtual float GetDialition(CellData cdata)
    {
        throw new NotImplementedException();
    }

}
