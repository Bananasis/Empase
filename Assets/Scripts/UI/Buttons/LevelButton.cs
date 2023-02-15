using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(CellButtonMultiplier))]
[RequireComponent(typeof(Animator))]
public class LevelButton : MonoBehaviour
{
    [Inject] private ILevelLoader _levelLoader;
    [Inject] private GameManager _gameManager;
    [Inject] private IWindowManager _windowManager;
    [SerializeField] private int levelId;
    private Animator _animator;
    private Button button;

    private CellButtonMultiplier cell;

    // Start is called before the first frame update
    void Start()
    {
        
        _animator = GetComponent<Animator>();
        button = GetComponent<Button>();
        cell = GetComponent<CellButtonMultiplier>();
        UpdateButton(_gameManager.Levels[levelId],true);
        button.onClick.AddListener(() => _windowManager.CloseWindowsUntil(WindowType.Loading,
            () => _levelLoader.LoadLevel(levelId,
                () => _windowManager.OpenWindow(WindowType.GameplayUI, true)), true));

        _gameManager.Levels.GetEvent(levelId).AddListener(UpdateButton);
    }

    private void UpdateButton(LevelCompletion state, bool initial)
    {
        UpdateButton(state);
        if (!initial) return;
        var _animation = state switch
        {
            LevelCompletion.Closed => "Disabled",
            LevelCompletion.Open => "Normal",
            LevelCompletion.Passed => "Normal",
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };
        _animator.Play(_animation);
    }

    private void UpdateButton(LevelCompletion state)
    {
        if (state == LevelCompletion.Closed)
        {
            button.interactable = false;
            return;
        }
        cell.SetId(levelId);
        button.interactable = true;
        cell.passed = state != LevelCompletion.Passed;
    }
}