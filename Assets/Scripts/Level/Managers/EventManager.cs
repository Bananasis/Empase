using System;
using System.Collections;
using System.Collections.Generic;
using Data.Events.LevelEvents;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] private List<GameEvent> _activeGameEvents = new List<GameEvent>();
    public List<CellEvent> globalPlayerEvents;
    public List<LevelEvent> globalLevelEvents;
    public List<GameEvent> globalEvents;
    [SerializeField] private AchievementEvent achievementEvent;
    [SerializeField] private Unlock unlockEvent;

    private void Awake()
    {
    }

    private void Start()
    {
        achievementEvent.Activate();
        unlockEvent.Activate();
        globalEvents.ForEach((g) => g.Activate());
    }

    public void Add(GameEvent gameEvent)
    {
        _activeGameEvents.Add(gameEvent);
    }

    public void Remove(GameEvent gameEvent)
    {
        _activeGameEvents.Remove(gameEvent);
    }

    public AchievementData GetAchievementData(Achievement achievement)
    {
        return achievementEvent.achievementDict[achievement];
    }

    public ShiftTextUnlockData GetShiftData(ShiftType type)
    {
        return unlockEvent.shiftDict[type];
    }

    public PropulsionUnlockData GetPropulsionData(PropulsionType type)
    {
        return unlockEvent.propulsionDict[type];
    }
}