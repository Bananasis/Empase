using UnityEngine;
using Zenject;

namespace Data
{
    [CreateAssetMenu(fileName = "CollectMass", menuName = "ScriptableObjects/VictoryConditions/CollectMass", order = 1)]
    public class CollectMass : VictoryCheck
    {
        public bool withPlayer;
        public bool abs;
        public float massPercent = 0.5f;
        protected override bool CheckForVictory(float time)
        {
            return (abs ? _gCellData.sumAbsMass : _gCellData.sumMass) * massPercent <
                   _gCellData.playerSize*_gCellData.playerSize*Mathf.PI;
        }
    }
}