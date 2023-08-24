using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenIoT.Lib.Tools.Persistence
{
    public class DefaultPersistence : IPersistence
    {
        public string FileName { get; private set; }

        private string _fileContent = null;
        private string FileContent 
        {
            get 
            { 
                if (String.IsNullOrWhiteSpace(this._fileContent))
                    this._fileContent = File.Exists(this.FileName) ? File.ReadAllText(this.FileName) : String.Empty;

                return this._fileContent;
            }

            set 
            {
                if (this._fileContent != value)
                {
                    this._fileContent = value;
                    File.WriteAllText(this.FileName, this._fileContent);
                }
            }
        }

        private Dictionary<string, string> _objects = null;
        private Dictionary<string, string> Objects
        {
            get
            {
                if (this._objects == null)
                    try
                    {
                        this._objects = JsonSerializer.Deserialize<Dictionary<string, string>>(this.FileContent, this.serializationOptions);
                    }
                    catch
                    {
                        this._objects = new Dictionary<string, string>();
                    }


                return this._objects;
            }
        }

        private JsonSerializerOptions serializationOptions;

        public DefaultPersistence(string fileName)
        {
            this.FileName = fileName;

            this.serializationOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                PropertyNameCaseInsensitive = true,
                IncludeFields = false
            };
        }

        private void Save()
        {
            this.FileContent = JsonSerializer.Serialize(this.Objects, this.serializationOptions);
        }

        public string GetString(string key)
        {
            if (!this.Objects.ContainsKey(key))
                return String.Empty;

            return this.Objects[key].ToString();
        }

        public void SetString(string key, string value)
        {
            this.Objects[key] = value;

            this.Save();
        }



        public T GetObject<T>(string key)
        {
            if (!this.Objects.ContainsKey(key))
                return default(T);

            return JsonSerializer.Deserialize<T>(this.Objects[key], this.serializationOptions);
        }

        public void SetObject(string key, object value)
        {
            this.Objects[key] = JsonSerializer.Serialize(value, this.serializationOptions);

            this.Save();
        }



        public void Delete(string key)
        {
            this.Objects.Remove(key);

            this.Save();
        }
    }
}
