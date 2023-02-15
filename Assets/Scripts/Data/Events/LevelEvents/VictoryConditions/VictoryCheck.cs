using Data;

public class VictoryCheck : VictoryCondition
{
    public override void Activate()
    {
        base.Activate();
        connections.Add(_levelManager.OnLevelTick.Subscribe((t) =>
        {
            if (!CheckForVictory(t)) return;
            OnEvent.Invoke();
            Deactivate();
        }));
    }
    protected virtual bool CheckForVictory(float time)
    {
        return true;
    }
}