using UnityEngine;

namespace Data.Events.LevelEvents.GenericLevelEvents
{
    
    [CreateAssetMenu(fileName = "OnTimeMark", menuName = "ScriptableObjects/GenericEvents/OnTimeMark", order = 1)]
    public class OnTimeMark:OnLevelBegin
    {
        protected override float timeToPass => timeMark;

        [SerializeField]
        protected float timeMark;
    }
}