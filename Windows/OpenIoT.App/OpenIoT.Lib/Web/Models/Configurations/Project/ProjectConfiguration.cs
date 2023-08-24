using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.Web.Models.Configurations.Project
{
    public class ProjectConfiguration
    {
        public List<Peripheral> Peripherals { get; set; }

        private List<PeripheralProperty> _properties = null;
        public List<PeripheralProperty> Properties 
        { 
            get
            {
                if (this._properties == null)
                    this._properties = this.Peripherals.SelectMany(p => p.Properties).ToList();

                return this._properties;
            }
        }

        public ProjectConfiguration()
        {
            this.Peripherals = new List<Peripheral>();
            this._properties = null;
        }

        public void Clear()
        {
            this.Peripherals.Clear();
            this._properties = null;
        }

        public Peripheral GetPropertyPeripheral(int semantic)
        {
            foreach (Peripheral peripheral in this.Peripherals)
                foreach (PeripheralProperty property in peripheral.Properties)
                    if (property.Semantic == semantic)
                        return peripheral;

            return null;
        }
    }
}
