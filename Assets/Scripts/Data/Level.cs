using System.Collections.Generic;
using Data;
using Data.Events.LevelEvents;
using UnityEngine;
using Zenject;


[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level", order = 1)]
public class Level : ScriptableObject
{
    public BorderShape borderShape;
    public BorderType BorderType;
    public float fieldRadius;
    [Inject] private GameManager _gameManager;
    [Inject] private EventManager _eventManager;

    public int id => _id;
    [SerializeField] private int _id;
    [SerializeField] private List<int> nextLevels = new List<int>();
    [SerializeField] private int nextZone;
    [SerializeField] private List<Spawner> _spawners;
    [SerializeField] private List<VictoryCondition> victory = new List<VictoryCondition>();
    [SerializeField] private List<DefeatCondition> defeat = new List<DefeatCondition>();
    [SerializeField] private List<LevelEvent> events = new List<LevelEvent>();
    [SerializeField] public float massDefect = 1;
    [HideInInspector] public List<CellData> cells;
    [HideInInspector] public List<CellData> targets;
    public CellData playerCellData;
    public List<CellEvent> playerEvents;


    public void Spawn()
    {
        SimpleCollisionTree sct = new SimpleCollisionTree(new Rect()
        {
            min = new Vector2(-fieldRadius, -fieldRadius), max = new Vector2(fieldRadius, fieldRadius)
        }, 4);
        cells = new List<CellData>();
        targets = new List<CellData>();
        sct.Add(playerCellData);
        _spawners.ForEach((sp) =>
        {
            if (sp.isLevelGoal)
            {
                targets.AddRange(sp.Spawn(sct, fieldRadius, borderShape));
                return;
            }

            cells.AddRange(sp.Spawn(sct, fieldRadius, borderShape));
        });
    }

    public void Pass()
    {
        _gameManager.Levels[id] = LevelCompletion.Passed;
        if (nextZone > _gameManager.LastOpenZone.val)
        {
            _gameManager.LastOpenZone.val++;
        }

        foreach (var nextLevel in nextLevels)
        {
            if (_gameManager.Levels[nextLevel] == LevelCompletion.Closed)
            {
                _gameManager.Levels[nextLevel] = LevelCompletion.Open;
            }
        }

        if (nextLevels.Count == 1)
        {
            _gameManager.CurrentLevel.val = nextLevels[0];
            _gameManager.Save();
            return;
        }

        _gameManager.CurrentLevel.val = -1;
        _gameManager.Save();
    }

    public void ActivateEvents(LevelState state,Player player)
    {
        void Activate(LevelEvent e)
        {
            if (state == LevelState.Restart && e.persistOnReload) return;
            e.Activate();
        }
        void ActivatePlayer(CellEvent e)
        {
            if (state == LevelState.Restart && e.persistOnReload) return;
            e.Activate(player);
        }
        playerEvents.ForEach(ActivatePlayer);
        _eventManager.globalPlayerEvents.ForEach(ActivatePlayer);
        victory.ForEach(Activate);
        defeat.ForEach(Activate);
        events.ForEach(Activate);
        _eventManager.globalLevelEvents.ForEach(Activate);
    }
}