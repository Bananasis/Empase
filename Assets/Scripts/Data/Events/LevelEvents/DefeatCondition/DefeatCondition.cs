using System;
using System.Collections.Generic;

public class DefeatCondition : LevelEvent
{
    protected override IAnnouncement _announcement =>
        new Announcement()
        {
            PopupDatas = new List<PopupData>()
            {
                new PopupData()
                {
                    type = PopupType.Comment,
                    text = "Oops!",
                    duration = Single.MaxValue,
                    vol = true,
                    force = true
                },
                new PopupData()
                {
                    type = PopupType.Tip,
                    text = "Press 'Space' to restart level",
                    duration = Single.MaxValue,
                    vol = true,
                    force = true
                }
            }
        };

    public override void Activate()
    {
        base.Activate();
        connections.Add(OnEvent.Subscribe(_levelManager.Loose));
    }

    protected override void OnStateChange(LevelState state)
    {
        if (state != LevelState.Began) Deactivate();
    }
}