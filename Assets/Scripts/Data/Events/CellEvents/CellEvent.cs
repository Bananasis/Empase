namespace Data.Events.LevelEvents
{
    public abstract class CellEvent:LevelEvent
    {
        public virtual void Activate(Cell cell)
        {
            Activate();
            connections.Add(cell.OnDeath.SubscribeOnce(Deactivate));
        }

        protected override IAnnouncement _announcement => null;
    }
}