using Palitri.OpenIoT.Web.Models.Configurations.Presets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palitri.OpenIoT.Web.Models
{
    public class Preset
    {
        public string Name { get; set; }
        public string ProjectId { get; set; }
        public string ProjectPresetId { get; set; }
        public PresetConfiguration Config { get; set; }

        public Preset()
        {
            this.Config = new PresetConfiguration();
        }
    }
}
