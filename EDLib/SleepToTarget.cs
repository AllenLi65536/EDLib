using System;
using System.Threading;

namespace EDLib
{
    /// <summary>
    /// Make a thread to sleep to the target time, and execute the designated function.
    /// </summary>
    public class SleepToTarget
    {
        private DateTime targetTime;
        private Action myAction;
        private readonly int minSleepMilliseconds = 250;

        /// <summary>
        /// Sleep to the TargetTime and call MyAction
        /// </summary>
        /// <param name="targetTime">Time to wakeup</param>
        /// <param name="myAction">Function to be executed on wakeup</param>
        public SleepToTarget(DateTime targetTime, Action myAction) {
            this.targetTime = targetTime;
            this.myAction = myAction;
        }

        /// <summary>
        /// Start the thread
        /// </summary>
        public void Start() {
            new Thread(new ThreadStart(ProcessTimer)).Start();
        }

        private void ProcessTimer() {
            DateTime Now = DateTime.Now;

            while (Now < targetTime) {
                int SleepMilliseconds = (int) Math.Round((targetTime - Now).TotalMilliseconds / 2);
                Console.WriteLine(SleepMilliseconds);
                Thread.Sleep(SleepMilliseconds > minSleepMilliseconds ? SleepMilliseconds : minSleepMilliseconds);
                Now = DateTime.Now;
            }

            myAction();
        }
    }
}
