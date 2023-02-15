using UnityEngine;
using Zenject;
[CreateAssetMenu(fileName = "DeathDefeat", menuName = "ScriptableObjects/DefeatConditions/DeathDefeat", order = 1)]
public class DeathDefeat : DefeatCondition
{
    [Inject] private ICellPool _cellPool;

    public override void Activate()
    {
        base.Activate();
        connections.Add(_cellPool.Player.OnDeath.Subscribe(() =>
        {
            OnEvent.Invoke();
            Deactivate();
        }));
    }
}