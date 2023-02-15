using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


[Serializable]
public struct ShiftTextUnlockData
{
    public ShiftType type;
    public string text;
    public GameEvent unlockEvent;
    public string description;
    public string lockedDescription;
    public string name;
    public Sprite image;
}

[Serializable]
public struct PropulsionUnlockData
{
    public PropulsionType type;
    public string text;
    public GameEvent unlockEvent;
    public string description;
    public string lockedDescription;
    public string name;
    public Sprite image;
}

[CreateAssetMenu(fileName = "Unlock", menuName = "ScriptableObjects/Unlock", order = 1)]
public class Unlock : GameEvent
{
    [SerializeField] private List<PropulsionUnlockData> propulsion;
    [SerializeField] private List<ShiftTextUnlockData> shift;
    [Inject] private GameManager _gameManager;
    public Dictionary<ShiftType, ShiftTextUnlockData> shiftDict = new Dictionary<ShiftType, ShiftTextUnlockData>();

    public Dictionary<PropulsionType, PropulsionUnlockData> propulsionDict =
        new Dictionary<PropulsionType, PropulsionUnlockData>();

    protected override IAnnouncement _announcement => announcement;
    private IAnnouncement announcement;

    public override void Activate()
    {
        propulsion.ForEach((pt) =>
        {
            propulsionDict[pt.type] = pt;
            pt.unlockEvent?.SubscribePersistent(() => _gameManager.MouseAbilities[pt.type] = true);
        });
        shift.ForEach((pt) =>
        {
            shiftDict[pt.type] = pt;
            pt.unlockEvent?.SubscribePersistent(() => _gameManager.ShiftAbilities[pt.type] = true);
        });
        base.Activate();
        _gameManager.ShiftAbilities.OnChange.Subscribe((b) =>
        {
            if (!b.Item2) return;
            announcement = new Announcement
            {
                PopupDatas = new List<PopupData>
                {
                    new PopupData {type = PopupType.Unlock, duration = 5, text = shiftDict[b.Item1].text}
                }
            };
            OnEvent.Invoke();
        });

        _gameManager.MouseAbilities.OnChange.Subscribe((b) =>
        {
            if (!b.Item2) return;
            announcement = new Announcement
            {
                PopupDatas = new List<PopupData>
                {
                    new PopupData {type = PopupType.Unlock, duration = 5, text = propulsionDict[b.Item1].text}
                }
            };
            OnEvent.Invoke();
        });
    }
}