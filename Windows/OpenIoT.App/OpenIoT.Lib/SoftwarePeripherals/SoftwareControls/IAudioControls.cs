using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.SoftwarePeripherals.SoftwareControls
{
    public interface IAudioControls
    {
        void ChangeVolume(int change);
        void ChangeMute();
    }
}
