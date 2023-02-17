using UnityEngine;
using Zenject;

public interface ICellManager : IRegistry<Cell>
{
    public void DeactivateAll();
}

public interface IGlobalCellStats
{
    public float playerSize { get; }
    public float playerMass { get; }
    public Vector2 playerPosition { get; set; }
    public float sumAbsMass { get; }
    public float sumMass { get; }
    public float maxPlayerSize { get; set; }
    public float maxCellSize { get; set; }
    public float massDefect { get; set; }
    public float maxPlayerMass { get; }
    void Reset();
}

public partial class CellManager : ICellManager, IGlobalCellStats
{
    [Inject] private ICellPool _cellPool;

    public void DeactivateAll()
    {
        for (var i = reg.Count - 1; i > -1; --i)
        {
            reg[i].Die();
        }
    }

    public void Reset()
    {
        var player = _cellPool.Player;
        playerSize = player.cData.cellMass.size;
        playerPosition = player.cData.position;
        maxPlayerSize = player.cData.cellMass.size;
        Calculate();
    }

    public float playerSize { get;private set; }
    public float playerMass { get;private set; }
    public Vector2 playerPosition { get; set; }
    public float sumAbsMass { get; private set; }
    public float sumMass { get; private set; }
    public float maxPlayerSize { get; set; }
    public float maxCellSize { get; set; }
    public float massDefect { get; set; } = 1;
    public float maxPlayerMass { get; private set; }
}