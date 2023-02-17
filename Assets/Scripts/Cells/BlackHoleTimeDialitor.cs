using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleTimeDialitor : TimeDialator
{
        
    protected Cell cell;
    [SerializeField] protected float strength = 1;
    [SerializeField] protected float radiusMultiplier = 2;
    private void Awake()
    {
        cell = GetComponent<Cell>();
    }
    
    public override float GetDialition(CellData cdata)
    {
        var vec = cell.cData.position - cdata.position;
        var dist = vec.magnitude;
        if (dist < cell.cData.cellMass.size || dist*radiusMultiplier > cell.cData.cellMass.size) return 1;
        return 1/(1+strength*(cell.cData.cellMass.size-dist*radiusMultiplier));
    }
    
    


    
}
