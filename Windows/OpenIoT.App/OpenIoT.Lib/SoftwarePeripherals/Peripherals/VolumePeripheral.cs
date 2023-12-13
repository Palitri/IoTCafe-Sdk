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

        // TODO: Remove deltaProperty and use volumeProperty instead, make useDeltaProperty a setting and rename it simply to "delta"
        private BoardProperty volumeProperty, deltaProperty, useDeltaProperty;

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


            if (softwareControlsDispatcher.GetBoard().properties == null)
                return;

            if (boardPeripheral.Properties.Count < 3)
                return;

            // This should be in constructor, but at the time of construction, the OpenIoTBoard object has not yet populated its properties, because it gets them by a request to the physical board
            if (this.volumeProperty == null)
                this.volumeProperty = softwareControlsDispatcher.GetBoard().properties.FirstOrDefault(p => p.semantic == boardPeripheral.Properties[0].Semantic);
            if (this.deltaProperty == null)
                this.deltaProperty = softwareControlsDispatcher.GetBoard().properties.FirstOrDefault(p => p.semantic == boardPeripheral.Properties[1].Semantic);
            if (this.useDeltaProperty == null)
                this.useDeltaProperty = softwareControlsDispatcher.GetBoard().properties.FirstOrDefault(p => p.semantic == boardPeripheral.Properties[2].Semantic);


            if (this.useDeltaProperty.GetBool())
            {
                int delta = (int)Math.Round(this.deltaProperty.GetFloat());
                if (delta != 0)
                {
                    this.audioControls.ChangeVolume(delta);
                    this.deltaProperty.SetValue(0.0f);

                    softwareControlsDispatcher.GetBoard().RequestPropertyUpdate(this.deltaProperty);
                }
            }
            else
            { 
                this.audioControls.SetVolume(this.volumeProperty.GetFloat());
            }
        }
    }
}
