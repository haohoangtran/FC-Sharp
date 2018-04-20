using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HDH
{
        public class GetProcesses : IWin32Window
        {
            internal IntPtr handle;
            internal String title;

            public IntPtr Handle
            {
                get { return handle; }
            }

            public String Title
            {
                get { return title; }
            }

            public static readonly Int32 GWL_STYLE = -16;
            public static readonly UInt64 WS_VISIBLE = 0x10000000L;
            public static readonly UInt64 WS_BORDER = 0x00800000L;
            public static readonly UInt64 DESIRED_WS = WS_BORDER | WS_VISIBLE;

            public delegate Boolean EnumWindowsCallback(IntPtr hwnd, Int32 lParam);

            public static List<GetProcesses> GetAllWindows()
            {
                List<GetProcesses> windows = new List<GetProcesses>();
                StringBuilder buffer = new StringBuilder(100);
                EnumWindows(delegate (IntPtr hwnd, Int32 lParam)
                {
                    if ((GetWindowLongA(hwnd, GWL_STYLE) & DESIRED_WS) == DESIRED_WS)
                    {
                        GetWindowText(hwnd, buffer, buffer.Capacity);
                        GetProcesses wnd = new GetProcesses();
                        wnd.handle = hwnd;
                        wnd.title = buffer.ToString();
                        windows.Add(wnd);
                    }
                    return true;
                }, 0);

                return windows;
            }

            [DllImport("user32.dll", EntryPoint = "GetClassLong")]
            static extern uint GetClassLong32(IntPtr hWnd, int nIndex);

            [DllImport("user32.dll", EntryPoint = "GetClassLongPtr")]
            static extern IntPtr GetClassLong64(IntPtr hWnd, int nIndex);

            [DllImport("user32.dll")]
            static extern Int32 EnumWindows(EnumWindowsCallback lpEnumFunc, Int32 lParam);

            [DllImport("user32.dll")]
            public static extern void GetWindowText(IntPtr hWnd, StringBuilder lpString, Int32 nMaxCount);

            [DllImport("user32.dll")]
            static extern UInt64 GetWindowLongA(IntPtr hWnd, Int32 nIndex);

            [DllImport("user32.dll")]
            public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);
        }
}
