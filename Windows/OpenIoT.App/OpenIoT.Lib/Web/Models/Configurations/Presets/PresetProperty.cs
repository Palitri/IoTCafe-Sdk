using OpenIoT.Lib.Web.Models.Configurations.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.Web.Models.Configurations.Presets
{
    public class PresetProperty
    {
        public string ScriptId { get; set; }
        public PeripheralPropertyType Type { get; set; }
        private object _value;
        public object Value 
        { 
            get { return this._value; }

            set
            {
                switch (this.Type)
                {
                    case PeripheralPropertyType.Integer:
                        this._value = int.Parse(value.ToString());
                        break;
                    case PeripheralPropertyType.Float:
                        this._value = float.Parse(value.ToString());
                        break;
                    case PeripheralPropertyType.Bool:
                        this._value = bool.Parse(value.ToString());
                        break;
                    default:
                        this._value = value;
                        break;
                }
            }
        }
    }
}
