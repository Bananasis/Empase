using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "OnDireSituation", menuName = "ScriptableObjects/GenericEvents/OnDireSituation", order = 1)]
public class OnDireSituation : LevelEvent
{
    [SerializeField] private float threshold = 0.5f;
    [Inject] private ICellPool _cellPool;

    public override void Activate()
    {
        base.Activate();
        connections.Add(_cellPool.Player.OnSizeChange.Subscribe((sizeMass) =>
        {
            if (sizeMass.Item2 > _gCellData.maxPlayerMass * threshold) return;
            OnEvent.Invoke();
            Deactivate();
        }));
    }

    protected override IAnnouncement _announcement => new Announcement()
    {
        PopupDatas = new List<PopupData>()
        {
            new PopupData()
            {
                type = PopupType.Comment, text = "Looks bad", force = true, vol = true, duration = Single.MaxValue,
            },
            new PopupData()
            {
                type = PopupType.Tip, text = "Press 'r' restart", force = true, vol = true, duration = Single.MaxValue,
            },
        }
    };

    protected override void OnStateChange(LevelState state)
    {
        if (state != LevelState.Began) Deactivate();
    }
}