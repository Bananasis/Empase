using System;
using UnityEngine.Events;

public interface ILevelManager
{
    PauseState paused { get; }
    int targetsLeft { get; }
    void SetPause(bool p0);
    UnityEvent<float> OnLevelTick { get; }
    public UnityEvent<LevelState> OnLevelStateChange { get; }
    UnityEvent<PauseState> OnPauseState { get; }

    public void Loose();
    public void Win();
}

public interface ILevelLoader
{
    void CloseLevel();
    void Restart(Action action);
    void LoadLevel(int level, Action onLoad = default);
    void Continue();
}

public partial class LevelManager : ILevelManager, ILevelLoader
{
    public UnityEvent<PauseState> OnPauseState { get; } = new UnityEvent<PauseState>();
    public UnityEvent<LevelState> OnLevelStateChange { get; } = new UnityEvent<LevelState>();
    public UnityEvent<float> OnLevelTick { get; } = new UnityEvent<float>();

    public PauseState paused
    {
        get => _paused;
        private set
        {
            if (_paused == value) return;
            _paused = value;
            OnPauseState.Invoke(_paused);
        }
    }

    private PauseState _paused;
    public int targetsLeft { get; set; }

    public void LoadLevel(int levelId, Action onLoad = default)
    {
        LoadLevel(levelHolder.GetLevel(levelId), onLoad);
    }

    public void Continue()
    {
        if (state == LevelState.Won)
        {
            _windowManager.OpenWindow(WindowType.Loading, onOpen: () =>
            {
                CloseLevel();
                var nextLevelId = _gameManager.CurrentLevel.val;
                if (-1 != nextLevelId)
                {
                    LoadLevel(nextLevelId, () => _windowManager.CloseTopWindow());
                    return;
                }

                _windowManager.CloseWindowsUntil(WindowType.MainMenu, skipMiddle: true);
            });
        }

        if (state == LevelState.Lost)
            Restart();
    }

    public void SetPause(bool pause)
    {
        if (pause == (paused == PauseState.Pause || paused == PauseState.Pausing)) return;
        if (pause != (paused == PauseState.Play || paused == PauseState.UnPausing)) return;
        Pause();
    }


    public void Restart(Action act = default)
    {
        CloseLevel(true);
        LoadLevel(_lastLevel, act);
    }

    public void CloseLevel()
    {
        CloseLevel(false);
    }

    private void CloseLevel(bool reload)
    {
        state = reload ? LevelState.Restart : LevelState.NotLoaded;
        connections.Dispose();
        _popupManager.ClearLevelPopups();
        _cellManager.DeactivateAll();
        circleBorder.gameObject.SetActive(false);
        squareBorder.gameObject.SetActive(false);
    }


    public void Loose()
    {
        if (state == LevelState.Won) return;
        state = LevelState.Lost;
    }

    public void Win()
    {
        _lastLevel.Pass();
        state = LevelState.Won;
    }
}

public enum PauseState
{
    Pause,
    Play,
    Pausing,
    UnPausing,
}

public enum LevelState
{
    NotLoaded,
    Restart,
    Began,
    Won,
    Lost
}