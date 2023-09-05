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
    internal class CursorPeripheral : ISoftwarePeripheral
    {
        private BoardProperty output;
        private BoardProperty absolute;
        private BoardProperty x;
        private BoardProperty y;
        private BoardProperty push;
        private BoardProperty altPush;

        private ICursorControls cursorControls;

        private SoftwareControlsDispatcher softwareControlsDispatcher;
        private Peripheral boardPeripheral;

        private float oldX, oldY;
        private bool oldPush, oldAltPush;

        public CursorPeripheral(SoftwareControlsDispatcher softwareControlsDispatcher, Peripheral boardPeripheral)
        {
            this.softwareControlsDispatcher = softwareControlsDispatcher;
            this.boardPeripheral = boardPeripheral;

            this.cursorControls = softwareControlsDispatcher.CreateCursorControls();
        }

        public void Update()
        {
            if (this.output != null)
            {
                if (this.output.GetBool())
                {
                    float x, y;
                    this.cursorControls.GetCursor(out x, out y);
                    this.x.SetValue(x);
                    this.y.SetValue(y);

                    bool pushed = this.cursorControls.GetButtonState(0);
                    this.push.SetValue(pushed);

                    pushed = this.cursorControls.GetButtonState(1);
                    this.altPush.SetValue(pushed);
                }
                else
                {
                    if ((this.oldX != this.x.GetFloat()) || (this.oldY != this.y.GetFloat()))
                    {
                        this.cursorControls.SetCursor(this.x.GetFloat(), this.y.GetFloat());
                        this.oldX = this.x.GetFloat();
                        this.oldY = this.y.GetFloat();
                    }

                    if (this.oldPush != this.push.GetBool())
                    {
                        this.cursorControls.SetButtonState(0, this.push.GetBool());
                        this.oldPush = this.push.GetBool();
                    }

                    if (this.oldAltPush != this.altPush.GetBool())
                    {
                        this.cursorControls.SetButtonState(1, this.altPush.GetBool());
                        this.oldAltPush = this.altPush.GetBool();
                    }
                }
            }
            else
            {
                // This should be in constructor, but at the time of construction, the OpenIoTBoard object has not yet populated its properties, because it gets them by a request to the physical board
                if (softwareControlsDispatcher.GetBoard().properties != null)
                {
                    this.output = softwareControlsDispatcher.GetBoard().properties.FirstOrDefault(p => p.semantic == boardPeripheral.Properties[0].Semantic);
                    this.absolute = softwareControlsDispatcher.GetBoard().properties.FirstOrDefault(p => p.semantic == boardPeripheral.Properties[1].Semantic);
                    this.x = softwareControlsDispatcher.GetBoard().properties.FirstOrDefault(p => p.semantic == boardPeripheral.Properties[2].Semantic);
                    this.y = softwareControlsDispatcher.GetBoard().properties.FirstOrDefault(p => p.semantic == boardPeripheral.Properties[3].Semantic);
                    this.push = softwareControlsDispatcher.GetBoard().properties.FirstOrDefault(p => p.semantic == boardPeripheral.Properties[4].Semantic);
                    this.altPush = softwareControlsDispatcher.GetBoard().properties.FirstOrDefault(p => p.semantic == boardPeripheral.Properties[5].Semantic);
                }
            }

        }
    }
}
