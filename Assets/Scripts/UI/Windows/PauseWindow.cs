using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PauseWindow : AlphaWindow
{
    [Inject] private ILevelManager _levelManager;
    [Inject] private ILevelLoader _levelLoader;
    [SerializeField] private Button restart;
    [SerializeField] private Button mainMenu;

    public override void Init()
    {
        base.Init();
        OnWindowClose.AddListener(() => _levelManager.SetPause(false));
        OnWindowOpen.AddListener(() => _levelManager.SetPause(true));
        restart.onClick.AddListener(
            () => _windowManager.OpenWindow(WindowType.Loading, true, onOpen:
                () => _levelLoader.Restart(
                    () => _windowManager.CloseTopWindow())));
        mainMenu.onClick.AddListener(() =>
            {
                _levelLoader.CloseLevel();
                _windowManager.CloseWindowsUntil(WindowType.MainMenu, skipMiddle: true);
            }
        );

        shortcuts[KeyCode.Escape] = () => _windowManager.CloseTopWindow();
        shortcuts[KeyCode.R] = restart.onClick.Invoke;
        shortcuts[KeyCode.M] = mainMenu.onClick.Invoke;
    }
}