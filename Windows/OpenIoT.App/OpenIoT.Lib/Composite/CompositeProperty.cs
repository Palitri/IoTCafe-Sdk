using Palitri.OpenIoT.Board.Models;
using Palitri.OpenIoT.Web.Models.Configurations.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palitri.OpenIoT.Composite
{
    public class CompositeProperty
    {
        public BoardProperty BoardProperty { get; set; }
        public PeripheralProperty PeripheralProperty { get; set; }
        public Peripheral ParentPeripheral { get; set; }
        public CompositeBoard Board { get; set; }
    }
}
