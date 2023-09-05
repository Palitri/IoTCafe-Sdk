using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.SoftwarePeripherals.SoftwareControls
{
    internal class WindowsCommandControl : ICommandControl
    {
        public void Run(string command)
        {
            ProcessStartInfo ProcessInfo;

            ProcessInfo = new ProcessStartInfo(command);
            ProcessInfo.CreateNoWindow = true;
            ProcessInfo.UseShellExecute = true;

            Process process = Process.Start(ProcessInfo);
        }
    }
}
