
    public abstract class OnTimer:LevelEvent
    {
        protected abstract float timeToPass { get; }
        public override void Activate()
        {
            base.Activate();
            connections.Add(_levelManager.OnLevelTick.Subscribe(t =>
            {
                if (t < timeToPass) return;
                OnEvent.Invoke();
                Deactivate();
            }));
        }
    }
