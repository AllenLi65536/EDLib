using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace EDLib
{
    /// <summary>
    /// Miscellaneous classes
    /// </summary>
    internal class NamespaceDoc { }

    /// <summary>
    /// Monitors heartbeat
    /// </summary>
    /// <example>
    /// <code>
    /// static HeartbeatMonitor hm = new HeartbeatMonitor(5, myAction);
    /// 
    /// static void myAction() {
    ///     //Do sth on timeout     
    ///     Console.WriteLine("myAction");
    ///     ...
    /// }
    /// 
    /// //This function shall be called on message receive with interval less than 5 seconds
    /// static void OnMessageReceived(object listener, MessageReceivedEventArgs messageReceivedEventArgs) {
    ///     Message message = messageReceivedEventArgs.Message;
    ///     hm.Heartbeat();
    ///     //Do sth
    ///     ...
    /// }
    /// </code>
    /// </example>
    public class HeartbeatMonitor
    {
        //public delegate void noHeartbeatCallback();

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        private readonly IntPtr handle;
        private DateTime updatedTime;
        private bool consoleShow;
        private readonly bool hideConsole;
        private readonly int timeoutSecs;
        private Action callBack;

        /// <summary>
        /// Check the newest heartbeat time (updatedTime) every timeoutSecs.
        /// If the newest heartbeat time is longer than timeoutSecs, noHeartbeatCallback will be called. 
        /// </summary>
        /// <param name="timeoutSecs">Timeout seconds</param>
        /// <param name="noHeartbeatCallback">Function to be called when timeout</param>
        /// <param name="hideConsole">Shall hide the console until timeout. Rehide it when not timeout.</param>
        /// <exception cref="ArgumentOutOfRangeException">timeoutSecs should > 0</exception>
        /// <exception cref="ArgumentNullException">noHeartbeatCallback not specified</exception>
        public HeartbeatMonitor(int timeoutSecs, Action noHeartbeatCallback, bool hideConsole = false) {
            if (timeoutSecs <= 0)
                throw new ArgumentOutOfRangeException("timeoutSecs", "timeoutSecs should > 0");
            if (noHeartbeatCallback == null)
                throw new ArgumentNullException("noHeartbeatCallback", "noHeartbeatCallback not specified");

            handle = GetConsoleWindow();
            updatedTime = DateTime.Now;
            consoleShow = true;
            this.hideConsole = hideConsole;
            this.timeoutSecs = timeoutSecs;
            callBack = noHeartbeatCallback;

            if (hideConsole) {
                ShowWindow(handle, SW_HIDE);
                consoleShow = false;
            }

            Thread workThread = new Thread(CheckHeartbeat);
            workThread.Start();
        }

        //Check data every timeoutSecs
        private void CheckHeartbeat() {
            int timeoutMillisecs = timeoutSecs * 1000;
            while (true) {
                Thread.Sleep(timeoutMillisecs);
                if ((DateTime.Now - updatedTime).TotalSeconds > timeoutSecs) {
                    if (!consoleShow) {
                        ShowWindow(handle, SW_SHOW);
                        consoleShow = true;
                    }
                    callBack();
                } else if (consoleShow && hideConsole) {
                    ShowWindow(handle, SW_HIDE);
                    consoleShow = false;
                }
            }
        }

        /// <summary>
        /// Send heartbeat signal to HeartbeatMonitor
        /// </summary>
        public void Heartbeat() {
            updatedTime = DateTime.Now;
        }
    }
}

