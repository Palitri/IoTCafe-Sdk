using OpenIoT.Lib.SoftwarePeripherals.SoftwareControls;
using OpenIoT.Lib.Web.Models.Configurations.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.SoftwarePeripherals
{
    public interface ISoftwarePeripheralDispatcher
    {
        string Name { get; }

        ISoftwarePeripheral Create(SoftwareControlsDispatcher softwareControlsDispatcher, Peripheral boardPeripheral);
    }
}
