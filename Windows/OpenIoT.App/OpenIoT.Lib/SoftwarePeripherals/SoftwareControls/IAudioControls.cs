using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palitri.OpenIoT.SoftwarePeripherals.SoftwareControls
{
    public interface IAudioControls
    {
        void SetVolume(float volume);
        void ChangeVolume(int change);
        void ChangeMute();
    }
}
