using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Insomnia
{
    class Program
    {
        #region System Imports

        #region SetThreadExecutionState

        [Flags]
        public enum EXECUTION_STATE : uint
        {
            ES_SYSTEM_REQUIRED = 0x00000001,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_CONTINUOUS = 0x80000000
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE flags);

        #endregion

        #region Set window in interface

        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        #endregion

        #region Get timeouts from system

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "SystemParametersInfo")]
        internal static extern int SystemParametersInfo(int uiAction, int uiParam, ref int pvParam, int fWinIni);

        #endregion

        #endregion

        static void Main(string[] args)
        {
            // Get currently running Insomnia processes.
            // Kill all of them if they exist including this one.
            // Otherwise the program continues.
            KillInsomniaProcessesWhenMoreThanOne();
            // Build the interval object that contains rules for this specific instance of the program.
            Interval interval = BuildIntervalFromArgs(args);
            // Get timeout values from system and set the timeout for this programs 
            // poking of the device to the lowest value found.
            interval.SetIntervalInMilliseconds(GetLowestTimeoutValueFromSystem());
            // Disable the system from going to sleep and locking by manipulating thread execution state 
            // and continously poking the system using simulated keypresses and/or wiggling the mouse back and forth.
            KeepSystemAlive(interval);
        }

        private static void KillInsomniaProcessesWhenMoreThanOne()
        {
            // Get current process
            Process currentProcess = Process.GetCurrentProcess();
            // Get all other processes with the same name.
            List<Process> runningInsomniaProcesses = new List<Process>();
            runningInsomniaProcesses.AddRange(Process.GetProcessesByName(currentProcess.ProcessName));
            // Parse instance started by visual studio for debugging.
            if (currentProcess.ProcessName.EndsWith(".vshost"))
                runningInsomniaProcesses.AddRange(Process.GetProcessesByName(Path.GetFileNameWithoutExtension(currentProcess.ProcessName)));
            // Kill all processes when more than one are running.
            if (runningInsomniaProcesses != null && runningInsomniaProcesses.Count > 1)
            {
                foreach (Process runningInsomniaProcess in runningInsomniaProcesses)
                    if (runningInsomniaProcess.Id != currentProcess.Id)
                        runningInsomniaProcess.Kill();
                currentProcess.Kill();
            }
        }

        private static Interval BuildIntervalFromArgs(string[] args)
        {
            Interval interval = new Interval();

            if (args != null && args.Length >= 1)
            {
                // Get Arguments and set properties on interval object.
                for (int i = 0; i < args.Length; i++)
                {
                    string argumentKey = (args[i].StartsWith("-")) ? args[i].Remove(0, 1).ToLower() : string.Empty;
                    if (string.IsNullOrEmpty(argumentKey) || 
                        argumentKey.Length != 1 || 
                        args.Length == i)
                        continue;
                    else
                    {
                        string argumentValue = (args.Length >= (i + 1)) ? args[i + 1] : string.Empty;
                        if (!string.IsNullOrEmpty(argumentValue))
                            switch (argumentKey)
                            {
                                case "s": // Start date
                                    interval.SetDateTime(argumentValue, true);
                                    break;
                                case "e": // End date
                                    interval.SetDateTime(argumentValue, false);
                                    break;
                                case "d": // Days of week
                                    interval.SetDaysOfWeek(argumentValue);
                                    break;
                                case "i": // Interval
                                    interval.SetIntervalInMilliseconds(argumentValue);
                                    break;
                                case "p": // Poll for stop poking
                                    interval.SetPollForStopPoking(argumentValue);
                                    break;
                                default:
                                    break;
                            }
                    }
                }
            }

            return interval;
        }

        #region GetLowestTimeoutValueFromSystem

        public static int batteryIdleTimer;
        public static int externalIdleTimer;
        public static int wakeupIdleTimer;
        public static int GetLowestTimeoutValueFromSystem()
        {
            return 
                Lowest(
                    new int[]
                    {
                        (SystemParametersInfo(252, 0, ref batteryIdleTimer, 0) == 1) ? batteryIdleTimer : -1,
                        (SystemParametersInfo(254, 0, ref externalIdleTimer, 0) == 1) ? externalIdleTimer : -1,
                        (SystemParametersInfo(256, 0, ref wakeupIdleTimer, 0) == 1) ? wakeupIdleTimer : -1
                    }
                );
        }

        private static int Lowest(params int[] inputs)
        {
            int lowest = inputs[0];
            foreach (var input in inputs)
                if (input < lowest) lowest = input;
            return lowest;
        }

        #endregion

        #region KeepSystemAlive

        private static void KeepSystemAlive(Interval interval)
        {
            // Prevent system from going turning off display or going to sleep
            SetThreadExecutionState(
                EXECUTION_STATE.ES_DISPLAY_REQUIRED | 
                EXECUTION_STATE.ES_SYSTEM_REQUIRED | 
                EXECUTION_STATE.ES_CONTINUOUS
            );

            // Initiate a timer that pokes the device at a given interval.
            System.Threading.Timer pokeDeviceTimer = 
                new System.Threading.Timer(
                    new TimerCallback(PokeDevice),
                    null,
                    0,
                    interval.IntervalInMilliseconds
                );

            // Check if the timer should stop at a given timespan.
            for (;ShouldKeepAlive(interval);)
                Thread.Sleep(interval.PollForStopPoking);

            // Release resources.
            pokeDeviceTimer.Dispose();
        }
        
        private static bool ShouldKeepAlive(Interval interval)
        {
            return
                interval.StartDate <= DateTime.Now && 
                interval.EndDate > DateTime.Now &&
                interval.DaysOfWeek[(int)(DateTime.Now.DayOfWeek)]
                ;
        }

        private static void PokeDevice(object state)
        {
            GrabWindowAndSendKeypress("SysListView32", "FolderView", "{F15}");
            //WiggleMouseAPixelBackAndForth();
        }

        private static void GrabWindowAndSendKeypress(string lpClassName, string lpWindowName, string keys)
        {
            try
            {
                IntPtr Handle = FindWindow(lpClassName, lpWindowName);

                if (Handle == IntPtr.Zero)
                {
                    SetForegroundWindow(Handle);
                    SendKeys.SendWait(keys);
                }
            }
            catch
            {

            }
        }

        private static void WiggleMouseAPixelBackAndForth()
        {
            try
            {
                Cursor.Position = new Point(Cursor.Position.X - 1, Cursor.Position.Y);
                Cursor.Position = new Point(Cursor.Position.X + 1, Cursor.Position.Y);
            }
            catch
            {
                try
                {
                    Cursor.Position = new Point(Cursor.Position.X + 1, Cursor.Position.Y);
                    Cursor.Position = new Point(Cursor.Position.X - 1, Cursor.Position.Y);
                }
                catch
                {

                }
            }
        }

        #endregion
    }
}