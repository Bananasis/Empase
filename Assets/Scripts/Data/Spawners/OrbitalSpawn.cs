using System.Collections.Generic;
using UnityEngine;

namespace Data.Spawners
{
    [CreateAssetMenu(fileName = "OrbitalSpawn", menuName = "ScriptableObjects/Spawners/OrbitalSpawn", order = 1)]
    public class OrbitalSpawn : ProceduralSpawn
    {
        [SerializeField] private bool uniform;
        [SerializeField] private float minOrbit = 0;
        [SerializeField] private float maxOrbit = 1;
        [SerializeField] private float orbitalSpeedMultiplier;
        [SerializeField] private float orbitalSpeedShift;
        [SerializeField] private Vector2 rotationalCenter;


 

        protected override CellData SetUpCell(BorderShape borderShape, float borderSize)
        {
            float orbitalPos;
            var pos = GetPosition(borderSize, borderShape, out orbitalPos);
            var orbitalSize = orbitalDistribution.Evaluate(orbitalPos);
            var size = minSize + (maxSize - minSize) * massDistribution.Evaluate(orbitalSize);
            var vec = pos - rotationalCenter;
            var dir = vec.normalized;
            var mag = vec.magnitude;
            var cellData = new CellData
            {
                position = pos,
                size = size,
                velocity = orbitalSpeedMultiplier / Mathf.Sqrt(mag-orbitalSpeedShift) * new Vector2(dir.y, -dir.x)
            };
            return cellData;
        }


        protected override Vector2 GetPosition(float borderSize, BorderShape borderShape, out float orbitalPos)
        {
            Vector2 pos;
            orbitalPos = 0;
            if (uniform)
            {
                var circ = random.UnitCircle();
                var magnitude = circ.magnitude;
                var dir = circ.normalized;
                orbitalPos = magnitude;
                pos = dir * (orbitalPos * (maxOrbit - minOrbit) + minOrbit);
            }
            else
            {
                var circ = random.UnitCircle();
                var dir = circ.normalized;
                orbitalPos = random.NextFloat();
                pos = dir * (orbitalPos * (maxOrbit - minOrbit) + minOrbit);
            }

            return pos * borderSize;
        }
    }
}