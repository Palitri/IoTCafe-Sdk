using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.SoftwarePeripherals.SoftwareControls
{
    public class WindowsCursorControls : ICursorControls
    {
        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const uint MOUSEEVENTF_RIGHTUP = 0x0010;
        private const uint MOUSEEVENTF_ABSOLUTE = 0x8000;
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, uint dwExtraInfo);
        private readonly uint[] mouseEventButtons = { MOUSEEVENTF_LEFTUP, MOUSEEVENTF_LEFTDOWN, MOUSEEVENTF_RIGHTUP, MOUSEEVENTF_RIGHTDOWN };



        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);



        [StructLayout(LayoutKind.Sequential)]
        struct POINT
        {
            internal int x;
            internal int y;
        }
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out POINT lpPoint);



        const int VK_LBUTTON = 0x01;
        const int VK_RBUTTON = 0x02;
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);
        private readonly int[] keyStateButtons = { VK_LBUTTON, VK_RBUTTON };




        private const int SM_CXSCREEN = 0;
        private const int SM_CYSCREEN = 1;
        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);


        private int screenWidth;
        private int screenHeight;

        public WindowsCursorControls()
        {
            this.screenWidth = GetSystemMetrics(SM_CXSCREEN);
            this.screenHeight = GetSystemMetrics(SM_CYSCREEN);
        }

        public bool GetButtonState(int button)
        {
            return (GetAsyncKeyState(keyStateButtons[button]) & 0x8000) != 0;
        }

        public void SetButtonState(int button, bool state)
        {
            mouse_event(mouseEventButtons[button * 2 + (state ? 1 : 0)], 0, 0, 0, 0);
        }

        public void GetCursor(out float x, out float y)
        {
            POINT pos;
            GetCursorPos(out pos);

            x = (float)pos.x / (float)this.screenWidth;
            y = (float)pos.y / (float)this.screenHeight;
        }

        public void SetCursor(float x, float y)
        {
            SetCursorPos((int)(x * (float)this.screenWidth), (int)(y * (float)this.screenHeight));
        }

        public void MoveCursor(float x, float y)
        {
            float posX, posY;
            this.GetCursor(out posX, out posY);

            SetCursor(posX + x, posY + y);
        }
    }
}
