using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Zenject;

[CreateAssetMenu(fileName = "AchievementEvent", menuName = "ScriptableObjects/AchievementEvent", order = 1)]
public class AchievementEvent : GameEvent
{
    [Inject] private GameManager _gameManager;


    protected override IAnnouncement _announcement => announcement;
    private Announcement announcement;
    [FormerlySerializedAs("achivements")] [SerializeField] private List<AchievementData> achievements = new List<AchievementData>();
    public readonly Dictionary<Achievement, AchievementData> achievementDict = new Dictionary<Achievement, AchievementData>();

    public override void Activate()
    {
        achievements.ForEach((a) =>
        {
            achievementDict[a.achievement] = a;
            a.unlockEvent.SubscribePersistent(() => _gameManager.Achievements[a.achievement] = true);
        });
        base.Activate();
        _gameManager.Achievements.OnChange.Subscribe((b) =>
        {
            if (!b.Item2) return;
            var data = achievementDict[b.Item1];
            announcement = new Announcement
            {
                PopupDatas = new List<PopupData>
                {
                    new PopupData {type = PopupType.Achievement, duration = 7, sprite = data.sprite, text = data.title},
                    new PopupData {type = PopupType.Unlock, duration = 3, text = "Achievement unlocked!", vol = true}
                }
            };
            OnEvent.Invoke();
        });
    }
}

[Serializable]
public struct AchievementData
{
    public Achievement achievement;
    public Sprite sprite;
    public string title;
    public string description;
    public GameEvent unlockEvent;
}