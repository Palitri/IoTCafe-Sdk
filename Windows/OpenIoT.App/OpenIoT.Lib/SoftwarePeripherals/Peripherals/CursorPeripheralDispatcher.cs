using OpenIoT.Lib.SoftwarePeripherals.SoftwareControls;
using OpenIoT.Lib.Web.Models.Configurations.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.SoftwarePeripherals.Peripherals
{
    internal class CursorPeripheralDispatcher : ISoftwarePeripheralDispatcher
    {
        public string Name => "Cursor";

        public ISoftwarePeripheral Create(SoftwareControlsDispatcher softwareControlsDispatcher, Peripheral boardPeripheral)
        {
            return new CursorPeripheral(softwareControlsDispatcher, boardPeripheral);
        }
    }
}
