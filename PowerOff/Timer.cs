using System;

namespace PowerOff
{
    [Serializable]
    public class Timer
    {
        private TimeSpan times;

        public Timer()
        {
            times = new TimeSpan(0,0,0);
        }

        public Timer(TimeSpan time)
        {
            times = time;
        }

        public void SetTimes(TimeSpan times)
        {
            this.times = times;
        }

        public TimeSpan GetTimes()
        {
            return times;
        }
    }
}
