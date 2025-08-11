using Palitri.OpenIoT.Composite;
using Palitri.OpenIoT.SoftwarePeripherals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palitri.OpenIoT.App.Forms
{
    internal class AppBase
    {
        private static AppBase instance = new AppBase();
        public static AppBase Instance { get { return instance; } }

        public CompositeBoard Board { get; private set; }

        public AppBase()
        {
            this.Board = new CompositeBoard();
        }
    }
}
