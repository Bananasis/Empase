using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleGravityMassDot : SunGravityMassDot
{
    public override float shift
    {
        get
        {
            var size = cell.cData.cellMass.size;
            return size * size;
        }
    }
}