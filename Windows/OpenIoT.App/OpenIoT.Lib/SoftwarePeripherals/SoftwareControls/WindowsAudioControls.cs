using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.SoftwarePeripherals.SoftwareControls
{
    internal class WindowsAudioControls : IAudioControls
    {
        //[DllImport("winmm.dll")]
        //public static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);

        //[DllImport("winmm.dll")]
        //public static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);

        private const int WM_APPCOMMAND = 0x319;

        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int APPCOMMAND_VOLUME_UP = 0xA0000;
        private const int APPCOMMAND_VOLUME_DOWN = 0x90000;

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        private IntPtr handle;

        public WindowsAudioControls(IntPtr handle)
        {
            this.handle = handle;
        }

        public void ChangeVolume(int change)
        {
            while (change > 0)
            {
                SendMessageW(handle, WM_APPCOMMAND, handle, (IntPtr)APPCOMMAND_VOLUME_UP);
                change--;
            }

            while (change < 0)
            {
                SendMessageW(handle, WM_APPCOMMAND, handle, (IntPtr)APPCOMMAND_VOLUME_DOWN);
                change++;
            }
        }

        public void ChangeMute()
        {
            SendMessageW(handle, WM_APPCOMMAND, handle, (IntPtr)APPCOMMAND_VOLUME_MUTE);
        }
    }
}
