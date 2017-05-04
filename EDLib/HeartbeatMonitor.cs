using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace EDLib
{
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

        public HeartbeatMonitor(int timeoutSecs, Action noHeartbeatCallback, bool hideConsole = false) {
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
            while (true) {
                Thread.Sleep(timeoutSecs * 1000);
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

        public void Heartbeat() {
            updatedTime = DateTime.Now;
        }
    }
}

