using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareBorderCollider : LevelBorderCollider
{
    // Use t$$anonymous$$s for initialization
    public override void Generate(float radius, bool hardBorder)
    {
        radius += 1;
        base.Generate(radius, hardBorder);

        Vector2[] points = new Vector2[5];

        points[0] = new Vector2(-radius, -radius);
        points[1] = new Vector2(-radius, radius);
        points[2] = new Vector2(radius, radius);
        points[3] = new Vector2(radius, -radius);
        points[4] = new Vector2(-radius, -radius);

        edgeCollider.points = points;
    }

    protected override void Annihilate(Cell cell)
    {
        var cd = cell.cData;
        var dir = (Vector2) transform.position - cd.position;

        var dist = Mathf.Max(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
        if (dist > _radius - 1) cell.Die();
        cell.SetSize(Mathf.Min(_radius - 1 - dist, cd.size));
    }

    protected override void Collide(Cell cell)
    {
        var cd = cell.cData;
        var dir = (Vector2) transform.position - cd.position;
        Vector2 vec = Vector3.zero;
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            dir.y = 0;
            vec.x = Mathf.Sign(dir.x);
        }
        else
        {
            dir.x = 0;
            vec.y = Mathf.Sign(dir.y);
        }


        var pushBack = vec * (_radius - 1 - cd.size) - dir;
        var dot = Vector2.Dot(cd.velocity, vec);
        cell.cData.velocity -= 2 * dot * vec;
        cell.Move(cd.position - 2 * (Vector2) pushBack);
    }
}