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

        private SoftwareControlsDispatcher softwareControlsDispatcher;
        private Peripheral boardPeripheral;

        public VolumePeripheral(SoftwareControlsDispatcher softwareControlsDispatcher, Peripheral boardPeripheral)
        {
            this.softwareControlsDispatcher = softwareControlsDispatcher;
            this.boardPeripheral = boardPeripheral;

            this.audioControls = softwareControlsDispatcher.CreateAudioControls();
        }

        public void Update()
        {
            //WinAPIVolume.Instance.SetVolume(this.volumeProperty.GetFloat());

            //return;

            if (this.volumeProperty != null)
            {
                //int newVolumeValue = (int)(this.volumeProperty.GetFloat() * 50.0f);
                //int delta = newVolumeValue - this.volumeValue;
                //if (delta != 0)
                //{
                //    this.audioControls.ChangeVolume(delta);

                //    this.volumeValue = newVolumeValue;
                //}


                this.audioControls.SetVolume(this.volumeProperty.GetFloat());
            }
            else
            {
                // This should be in constructor, but at the time of construction, the OpenIoTBoard object has not yet populated its properties, because it gets them by a request to the physical board
                if (softwareControlsDispatcher.GetBoard().properties != null)
                    this.volumeProperty = softwareControlsDispatcher.GetBoard().properties.FirstOrDefault(p => p.semantic == boardPeripheral.Properties[0].Semantic);
            }

        }
    }
}
