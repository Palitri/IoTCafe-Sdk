using OpenIoT.Lib.Web.Models;
using OpenIoT.Lib.Web.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace OpenIoT.Lib.Web.Api
{
    public class OpenIoTService
    {
        public string? Token { get; set; }

        public string BaseUrl { get; set; }
        
        private JsonSerializerOptions serializationOptions;

        public OpenIoTService()
        {
            this.Token = null;

            this.BaseUrl = "http://api.iot.cafe";

            this.serializationOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                PropertyNameCaseInsensitive = true,
                IncludeFields = false,
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
            };
        }

        public async Task<UserLogin?> RequestUserLogin(String email, String password)
        {
            string response = await new NetworkRequest()
            {
                RequestBody = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\"}",
                HttpMethod = "POST"
            }.SendRequestAsync(this.BaseUrl + "/user/login");

            UserLogin? userLogin = this.DeserializeJson<UserLogin>(response);

            this.Token = userLogin?.Token;

            return userLogin;
        }

        public async Task<IEnumerable<Project>> RequestUserProjects()
        {
            string response = await new NetworkRequest()
            {
                BearerToken = this.Token,
                HttpMethod = "GET"
            }.SendRequestAsync(this.BaseUrl + "/projects");

            using (JsonDocument jdoc = JsonDocument.Parse(response))
            {
                IEnumerable<Project>? projectsList = this.DeserializeJson<List<Project>>(jdoc.RootElement.GetProperty("userProjects"));

                return projectsList;
            }
        }

        public async Task<Tuple<Project, PresetsCollection>> RequestUserProject(string projectId)
        {
            string response = await new NetworkRequest()
            {
                BearerToken = this.Token,
                HttpMethod = "GET"
            }.SendRequestAsync(this.BaseUrl + "/project/" + projectId);

            using (JsonDocument jdoc = JsonDocument.Parse(response))
            {
                Project? project = jdoc.RootElement.GetProperty("project").Deserialize<Project>(this.serializationOptions);
                PresetsCollection? projectPresets = this.DeserializeJson<PresetsCollection>(jdoc.RootElement.GetProperty("projectPresets"));

                return new Tuple<Project, PresetsCollection>(project, projectPresets);
            }
        }

        public async Task<PresetsCollection> RequestProjectPresets(string projectId)
        {
            string response = await new NetworkRequest()
            {
                BearerToken = this.Token,
                HttpMethod = "GET"
            }.SendRequestAsync(this.BaseUrl + "/project/" + projectId + "/presets");

            using (JsonDocument jdoc = JsonDocument.Parse(response))
            {
                PresetsCollection? presets = this.DeserializeJson<PresetsCollection>(jdoc.RootElement.GetProperty("projectPresets"));

                return presets;
            }
        }

        public async Task<Preset> RequestSaveProjectPreset(string projectId, Preset preset)
        {
            string response = await new NetworkRequest()
            {
                BearerToken = this.Token,
                HttpMethod = "POST",
                RequestBody = this.SerializeObject(preset)
            }.SendRequestAsync(this.BaseUrl + "/project/" + projectId + "/presets");

            using (JsonDocument jdoc = JsonDocument.Parse(response))
            {
                Preset? projectPreset = this.DeserializeJson<Preset>(jdoc.RootElement.GetProperty("projectPreset"));

                return projectPreset;
            }
        }

        public async Task<Preset> RequestUpdateProjectPreset(Preset preset)
        {
            string response = await new NetworkRequest()
            {
                BearerToken = this.Token,
                HttpMethod = "PUT",
                RequestBody = this.SerializeObject(preset)
            }.SendRequestAsync(this.BaseUrl + "/project/" + preset.ProjectId + "/preset/" + preset.ProjectPresetId);

            using (JsonDocument jdoc = JsonDocument.Parse(response))
            {
                Preset? projectPreset = this.DeserializeJson<Preset>(jdoc.RootElement.GetProperty("projectPreset"));

                return projectPreset;
            }
        }

        public async Task<bool> RequestDeleteProjectPreset(Preset preset)
        {
            string response = await new NetworkRequest()
            {
                BearerToken = this.Token,
                HttpMethod = "DELETE",
                RequestBody = this.SerializeObject(preset)
            }.SendRequestAsync(this.BaseUrl + "/project/" + preset.ProjectId + "/preset/" + preset.ProjectPresetId);

            return this.IsResponseSuccess(response);
        }

        public string SerializeObject(object obj)
        {
            return JsonSerializer.Serialize(obj, this.serializationOptions);
        }

        public T DeserializeJson<T>(JsonElement json)
        {
            return json.Deserialize<T>(this.serializationOptions);
        }

        public T DeserializeJson<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, this.serializationOptions);
        }

        public bool IsResponseSuccess(string response)
        {
            using (JsonDocument jdoc = JsonDocument.Parse(response))
            {
                int responseCode;
                if (!jdoc.RootElement.GetProperty("responseCode").TryGetInt32(out responseCode))
                    return false;

                return responseCode == 1;
            }
        }
    }
}
