using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Zenject;

public class Installer : MonoInstaller
{
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private MouseInput _inputProvider;
    [SerializeField] private CellManager _cellManager;
    [SerializeField] private CellPool _cellPool;
    [SerializeField] private WindowManager _windowManager;
    [SerializeField] private TimeDialitionManager _timeDialitionManager;
    [SerializeField] private GravityManager _gravityManager;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private PopupManager _popupManage;
    [SerializeField] private EventManager _eventManager;
    public override void InstallBindings()
    {
        Container.Bind<ILevelManager>().To<ILevelManager>().FromInstance(_levelManager).AsSingle();
        Container.Bind<ILevelLoader>().To<ILevelLoader>().FromInstance(_levelManager).AsSingle();
        Container.Bind<IInputProvider>().To<IInputProvider>().FromInstance(_inputProvider).AsSingle();
        Container.Bind<ICellManager>().To<ICellManager>().FromInstance(_cellManager).AsSingle();
        Container.Bind<IGlobalCellStats>().To<IGlobalCellStats>().FromInstance(_cellManager).AsSingle();
        Container.Bind<ICellPool>().To<ICellPool>().FromInstance(_cellPool).AsSingle();
        Container.Bind<IWindowManager>().To<IWindowManager>().FromInstance(_windowManager).AsSingle();
        Container.Bind<ITimeDialitionManager>().To<ITimeDialitionManager>().FromInstance(_timeDialitionManager)
            .AsSingle();
        Container.Bind<IGravityManager>().To<IGravityManager>().FromInstance(_gravityManager).AsSingle();

        Container.Bind<LevelManager>().To<LevelManager>().FromInstance(_levelManager).AsSingle();
        Container.Bind<CellManager>().To<CellManager>().FromInstance(_cellManager).AsSingle();
        Container.Bind<CellPool>().To<CellPool>().FromInstance(_cellPool).AsSingle();
        Container.Bind<TimeDialitionManager>().To<TimeDialitionManager>().FromInstance(_timeDialitionManager)
            .AsSingle();
        Container.Bind<GravityManager>().To<GravityManager>().FromInstance(_gravityManager).AsSingle();
        Container.Bind<GameManager>().To<GameManager>().FromInstance(_gameManager).AsSingle();
        Container.Bind<PopupManager>().To<PopupManager>().FromInstance(_popupManage).AsSingle();
        Container.Bind<EventManager>().To<EventManager>().FromInstance(_eventManager).AsSingle();
        var scriptables = Resources.LoadAll("Data", typeof(ScriptableObject));
   
        foreach (ScriptableObject scriptObject in scriptables)
        {
            Container.QueueForInject(scriptObject);  
        }
        
    }
}