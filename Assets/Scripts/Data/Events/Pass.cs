using System;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Pass", menuName = "ScriptableObjects/GenericEvents/Pass", order = 1)]
public class Pass : GameEvent
{
    [SerializeField] protected PassType type;
    [SerializeField] protected int number;
    [Inject] private GameManager _gameManager;
    protected override IAnnouncement _announcement => null;

    public override void Activate()
    {
        base.Activate();
        switch (type)
        {
            case PassType.ZoneOpen:
                if (_gameManager.LastOpenZone.val >= number)
                {
                    Deactivate();
                    return;
                }

                connections.Add(_gameManager.LastOpenZone.OnChange.Subscribe((z) =>
                    {
                        if (z != number) return;
                        OnEvent.Invoke();
                        Deactivate();
                    }
                ));
                break;
            case PassType.LevelOpen:
                if (_gameManager.Levels[number] != LevelCompletion.Closed)
                {
                    Deactivate();
                    return;
                }

                connections.Add(_gameManager.Levels.GetEvent(number).Subscribe((z) =>
                    {
                        if (z != LevelCompletion.Open) return;
                        OnEvent.Invoke();
                        Deactivate();
                    }
                ));
                break;
            case PassType.LevelPassed:
                if (_gameManager.Levels[number] == LevelCompletion.Passed)
                {
                    Deactivate();
                    return;
                }

                connections.Add(_gameManager.Levels.GetEvent(number).Subscribe((z) =>
                    {
                        if (z != LevelCompletion.Passed) return;
                        OnEvent.Invoke();
                        Deactivate();
                    }
                ));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}


public enum PassType
{
    ZoneOpen,
    LevelOpen,
    LevelPassed,
}