using Palitri.OpenIoT.SoftwarePeripherals.SoftwareControls;
using Palitri.OpenIoT.Web.Models.Configurations.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palitri.OpenIoT.SoftwarePeripherals.Peripherals
{
    internal class VolumePeripheralDispatcher : ISoftwarePeripheralDispatcher
    {
        public string Name => "Volume";

        public ISoftwarePeripheral Create(SoftwareControlsDispatcher softwareControlsDispatcher, Peripheral boardPeripheral)
        {
            return new VolumePeripheral(softwareControlsDispatcher, boardPeripheral);
        }
    }
}
