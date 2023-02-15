using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public partial class CellManager : Registry<Cell>
{
    [Inject] private IGravityManager _gravityManager;
    [Inject] public ITimeDialitionManager _timeDialitionManager;

    private void Start()
    {
        var player = _cellPool.Player;
        player.OnSizeChange.Subscribe((s) => playerSize = s.Item1);
        player.OnSizeChange.Subscribe((s) => maxPlayerSize = Mathf.Max(s.Item1, maxPlayerSize));
    }

    private void FixedUpdate()
    {
        Calculate();
    }

    void Calculate()
    {
        maxCellSize = 0;
        sumMass = 0;
        sumAbsMass = 0;
        foreach (var cell in reg)
        {
            cell.lData.sizeRatio = -playerSize + cell.cData.size;
            cell.lData.timeMultiplier = _timeDialitionManager.GetTime(cell.cData);
            cell.lData.gravityAcceleration = _gravityManager.GetAcceleration(cell.cData);
            maxCellSize = Mathf.Max(cell.cData.size, maxCellSize);
            sumMass += cell.cData.mass;
            sumAbsMass += cell.cData.mass * cell.cData.massMultiplier;
        }
    }
}