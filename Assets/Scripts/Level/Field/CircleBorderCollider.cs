using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleBorderCollider : LevelBorderCollider
{
    [SerializeField] private int NumEdges = 100;
    // Use t$$anonymous$$s for initialization
    public override void Generate(float radius,bool hardBorder)
    {
        radius = radius + 1;
        base.Generate(radius, hardBorder);
      
        Vector2[] points = new Vector2[NumEdges+1];

        for (int i = 0; i < NumEdges+1; i++)
        {
            float angle = 2 * Mathf.PI * i / NumEdges;
            float x = _radius * Mathf.Cos(angle);
            float y = _radius * Mathf.Sin(angle);

            points[i] = new Vector2(x, y);
        }

        edgeCollider.points = points;
    }

    protected override void Annihilate(Cell cell)
    {
        var cd = cell.cData;
        var dist = ( (Vector2)transform.position - cd.position).magnitude;
        if (dist > _radius-1) cell.Die();
        cell.SetSize( Mathf.Min(_radius-1 - dist,cd.cellMass.size))  ;
    }
    protected override void Collide(Cell cell)
    {
        var cd = cell.cData;
        var pos = (Vector2)transform.position - cd.position;
        var vec = pos.normalized;
        var pushBack = vec * (_radius - 1 - cd.cellMass.size) - pos;
        var dot = Vector2.Dot(cd.velocity, vec);
        cell.cData.velocity -= 2 * dot * vec;
        cell.Move(cd.position - 2 * pushBack);
    }


}
