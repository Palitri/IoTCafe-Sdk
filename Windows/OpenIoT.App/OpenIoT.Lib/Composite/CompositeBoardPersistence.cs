using OpenIoT.Lib.Tools.Persistence;
using OpenIoT.Lib.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.Composite
{
    public class CompositeBoardPersistence
    {
        public IPersistence persistence;

        public CompositeBoardPersistence(IPersistence persistence)
        {
            this.persistence = persistence;
        }



        public void setToken(String token)
        {
            this.persistence.SetString("token", token);
        }

        public String getToken()
        {
            return this.persistence.GetString("token");
        }



        public void SetProject(Project value)
        {
            this.persistence.SetObject("project", value);
        }

        public Project GetProject()
        {
            Project result = this.persistence.GetObject<Project>("project");

            return result;
        }

        public void DeleteProject()
        {
            this.persistence.Delete("project");
        }



        public void SetPresets(PresetsCollection value)
        {
            this.persistence.SetObject("presets", value);
        }

        public PresetsCollection GetPresets()
        {
            try
            {
                PresetsCollection result = this.persistence.GetObject<PresetsCollection>("presets");

                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public void DeletePresets()
        {
            this.persistence.Delete("presets");
        }



        public void SetDeviceName(String deviceName)
        {
            this.persistence.SetString("deviceName", deviceName);
        }

        public String GetDeviceName()
        {
            return this.persistence.GetString("deviceName");
        }
    }
}
