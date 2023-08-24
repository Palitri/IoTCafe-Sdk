using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenIoT.Lib.Web.Models.Configurations.Project
{
    public class PeripheralProperty
    {
        [JsonPropertyName("Id")]
        public string ScriptId { get; set; }
        public PeripheralPropertyType Type { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
        public string DisplayFormat { get; set; }
        public float Min{ get; set; }
        public float Max{ get; set; }
        public float Step { get; set; }
        public bool InstantUpdate { get; set; }
        public int Semantic { get; set; }
        public bool Visible { get; set; }
    }
}
