
    using UnityEngine;

    public interface IGravityManager : IRegistry<GravityMassDot>
    {
        Vector2 GetAcceleration(CellData cellData);
    }

    public partial class GravityManager : IGravityManager
    {
        public Vector2 GetAcceleration(CellData cellData)
        {
            var acc = Vector2.zero;
            foreach (var attractor in reg)
            {
                acc += attractor.GetAcceleration(cellData);
            
            }

            return acc;
        }
    }