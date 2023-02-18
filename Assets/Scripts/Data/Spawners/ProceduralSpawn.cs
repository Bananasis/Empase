using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Data.Spawners
{
    public class ProceduralSpawn : Spawner
    {
        [SerializeField] protected AnimationCurve massDistribution = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] protected AnimationCurve orbitalDistribution = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] protected float maxSize;
        [SerializeField] private int maxIterations = 1000;
        [SerializeField] private int number;
        [SerializeField] private List<CellType> possibleTypes = new List<CellType> {CellType.Matter};

        public override List<CellData> Spawn(SimpleCollisionTree sct, float borderSize, BorderShape borderShape)
        {
            random = new Random();
            List<CellData> spawned = new List<CellData>();
            int unsuccessful = 0;
            for (int i = 0; i < number && unsuccessful < maxIterations; i++)
            {
                var cellData = SetUpCell(borderShape, borderSize);
                cellData.type = possibleTypes[random.Next(possibleTypes.Count)];
                
                if (TrySpawn(sct, borderSize, borderShape, ref cellData))
                {
                    spawned.Add(cellData);
                    continue;
                }

                i--;
                unsuccessful++;
            }

            return spawned;
        }

        protected virtual CellData SetUpCell(BorderShape borderShape, float borderSize)
        {
            float orbitalPos;
            var pos = GetPosition(borderSize, borderShape, out orbitalPos);
            var orbitalSize = orbitalDistribution.Evaluate(orbitalPos);
            var size = minSize + (maxSize - minSize) * massDistribution.Evaluate(orbitalSize);
            var cellData = new CellData
            {
                position = pos,
                cellMass = {size = size}
            };
            return cellData;
        }

        private Vector2 GetPosition(float borderSize, BorderShape borderShape)
        {
            return GetPosition(borderSize, borderShape, out _);
        }

        protected virtual Vector2 GetPosition(float borderSize, BorderShape borderShape, out float orbitalPos)
        {
            throw new NotImplementedException();
        }
    }
}