using UnityEngine;
using Zenject;

namespace Data
{
    [CreateAssetMenu(fileName = "BecomeBiggest", menuName = "ScriptableObjects/VictoryConditions/BecomeBiggest", order = 1)]
    public class BecomeBiggest:VictoryCheck
    {
        
        protected override bool CheckForVictory(float time)
        {
            return _gCellData.playerSize >= _gCellData.maxCellSize;
        }
    }
}