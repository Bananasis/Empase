using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsWindow : AlphaWindow
{
    public override void Init()
    {
        base.Init();
        shortcuts[KeyCode.Escape] = () => _windowManager.CloseTopWindow();
    }
}
