using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.Web.Models.Configurations.Presets
{
    public class PresetConfiguration
    {
        public List<PresetProperty> Properties { get; set; }

        public PresetConfiguration()
        {
            this.Properties = new List<PresetProperty>();
        }

    }
}
