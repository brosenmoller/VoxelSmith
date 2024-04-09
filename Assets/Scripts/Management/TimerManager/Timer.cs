using System;

namespace VoxelSmith
{
    public class Timer
    {
        private enum TimerState
        {
            Running,
            Paused,
            Finished,
        }

        private float currentTime = 0f;
        private float endTime;
        public event Action OnTimerEnd;
        private bool loop;

        private TimerState _state = TimerState.Paused;

        private TimerState State
        {
            get { return _state; }
            set
            {
                if (_state != TimerState.Running && value == TimerState.Running)
                {
                    GameManager.TimerManager.OnTimerUpdate += UpdateTimer;
                }
                else if (_state == TimerState.Running && value != TimerState.Running)
                {
                    GameManager.TimerManager.OnTimerUpdate -= UpdateTimer;
                }

                _state = value;
            }
        }
        public float EndTime
        {
            get { return endTime; }
            set
            {
                if (currentTime > value) { State = TimerState.Finished; }
                else if (IsFinished && currentTime < value) { State = TimerState.Paused; }

                endTime = value;
            }
        }
        public float TimeLeft { get { return endTime - currentTime; } }
        public bool IsRunning { get { return State == TimerState.Running; } }
        public bool IsFinished { get { return State == TimerState.Finished; } }

        public Timer(float endTime, Action callback = null, bool autoStart = false, bool loop = false)
        {
            this.loop = loop;
            this.endTime = endTime;

            if (autoStart) { State = TimerState.Running; }
            else { State = TimerState.Paused; }

            if (callback != null) { OnTimerEnd += callback; }
        }

        public void SetIsLooping(bool loop)
        {
            this.loop = loop;
            if (loop && IsFinished) { Restart(); }
        }

        public void Restart()
        {
            Reset();
            Start();
        }

        public void Start()
        {
            State = TimerState.Running;
        }

        public void UpdateTimer(float delta)
        {
            currentTime += delta;
            if (currentTime >= endTime)
            {
                try { OnTimerEnd?.Invoke(); }
                catch (Exception) { }

                if (loop) { Reset(); }
                else { State = TimerState.Finished; }
            }
        }

        public void Reset()
        {
            currentTime = 0f;
        }

        public void Pause()
        {
            State = TimerState.Paused;
        }
    }
}
