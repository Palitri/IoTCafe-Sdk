using OpenIoT.Lib.SoftwarePeripherals.Peripherals;
using OpenIoT.Lib.SoftwarePeripherals.SoftwareControls;
using OpenIoT.Lib.Web.Models.Configurations.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenIoT.Lib.SoftwarePeripherals
{
    // Consider alternate architecture with SoftwareTerminals: a collection of bindable values
    public class SoftwarePeripheralsManager
    {
        private List<ISoftwarePeripheral> softwarePeripherals;

        private List<ISoftwarePeripheralDispatcher> softwarePeripheralDispatchers;

        private SoftwareControlsDispatcher softwareControls;
        
        public SoftwarePeripheralsManager(SoftwareControlsDispatcher softwareControls)
        {
            this.softwareControls = softwareControls;

            this.softwarePeripherals = new List<ISoftwarePeripheral>();
            this.softwarePeripheralDispatchers = new List<ISoftwarePeripheralDispatcher>();

            this.AddPeripheralDispatcher(new VolumePeripheralDispatcher());
            this.AddPeripheralDispatcher(new CursorPeripheralDispatcher());
            this.AddPeripheralDispatcher(new CommandPeripheralDispatcher());

        }

        public void AddPeripheralDispatcher(ISoftwarePeripheralDispatcher dispatcher)
        {
            this.softwarePeripheralDispatchers.Add(dispatcher); 
        }
        
        public void AddPeripheral(Peripheral peripheral)
        {
            ISoftwarePeripheralDispatcher dispatcher = this.softwarePeripheralDispatchers.FirstOrDefault(d => d.Name == peripheral.TypeId);

            if (dispatcher == null)
                return;

            ISoftwarePeripheral softwarePeripheral = dispatcher.Create(softwareControls, peripheral);

            this.softwarePeripherals.Add(softwarePeripheral);
        }

        public void Update()
        {
            foreach (ISoftwarePeripheral softwarePeripheral in this.softwarePeripherals)
                softwarePeripheral.Update();
        }

        public void UpdateOnChange(int propertyId)
        {
            foreach (ISoftwarePeripheral softwarePeripheral in this.softwarePeripherals)
                softwarePeripheral.Update();
        }
    }
}
