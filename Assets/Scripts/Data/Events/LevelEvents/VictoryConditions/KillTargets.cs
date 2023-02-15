using UnityEngine;
using Zenject;

namespace Data
{
    [CreateAssetMenu(fileName = "KillTargets", menuName = "ScriptableObjects/VictoryConditions/KillTargets", order = 1)]
    public class KillTargets:VictoryCheck
    {
 
        protected override bool CheckForVictory(float time)
        {
            return _levelManager.targetsLeft == 0;
        }
    }
}