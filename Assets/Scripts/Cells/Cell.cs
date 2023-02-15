using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;


[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Cell : RegistryEntity
{

    [Inject,SerializeField,HideInInspector] private CellManager _cellManager;
    [Inject,SerializeField,HideInInspector] private CellPool _cellPool;
    [SerializeField] public CellData cData;
    [SerializeField] public LocalData lData;
    
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
    public readonly UnityEvent<(float,float)> OnSizeChange = new UnityEvent<(float,float)>();

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
        transform.localScale = new Vector3(cData.size, cData.size, 1) / 0.4f;
    }

    public virtual void UpdatePropertyBlock()
    {
        sr.GetPropertyBlock(mpb);
        mpb.SetFloat(Dencity, Mathf.Sqrt(cData.size));
        mpb.SetFloat(Id, (float) id % 1000);
        mpb.SetFloat(Weight, lData.sizeRatio);
        sr.SetPropertyBlock(mpb);
    }

    public virtual void UpdateVelocity()
    {
        cData.velocity += lData.gravityAcceleration * lData.timeMultiplier* Time.fixedDeltaTime;
        body.velocity = cData.velocity * lData.timeMultiplier;
        Debug.DrawLine(cData.position, cData.position + cData.velocity, Color.yellow);
    }

    public virtual void UpdateCell()
    {
        cData.position = _transform.position;
        UpdateCellSize();
        UpdateVelocity();
        UpdatePropertyBlock();
    }

    public virtual void Die()
    {
        SetSize(0);
        OnDeath.Invoke();
        _cellPool.Return(this);
    }

    public void SetSize(float size)
    {
        cData.size = size;
        OnSizeChange.Invoke( (cData.size,cData.mass));
    }

    public void SetMass(float mass)
    {
        cData.mass = mass;
        OnSizeChange.Invoke((cData.size,cData.mass));
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

    public float inertiaMultiplier => type == CellType.AntiInertial ? -1 : 1;
    public float massMultiplier => type == CellType.Antimatter ? -1 : 1;

    [SerializeField] private float _size;
    [SerializeField] private float _mass;
    public CellType type;

    public float size
    {
        get => _size;
        set
        {
            _size = value;
            _mass = _size * _size * Mathf.PI;
        }
    }

    public float mass
    {
        get => _mass;
        set
        {
            _mass = value;
            _size = Mathf.Sqrt(_mass / Mathf.PI);
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