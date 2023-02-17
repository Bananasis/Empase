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
        player.OnSizeChange.Subscribe((s) =>
        {
            playerSize = s.Item1;
            playerMass = s.Item2;
        });
        player.OnSizeChange.Subscribe((s) =>
        {
            maxPlayerSize = Mathf.Max(s.Item1, maxPlayerSize);
            maxPlayerMass = Mathf.Max(s.Item2, maxPlayerMass);
        });
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
        var rPlayerSize = playerSize > 0 ? 1 / playerSize : 1;
        foreach (var cell in reg)
        {
            cell.lData.sizeRatio = (-playerSize + cell.cData.cellMass.size) * rPlayerSize;
            cell.lData.timeMultiplier = _timeDialitionManager.GetTime(cell.cData);
            cell.lData.gravityAcceleration = _gravityManager.GetAcceleration(cell.cData);
            maxCellSize = Mathf.Max(cell.cData.cellMass.size, maxCellSize);
            sumMass += cell.cData.cellMass.mass;
            sumAbsMass += cell.cData.cellMass.mass * cell.cData.massMultiplier;
        }
    }
}