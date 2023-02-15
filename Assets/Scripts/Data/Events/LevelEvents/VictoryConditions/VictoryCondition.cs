using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class VictoryCondition : LevelEvent
    {
        protected override IAnnouncement _announcement => new Announcement()
        {
            PopupDatas = new List<PopupData>()
            {
                new PopupData()
                {
                    type = PopupType.Comment,
                    text = "Congrats!",
                    duration = Single.MaxValue,
                    vol = true,
                    force = true
                },
                new PopupData()
                {
                    type = PopupType.Tip,
                    text = "Press 'Space' to continue\n",
                    duration = Single.MaxValue,
                    vol = true,
                    force = true
                }
            }
        };

        public override void Activate()
        {
            base.Activate();
            connections.Add(OnEvent.Subscribe(_levelManager.Win));
        }

        protected override void OnStateChange(LevelState state)
        {
            if (state != LevelState.Began) Deactivate();
        }
    }
}