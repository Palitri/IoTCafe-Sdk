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
    internal class CommandPeripheral : ISoftwarePeripheral
    {
        private BoardProperty active;
        private BoardProperty command;

        private ICommandControl commandControl;

        private SoftwareControlsDispatcher softwareControlsDispatcher;
        private Peripheral boardPeripheral;

        private bool oldActive;

        public CommandPeripheral(SoftwareControlsDispatcher softwareControlsDispatcher, Peripheral boardPeripheral)
        {
            this.softwareControlsDispatcher = softwareControlsDispatcher;
            this.boardPeripheral = boardPeripheral;

            this.commandControl = softwareControlsDispatcher.CreateCommandControl();
        }

        public void Update()
        {
            if (this.active != null)
            {
                if (this.oldActive != this.active.GetBool())
                {
                    this.commandControl.Run(this.command.GetString());
                    this.oldActive = this.active.GetBool();
                }
            }
            else
            {
                // This should be in constructor, but at the time of construction, the OpenIoTBoard object has not yet populated its properties, because it gets them by a request to the physical board
                if (softwareControlsDispatcher.GetBoard().properties != null)
                {
                    this.active = softwareControlsDispatcher.GetBoard().properties.FirstOrDefault(p => p.semantic == boardPeripheral.Properties[0].Semantic);
                    this.command = softwareControlsDispatcher.GetBoard().properties.FirstOrDefault(p => p.semantic == boardPeripheral.Properties[1].Semantic);
                }
            }

        }
    }
}
