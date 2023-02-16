using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameplayUIWindow : AlphaWindow
{
    [Inject] private ILevelLoader _levelLoader;
    [Inject] private ILevelManager _levelManager;
    [SerializeField] private Button pause;
    
    public override void Init()
    {
        base.Init();
        pause.onClick.AddListener(() => {_windowManager.OpenWindow(WindowType.Pause); });
        shortcuts[KeyCode.Escape] = pause.onClick.Invoke;
        shortcuts[KeyCode.R] = () => _windowManager.OpenWindow(WindowType.Loading, onOpen:
            () => _levelLoader.Restart(
                () => _windowManager.CloseTopWindow()));
        shortcuts[KeyCode.Space] = () => _levelLoader.Continue();
    }



  

   
}

