using OpenIoT.Lib.Tools.Utils;
using OpenIoT.Lib.Web.Models.Configurations.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.Web.Models
{
    public class Project
    {
        public string ProjectId { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public ProjectConfiguration BoardConfig { get; set; }
        public string SchemeCode { get; set; }
        public string ScriptCode { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdatedDate { get; set; }


        public byte[] GetCompiledSchemeCode()
        {
            return ArrayUtils.CommaSeparatedValuesToBytes(this.SchemeCode);
        }

        public byte[] GetCompiledScriptCode()
        {
            return ArrayUtils.CommaSeparatedValuesToBytes(this.ScriptCode);
        }
    }
}
