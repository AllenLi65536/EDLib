using System;
using System.Threading;

namespace EDLib
{
    /// <summary>
    /// Make a thread to sleep to the target time, and execute the designated function.
    /// </summary>
    /// <example>
    /// <code>
    /// static void main() {
    ///     ...
    ///     SleepToTarget st = new SleepToTarget(new DateTime(2037, 5, 10, 09, 00, 00), myAction);
    ///     st.Start();
    ///     ...
    /// }
    /// 
    /// //This function will be called on 2037/5/10 09:00:00. (As long as the program is still alive)
    /// static void myAction() {
    ///     Console.WriteLine("myAction");
    /// }
    /// </code>
    /// </example>
    public class SleepToTarget
    {
        private DateTime targetTime;
        private Action myAction;
        private readonly int minSleepMilliseconds = 250;

        /// <summary>
        /// Make a new thread to sleep to the targetTime and call myAction
        /// </summary>
        /// <param name="targetTime">Time to wakeup</param>
        /// <param name="myAction">Function to be executed on wakeup</param>
        /// <exception cref="ArgumentException">targetTime not initialized</exception>
        /// <exception cref="ArgumentNullException">myAction not specified</exception>
        public SleepToTarget(DateTime targetTime, Action myAction) {
            if (targetTime.Ticks == 0)
                throw new ArgumentException("targetTime not initialized", "targetTime");
            if (myAction == null)
                throw new ArgumentNullException("myAction", "myAction not specified");
            this.targetTime = targetTime;
            this.myAction = myAction;

            //new Thread(ProcessTimer).Start();
        }

        /// <summary>
        /// Remember to call this to start the thread at some time after construction.
        /// </summary>
        public void Start() {
            new Thread(ProcessTimer).Start();
        }

        private void ProcessTimer() {
            DateTime Now = DateTime.Now;

            while (Now < targetTime) {
                int SleepMilliseconds = (int) Math.Round((targetTime - Now).TotalMilliseconds / 2);
                //Console.WriteLine(SleepMilliseconds);
                Thread.Sleep(SleepMilliseconds > minSleepMilliseconds ? SleepMilliseconds : minSleepMilliseconds);
                Now = DateTime.Now;
            }

            myAction();
        }
    }
}
