using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(EdgeCollider2D))]

public class LevelBorderCollider : MonoBehaviour
{
    protected float _radius = 50;
    [SerializeField] protected bool _hardBorder = true;
    [SerializeField] private VoronoiController vc;
    [SerializeField] private BorderDencityController bdc;
    [SerializeField] private SpriteRenderer border;
    [SerializeField] private Color annihilateColor;
    [SerializeField] private Color hardColor;
        
    protected EdgeCollider2D edgeCollider;
    
    private void Awake()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
    
    }
    public virtual void Generate(float radius, bool hardBorder)
    {
        vc.SetSize(2*(radius-1));
        bdc.SetSize(2*(radius-1));
        _radius = radius;
        _hardBorder = hardBorder;
        border.color = hardBorder ? hardColor : annihilateColor;
    }
    
    protected  virtual void Annihilate(Cell cell)
    {
       
    }

    protected  virtual void Collide(Cell cell)
    {
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Cell cell = other.GetComponent<Cell>();
        if (cell == null) return;
        if (!_hardBorder)
        {
            Annihilate(cell);
            return;
        }

        Collide(cell);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Cell cell = other.GetComponent<Cell>();
        if (cell == null) return;
        if (!_hardBorder)
        {
            Annihilate(cell);
            return;
        }

        Collide(cell);
    }
}
public enum BorderType
{
    Collider,
    Annihilator
}

public enum BorderShape
{    
    Square,
    Circle
}
