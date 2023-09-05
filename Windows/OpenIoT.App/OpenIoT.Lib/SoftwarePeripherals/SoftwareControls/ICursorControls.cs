using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.SoftwarePeripherals.SoftwareControls
{
    public interface ICursorControls
    {
        bool GetButtonState(int button);
        void SetButtonState(int button, bool state);
        void GetCursor(out float x, out float y);
        void SetCursor(float x, float y);
        void MoveCursor(float x, float y);
    }
}
