namespace PlanesRemake.Runtime.Utils
{
    using System;
    
    public class Timer
    {
        //We are using a delegate here to clarify the parameters that have to be passed in when the event below 
        //this declaration is called.
        public delegate void TimerTick(float deltaTime, float timeTranscurred);
        public event TimerTick OnTimerTick;
        public event Action OnTimerCompleted;

        private float duration = 0;
        private float timeTranscurred = 0;
        private bool isRepeating = false;

        public bool IsRunning { get; private set; } = false;

        public Timer(float sourceDuration, bool sourceIsRepeating = false)
        {
            duration = sourceDuration;
            isRepeating = sourceIsRepeating;
        }

        /// <summary>
        /// You can use this to unpause a timer or restart a timer that has been stopped.
        /// </summary>
        public void Start()
        {
            TimerManager.Instance.AddTimer(this);
            IsRunning = true;
        }

        public void Pause()
        {
            TimerManager.Instance.RemoveTimer(this);
            IsRunning = false;
        }

        /// <summary>
        /// If the timer is stop suddenly, the OnTimerCompleted action will not be called.
        /// </summary>
        public void Stop()
        {
            TimerManager.Instance.RemoveTimer(this);
            timeTranscurred = 0;
            IsRunning = false;
        }

        public void Tick(float deltaTime)
        {
            timeTranscurred += deltaTime;

            if (timeTranscurred > duration)
            {
                OnTimerCompleted?.Invoke();

                if (isRepeating)
                {
                    timeTranscurred = 0;
                }
                else
                {
                    Stop();
                }
            }

            OnTimerTick?.Invoke(deltaTime, timeTranscurred);
        }
    }
}