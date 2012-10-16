#region

using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;

#endregion

namespace BackInTime
{
    internal class Program
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool SetLocalTime(ref SYSTEMTIME lpSystemTime);

        public static void SetTime(ushort day, ushort month, ushort year, ushort hour, ushort minute)
        {
            var time = new SYSTEMTIME {wDay = day, wMonth = month, wYear = year, wHour = hour, wMinute = minute};
            if (!SetLocalTime(ref time))
            {
                // The native function call failed, so throw an exception
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        public static void SetTime(SYSTEMTIME time)
        {
            if (!SetLocalTime(ref time))
            {
                // The native function call failed, so throw an exception
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        public static void SetTime(DateTime timed)
        {
            var time = new SYSTEMTIME
                           {
                               wDay = (ushort) timed.Day,
                               wMonth = (ushort) timed.Month,
                               wYear = (ushort) timed.Year,
                               wHour = (ushort) timed.Hour,
                               wMinute = (ushort) timed.Minute
                           };
            if (!SetLocalTime(ref time))
            {
                // The native function call failed, so throw an exception
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        private static void Main(string[] args)
        {
            if (args.Length == 5)
            {
                var time = new SYSTEMTIME
                               {
                                   wDay = ushort.Parse(args[0]),
                                   wMonth = ushort.Parse(args[1]),
                                   wYear = ushort.Parse(args[2]),
                                   wHour = ushort.Parse(args[3]),
                                   wMinute = ushort.Parse(args[4])
                               };
                SetTime(time);
                return;
            }
            if (File.Exists("date.txt"))
            {


                var tr = new StreamReader("date.txt");
                string dateString = tr.ReadLine();
                if (dateString != null)
                {
                    dateString = dateString.Trim();
                    DateTime convertedDate = DateTime.Parse(dateString);
                    SetTime(convertedDate);
                }
            }
            else
            {
                SetTime(1, 1, 2012, 12, 00);
            }
        }

        #region Nested type: SYSTEMTIME

        [StructLayout(LayoutKind.Sequential)]
        internal struct SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek; // ignored for the SetLocalTime function
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;
        }

        #endregion
    }
}