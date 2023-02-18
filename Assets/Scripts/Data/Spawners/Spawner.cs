using System;
using System.Collections.Generic;
using Data.Events.LevelEvents;
using UnityEngine;
using Utils;
using Random = System.Random;


public class Spawner : ScriptableObject
{
    protected Random random;
    public bool isLevelGoal;
    [SerializeField] private CollisionHandling wallCollision;
    [SerializeField] private CollisionHandling cellCollision;
    [SerializeField] private float shrinkCompensate = 0.99f;
    [SerializeField] protected float minSize;
    [SerializeField] private float gapMultiplier = 1;


    public virtual List<CellData> Spawn(SimpleCollisionTree sct, float borderSize, BorderShape borderShape)
    {
        throw new NotImplementedException();
    }


    protected virtual bool TrySpawn(SimpleCollisionTree sct, float borderSize, BorderShape borderShape,
        ref CellData cellData)
    {
        var collision = borderShape switch
        {
            BorderShape.Square =>
                -borderSize + cellData.cellMass.size +
                Mathf.Max(-cellData.position.x, cellData.position.x, -cellData.position.y, cellData.position.y),
            BorderShape.Circle =>
                -borderSize + cellData.cellMass.size + cellData.position.magnitude,
            _ => throw new ArgumentOutOfRangeException(nameof(borderShape), borderShape, null)
        };
        if (collision > 0)
            switch (wallCollision)
            {
                case CollisionHandling.Shrink:
                    if ((cellData.cellMass.size - collision) * gapMultiplier < 0.01) return false;
                    cellData.cellMass.size = (cellData.cellMass.size - collision) * shrinkCompensate * gapMultiplier;
                    break;

                case CollisionHandling.Destroy:
                    cellData.cellMass.size *= gapMultiplier;
                    if (collision > 0) return false;
                    break;

                case CollisionHandling.Mixed:
                    if ((cellData.cellMass.size - collision) * gapMultiplier < minSize) return false;
                    cellData.cellMass.size = (cellData.cellMass.size - collision) * shrinkCompensate * gapMultiplier;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

        collision = sct.DetectMaximalCollision(cellData);
        switch (cellCollision)
        {
            case CollisionHandling.Shrink:
                if (cellData.cellMass.size - collision < 0.01) return false;
                cellData.cellMass.size = (cellData.cellMass.size - collision) * shrinkCompensate;
                sct.Add(cellData);
                return true;
            case CollisionHandling.Destroy:
                if (collision > 0) return false;
                sct.Add(cellData);
                return true;
            case CollisionHandling.Mixed:
                if (cellData.cellMass.size - collision < minSize) return false;
                cellData.cellMass.size = (cellData.cellMass.size - collision) * shrinkCompensate;
                sct.Add(cellData);
                return true;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

public enum CollisionHandling
{
    Shrink,
    Destroy,
    Mixed
}