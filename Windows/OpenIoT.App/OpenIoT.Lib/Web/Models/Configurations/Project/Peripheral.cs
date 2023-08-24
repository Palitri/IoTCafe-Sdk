using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenIoT.Lib.Web.Models.Configurations.Project
{
    public class Peripheral
    {
        [JsonPropertyName("Id")]
        public string ScriptId { get; set; }
        public string TypeId { get; set; }
        public string Name { get; set; }

        public List<PeripheralProperty> Properties { get; set; }
        public List<PeripheralSetting> Settings { get; set; }
        public List<PeripheralPin> Pins { get; set; }

        public Peripheral()
        {
            this.Properties = new List<PeripheralProperty>();
            this.Settings = new List<PeripheralSetting>();
            this.Pins = new List<PeripheralPin>();
        }
    }
}
