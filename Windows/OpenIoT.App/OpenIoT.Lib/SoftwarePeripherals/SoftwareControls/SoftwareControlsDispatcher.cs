using OpenIoT.Lib.Board.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.SoftwarePeripherals.SoftwareControls
{
    public class SoftwareControlsDispatcher
    {
        IntPtr handle;
        OpenIoTBoard board;

        public SoftwareControlsDispatcher(IntPtr handle, OpenIoTBoard board)
        {
            this.handle = handle;
            this.board = board;
        }

        public OpenIoTBoard GetBoard()
        {
            return this.board;
        }

        public IAudioControls CreateAudioControls()
        {
            return new WindowsAudioControls(this.handle);
        }

        public ICursorControls CreateCursorControls()
        {
            return new WindowsCursorControls();
        }

        public ICommandControl CreateCommandControl()
        {
            return new WindowsCommandControl();
        }
    }
}
