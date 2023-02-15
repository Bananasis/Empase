using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AchievementsWindow : AlphaWindow
{
    [SerializeField] private List<AchievementPanel> _achievementPanels = new List<AchievementPanel>();
    [SerializeField] private GridLayoutGroup layout;
    [Inject] private EventManager _eventManager;
    [SerializeField] private Vector2 cellSizeRatio;
    [SerializeField] private Canvas canvas;

    public override void Init()
    {
        base.Init();

        shortcuts[KeyCode.Escape] = () => _windowManager.CloseTopWindow();
    }

    public void Start()
    {
        for (var i = 0; i < _achievementPanels.Count; i++)
        {
            var ach = (Achievement) i;
            var ae = _eventManager.GetAchievementData(ach);
            _achievementPanels[i].Set(ae.sprite, ae.title,
                ae.description, _achievementPanels.Count - i + 1,
                (Achievement) i);
        }
    }

    private Vector2 _cachedScreenSize;

    void Update()
    {
        Vector2 screenSize = canvas.pixelRect.max;

        if (_cachedScreenSize == screenSize)
            return;
        layout.cellSize = screenSize.x * cellSizeRatio;


        // _rect.
        _cachedScreenSize = screenSize;
    }
}