using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;


[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Cell : RegistryEntity
{
    [Inject, SerializeField, HideInInspector]
    private CellManager _cellManager;

    [Inject, SerializeField, HideInInspector]
    private CellPool _cellPool;

    [Inject, SerializeField, HideInInspector]
    protected GameManager _gameManager;

    [SerializeField] public CellData cData;
    [SerializeField] public LocalData lData;
    [SerializeField] private SpriteRenderer definiteBorder;
    [HideInInspector] public int id;
    protected SpriteRenderer sr;
    protected Rigidbody2D body;
    private Transform _transform;
    protected MaterialPropertyBlock mpb;

    protected static readonly int Dencity = Shader.PropertyToID("Dencity");
    protected static readonly int Weight = Shader.PropertyToID("Weight");
    protected static readonly int Id = Shader.PropertyToID("Id");

    public readonly UnityEvent OnDeath = new UnityEvent();
    public readonly UnityEvent<Cell> OnConsume = new UnityEvent<Cell>();
    public readonly UnityEvent<Cell> OnConsumed = new UnityEvent<Cell>();
    public readonly UnityEvent<(float, float)> OnSizeChange = new UnityEvent<(float, float)>();

    protected virtual void OnAwake()
    {
        _transform = transform;
        id = Random.Range(0, int.MaxValue);
        body = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        mpb = new MaterialPropertyBlock();
    }

    protected virtual void OnEnable()
    {
        UpdateCell();
        _cellManager.Add(this);
    }

    protected virtual void OnDisable()
    {
        _cellManager.Remove(this);
    }

    private void Awake()
    {
        OnAwake();
    }

    private void FixedUpdate()
    {
        UpdateCell();
    }

    public virtual void UpdateCellSize()
    {
        transform.localScale = new Vector3(cData.cellMass.size, cData.cellMass.size, 1) / 0.4f;
    }

    public virtual void UpdatePropertyBlock()
    {
        sr.GetPropertyBlock(mpb);
        mpb.SetFloat(Dencity, Mathf.Sqrt(cData.cellMass.size));
        mpb.SetFloat(Id, (float) id % 1000);
        mpb.SetFloat(Weight, lData.sizeRatio);
        sr.SetPropertyBlock(mpb);
    }

    public virtual void UpdateVelocity()
    {
        cData.velocity += lData.gravityAcceleration * lData.timeMultiplier * Time.fixedDeltaTime;
        body.velocity = cData.velocity * lData.timeMultiplier;
        Debug.DrawLine(cData.position, cData.position + cData.velocity, Color.yellow);
    }

    public virtual void UpdateCell()
    {
        cData.position = _transform.position;
        UpdateCellSize();
        UpdateVelocity();
        UpdatePropertyBlock();
        if (definiteBorder == null) return;
        var color = lData.sizeRatio > 0 ? Color.red : Color.blue;
        color.a = _gameManager.DefiniteBorderAlpha.val;
        definiteBorder.color = color;
    }

    public virtual void Die()
    {
        SetSize(0);
        OnDeath.Invoke();
        _cellPool.Return(this);
    }

    public void SetSize(float size)
    {
        cData.cellMass.size = size;
        OnSizeChange.Invoke((cData.cellMass.size, cData.cellMass.mass));
    }

    public void SetMass(float mass)
    {
        cData.cellMass.mass = mass;
        OnSizeChange.Invoke((cData.cellMass.size, cData.cellMass.mass));
    }

    public void Move(Vector2 pos)
    {
        body.position = pos;
        cData.position = pos;
    }
}

[Serializable]
public struct CellData
{
    public Vector2 position;
    public Vector2 velocity;
    public bool pinned;
    public CellMassData cellMass;
    public float inertiaMultiplier => type == CellType.AntiInertial ? -1 : 1;
    public float massMultiplier => type == CellType.Antimatter ? -1 : 1;
    public CellType type;
}

[Serializable]
public struct CellMassData
{
    public float massDefect
    {
        get => _massDefect + 1;

        set
        {
            if (_massDirty) _mass = _size * _size * Mathf.PI * massDefect;
            _massDefect = value - 1;
            _massDirty = false;
            _sizeClean = false;
        }
    }

    private float _size;
    [SerializeField] private float _mass;
    [SerializeField, Range(-0.99f, 3)] private float _massDefect;

    private bool _sizeClean;

    public float size
    {
        get
        {
            if (_sizeClean) return _size;
            _size = Mathf.Sqrt(_mass / (Mathf.PI * massDefect));
            _sizeClean = true;
            return _size;
        }
        set
        {
            if (_size == value) return;
            _sizeClean = true;
            _size = value;
            _massDirty = true;
        }
    }

    private bool _massDirty;

    public float mass
    {
        get
        {
            if (!_massDirty) return _mass;
            _mass = _size * _size * Mathf.PI * massDefect;
            _massDirty = false;
            return _mass;
        }
        set
        {
            if (_mass == value) return;
            _sizeClean = false;
            _mass = value;
            _massDirty = false;
        }
    }
}

[Serializable]
public struct LocalData
{
    public float sizeRatio;
    public float timeMultiplier;
    public Vector2 gravityAcceleration;
}