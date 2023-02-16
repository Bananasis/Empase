using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuWindow : AlphaWindow
{
    [Inject] private ILevelLoader _levelLoader;
    [Inject] private GameManager _gameManager;
    [SerializeField] private Button play;
    [SerializeField] private Button achievements;
    [SerializeField] private Button levels;
    [SerializeField] private Button settings;
    [SerializeField] private Button cell;

    // Start is called before the first frame update
    
    public override void Init()
    {
        base.Init();
        play.onClick.AddListener(() =>
        {
            
            if (_gameManager.CurrentLevel.val == -1)
            {
                _windowManager.OpenWindow(WindowType.Levels);
                return;
            }

            _windowManager.OpenWindow(WindowType.Loading, true,
                onOpen: () =>
                    _levelLoader.LoadLevel(_gameManager.CurrentLevel.val,
                        () => _windowManager.OpenWindow(WindowType.GameplayUI, true)));
        });
       
        achievements.onClick.AddListener(() => _windowManager.OpenWindow(WindowType.Achievements));
        levels.onClick.AddListener(() => _windowManager.OpenWindow(WindowType.Levels));
        settings.onClick.AddListener(() => _windowManager.OpenWindow(WindowType.Settings));
        cell.onClick.AddListener(() => _windowManager.OpenWindow(WindowType.Cell));

        if (!_gameManager.Achievements[Achievement.Consume])
        {
            achievements.interactable = false;
            achievements.animator.Play("Disabled");
            _gameManager.Achievements.GetEvent(Achievement.Consume)
                .SubscribeOnce((_) => achievements.interactable = true);
        }

        if (_gameManager.Levels[0] == LevelCompletion.Open)
        {
            levels.interactable = false;
            levels.animator.Play("Disabled");
            _gameManager.Levels.GetEvent(0)
                .SubscribeOnce((_) => levels.interactable = true);
        }

        if (!_gameManager.ShiftAbilities[ShiftType.Time])
        {
            cell.interactable = false;
            cell.animator.Play("Disabled");
            _gameManager.ShiftAbilities.GetEvent(ShiftType.Time)
                .SubscribeOnce((_) => cell.interactable = true);
        }


        shortcuts[KeyCode.S] = settings.onClick.Invoke;
        shortcuts[KeyCode.P] = play.onClick.Invoke;
        shortcuts[KeyCode.A] = achievements.onClick.Invoke;
        shortcuts[KeyCode.L] = levels.onClick.Invoke;
        shortcuts[KeyCode.C] = cell.onClick.Invoke;
    }
}