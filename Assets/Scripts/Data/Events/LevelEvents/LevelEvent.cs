using System;
using Zenject;

public abstract class LevelEvent : GameEvent
{
    [Inject] protected ILevelManager _levelManager;
    [Inject] protected IGlobalCellStats _gCellData;
    public virtual bool persistOnReload => false;

    public override void Activate()
    {
        base.Activate();
        connections.Add(_levelManager.OnLevelStateChange.Subscribe(OnStateChange));
    }

    protected virtual void OnStateChange(LevelState state)
    {
        switch (state)
        {
            case LevelState.NotLoaded:
                Deactivate();
                break;
            case LevelState.Restart:
                if(!persistOnReload) 
                    Deactivate();
                break;
        }
    }


}