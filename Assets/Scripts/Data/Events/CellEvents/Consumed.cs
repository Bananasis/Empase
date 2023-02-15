
    using Data.Events.LevelEvents;
    using UnityEngine;
    using Zenject;
    [CreateAssetMenu(fileName = "Consumed", menuName = "ScriptableObjects/GenericEvents/Consumed",
        order = 1)]
    public class Consumed:CellEvent
    {
       [SerializeField] protected CellType type;
       [SerializeField] protected bool any;
        public override void Activate(Cell cell)
        {
            connections.Add(cell.OnConsumed.Subscribe((c) =>
            {
                if (c.cData.type != type && ! any) return;
                OnEvent.Invoke();
                Deactivate();
            }));
            base.Activate();
        }
    }
