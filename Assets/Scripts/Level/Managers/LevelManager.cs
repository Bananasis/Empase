using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using Zenject;


public partial class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelHolder levelHolder;
    [SerializeField] private LevelBorderCollider circleBorder;
    [SerializeField] private LevelBorderCollider squareBorder;
    [Inject] private ICellManager _cellManager;
    [Inject] private IWindowManager _windowManager;
    [Inject] private ICellPool _cellPool;
    [Inject] private IGlobalCellStats _gCellData;
    [Inject] private GameManager _gameManager;
    [Inject] private PopupManager _popupManager;
    private float _timePassed = 0;
    private readonly List<IDisposable> connections = new List<IDisposable>();
    private LevelState _state;

    private LevelState state
    {
        get => _state;
        set
        {
            if (_state == value) return;
            _state = value;
            OnLevelStateChange.Invoke(_state);
        }
    }

    private float deltaTime;
    private List<CellData> cells;
    private List<CellData> targets;
    private Level _lastLevel;


    private void Awake()
    {
        deltaTime = Time.fixedDeltaTime;
    }


    private void LoadLevel(Level level, Action onLoad = default)
    {
        _lastLevel = level;
        switch (level.borderShape)
        {
            case BorderShape.Square:
                circleBorder.gameObject.SetActive(false);
                squareBorder.gameObject.SetActive(true);
                squareBorder.Generate(level.fieldRadius, level.BorderType == BorderType.Collider);

                break;
            case BorderShape.Circle:
                squareBorder.gameObject.SetActive(false);
                circleBorder.gameObject.SetActive(true);
                circleBorder.Generate(level.fieldRadius, level.BorderType == BorderType.Collider);

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        Time.fixedDeltaTime = 0;
        Time.timeScale = 0;
        paused = PauseState.Pause;

        if (state != LevelState.NotLoaded)
        {
            BeginLevel(level);
            onLoad?.Invoke();
            return;
        }

        StartCoroutine(WaitForSpawn(level, () =>
        {
            cells = level.cells;
            targets = level.targets;
            level.cells = null;
            level.targets = null;
            BeginLevel(level);
            onLoad?.Invoke();
        }));
    }

    private void BeginLevel(Level level)
    {
        var player = _cellPool.CellGetPlayer(level.playerCellData);
        _gCellData.massDefect = level.massDefect;
        cells.ForEach((cd) => _cellPool.GetCell(cd));
        targetsLeft = targets.Count;
        _timePassed = 0;
        targets.ForEach((cd) =>
            {
                var cell = _cellPool.GetCell(cd);
                connections.Add(cell.OnDeath.SubscribeOnce(() => { targetsLeft--; }));
            }
        );
        _gCellData.Reset();
        level.ActivateEvents(_state, player);

        state = LevelState.Began;
        SetPause(false);
    }


    IEnumerator WaitForSpawn(Level level, Action act)
    {
        Thread _thread = new Thread(level.Spawn);
        _thread.Start();
        while (_thread.IsAlive)
        {
            yield return null;
        }

        act.Invoke();
    }

#if UNITY_EDITOR
        private void Update()
    {
        if (state != LevelState.Began) return;
        if (Input.GetKey(KeyCode.V))
            Win();
            Continue();
    }
#endif


    private void FixedUpdate()
    {
        if (LevelState.Restart == _state || _state == LevelState.NotLoaded) return;
        _timePassed += Time.fixedDeltaTime;
        OnLevelTick.Invoke(_timePassed);
    }


    private void Pause()
    {
        switch (paused)
        {
            case PauseState.Pause:
                paused = PauseState.UnPausing;
                StartCoroutine(SoftPause());
                break;
            case PauseState.Play:
                paused = PauseState.Pausing;
                StartCoroutine(SoftPause());
                break;
            case PauseState.Pausing:
                paused = PauseState.UnPausing;
                break;
            case PauseState.UnPausing:
                paused = PauseState.Pausing;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    IEnumerator SoftPause()
    {
        float timeScale = paused == PauseState.Pausing ? 1 : 0;
        while (true)
        {
            timeScale += paused == PauseState.Pausing ? -Time.fixedUnscaledDeltaTime : Time.fixedUnscaledDeltaTime;
            if (paused == PauseState.Pausing && timeScale < 0)
            {
                Time.fixedDeltaTime = 0;
                Time.timeScale = 0;
                paused = PauseState.Pause;
                yield break;
            }

            if (paused == PauseState.UnPausing && timeScale > 1)
            {
                Time.fixedDeltaTime = deltaTime;
                Time.timeScale = 1;
                paused = PauseState.Play;
                yield break;
            }

            Time.fixedDeltaTime = deltaTime * timeScale;
            Time.timeScale = timeScale;
            yield return null;
        }
    }
}