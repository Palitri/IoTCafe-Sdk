using OpenIoT.Lib.Board.Api;
using OpenIoT.Lib.Board.Models;
using OpenIoT.Lib.SoftwarePeripherals.SoftwareControls;
using OpenIoT.Lib.Web.Models;
using OpenIoT.Lib.Web.Models.Configurations.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.SoftwarePeripherals.Peripherals
{
    internal class VolumePeripheral : ISoftwarePeripheral
    {
        private IAudioControls audioControls;

        private BoardProperty volumeProperty;

        public VolumePeripheral(SoftwareControlsDispatcher softwareControlsDispatcher, Peripheral boardPeripheral)
        {
            this.audioControls = softwareControlsDispatcher.CreateAudioControls();

            this.volumeProperty = softwareControlsDispatcher.GetBoard().properties.FirstOrDefault(p => p.semantic == boardPeripheral.Properties[0].Semantic);
        }

        public void Update()
        {
            if (this.volumeProperty != null)
            {
                this.audioControls.ChangeVolume((int)(this.volumeProperty.GetFloat() * 100) - 50);
            }
        }
    }
}
