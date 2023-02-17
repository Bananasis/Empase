using System;
using UnityEngine;

namespace Data.Spawners
{
    [CreateAssetMenu(fileName = "FillSpawn", menuName = "ScriptableObjects/Spawners/FillSpawn", order = 1)]
    public class FillSpawn : ProceduralSpawn
    {
        [SerializeField] private AnimationCurve sizeSpeed = AnimationCurve.Linear(0, 1, 1, 0);
        [SerializeField] private float maxSpeed;
        [SerializeField] private float minSpeed;

        protected override Vector2 GetPosition(float borderSize, BorderShape borderShape, out float orbitalPos)
        {
            orbitalPos = 0;
            orbitalPos = random.NextFloat();
            Vector2 pos = borderShape switch
            {
                BorderShape.Square => new Vector2(random.NextFloat() - 0.5f, random.NextFloat() - 0.5f) * 2,
                BorderShape.Circle => random.UnitCircle(),
                _ => throw new ArgumentOutOfRangeException(nameof(borderShape), borderShape, null)
            };
            return pos * borderSize;
        }

        protected override CellData SetUpCell(BorderShape borderShape, float borderSize)
        {
            float orbitalPos;
            var pos = GetPosition(borderSize, borderShape, out orbitalPos);
            var orbitalSize = orbitalDistribution.Evaluate(orbitalPos);
            var sizeMult = massDistribution.Evaluate(orbitalSize);
            var size = minSize + (maxSize - minSize) * sizeMult;
            var speed = minSpeed + (maxSpeed - minSpeed) * sizeSpeed.Evaluate(sizeMult);
            var cellData = new CellData
            {
                position = pos,
                cellMass = {size = size},
                velocity = random.UnitCircle() * speed
            };
            return cellData;
        }
    }
}