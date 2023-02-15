using Data.Events.LevelEvents;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Consume", menuName = "ScriptableObjects/GenericEvents/Consume",
    order = 1)]
public class Consume : CellEvent

{
    [SerializeField] protected CellType type;
    [SerializeField] protected bool any;


    public override void Activate(Cell cell)
    {
        connections.Add(cell.OnConsume.Subscribe((c) =>
        {
            if (c.cData.type != type && !any) return;
            OnEvent.Invoke();
            Deactivate();
        }));
        base.Activate();
    }
}