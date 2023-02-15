using UnityEngine;
using Zenject;

namespace Data
{
    [CreateAssetMenu(fileName = "Survive", menuName = "ScriptableObjects/VictoryConditions/Survive", order = 1)]
    public class Survive : VictoryCheck
    {
        public float timeToPasst = 10;
        protected override bool CheckForVictory(float time)
        {
            return timeToPasst < time;
        }
    }
}