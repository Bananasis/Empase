using UnityEngine;
using Zenject;

namespace Data
{
    [CreateAssetMenu(fileName = "GoToPlace", menuName = "ScriptableObjects/VictoryConditions/GoToPlace", order = 1)]
    public class GoToPlace:VictoryCheck
    {

        public Vector2 pos;
        public float proximity = 5;
        protected override bool CheckForVictory(float time)
        {
            return (_gCellData.playerPosition - pos).sqrMagnitude < proximity * proximity;
        }
    }
}