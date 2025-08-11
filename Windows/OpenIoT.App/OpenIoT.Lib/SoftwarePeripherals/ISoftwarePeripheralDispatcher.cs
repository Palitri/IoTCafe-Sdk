using Palitri.OpenIoT.SoftwarePeripherals.SoftwareControls;
using Palitri.OpenIoT.Web.Models.Configurations.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palitri.OpenIoT.SoftwarePeripherals
{
    public interface ISoftwarePeripheralDispatcher
    {
        string Name { get; }

        ISoftwarePeripheral Create(SoftwareControlsDispatcher softwareControlsDispatcher, Peripheral boardPeripheral);
    }
}
