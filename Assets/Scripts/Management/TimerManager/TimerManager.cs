using System;

public class TimerManager : Manager
{
    public event Action<float> OnTimerUpdate;
    public override void OnUpdate(double delta)
    {
        OnTimerUpdate?.Invoke((float)delta);
    }
}
