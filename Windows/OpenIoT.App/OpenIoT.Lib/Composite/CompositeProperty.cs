using OpenIoT.Lib.Board.Models;
using OpenIoT.Lib.Web.Models.Configurations.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.Composite
{
    public class CompositeProperty
    {
        public BoardProperty BoardProperty { get; set; }
        public PeripheralProperty PeripheralProperty { get; set; }
        public Peripheral ParentPeripheral { get; set; }
        public CompositeBoard Board { get; set; }
    }
}
