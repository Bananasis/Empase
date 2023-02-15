using System.Collections.Generic;
using UnityEngine;
using Utils;
using Random = System.Random;

namespace Data.Spawners
{
    [CreateAssetMenu(fileName = "ListSpawn", menuName = "ScriptableObjects/Spawners/ListSpawn", order = 1)]
    public class ListSpawn : Spawner
    {
        [SerializeField] private List<CellData> list;

        public override List<CellData> Spawn(SimpleCollisionTree sct, float borderSize, BorderShape borderShape)
        {
            random = new Random();
            List<CellData> spawned = new List<CellData>();
            foreach (var cellData in list)
            {
                var cd = cellData;
                if (!TrySpawn(sct, borderSize, borderShape, ref cd))
                    throw new GameException("Cant Spawn!");
                spawned.Add(cd);
            }

            return spawned;
        }
    }
}