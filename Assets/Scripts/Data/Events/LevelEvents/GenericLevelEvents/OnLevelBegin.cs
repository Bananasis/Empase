using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OnLevelBegin", menuName = "ScriptableObjects/GenericEvents/OnLevelBegin", order = 1)]
public class OnLevelBegin : OnTimer
{
    public override bool persistOnReload => true;
    [SerializeField] private string title = "Level";
    [SerializeField] private string comment = "Survive";
    [SerializeField] private string tip = "Don't die";

    protected override IAnnouncement _announcement => new Announcement()
    {
        PopupDatas = new List<PopupData>()
        {
            new PopupData()
            {
                type = PopupType.Comment, text = comment, force = true, vol = true, duration = 10
            },
            new PopupData()
            {
                type = PopupType.Title, text = title, force = true, vol = true, duration = 10
            },
            new PopupData()
            {
                type = PopupType.Tip, text = tip, force = true, vol = true, duration = 20
            },
        }
    };

    protected override float timeToPass => 0;
}