using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GravityMassDot : RegistryEntity
{
    [Inject,SerializeField,HideInInspector] private GravityManager _gravityManager;
    protected virtual void OnEnable()
    {
        _gravityManager.Add(this);
    }

    protected virtual void OnDisable()
    {
        _gravityManager.Remove(this);
    }

    public virtual float shift => 0;
    public virtual Vector2 position => transform.position;

    public virtual float mass
    {
        get
        {
            var sc = transform.localScale.x;
            return sc * sc * Mathf.PI;
        }
    }

    public virtual Vector2 GetAcceleration(CellData cData)
    {
        var vec = position - cData.position;
        var distSqr = vec.sqrMagnitude - shift;
        if (distSqr < cData.cellMass.size) return Vector2.zero;
        return mass / distSqr * vec.normalized;
    }
}