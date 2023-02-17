using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Cell))]
public class SunGravityMassDot : GravityMassDot
{
    [SerializeField] protected float massMultiplier = 1;
    protected Cell cell;
    public override Vector2 position => cell.cData.position;
    public override float mass => cell.cData.cellMass.mass * massMultiplier;

    private void Awake()
    {
        cell = GetComponent<Cell>();
    }
}