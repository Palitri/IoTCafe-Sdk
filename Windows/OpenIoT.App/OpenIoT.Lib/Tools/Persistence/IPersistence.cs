using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.Tools.Persistence
{
    public interface IPersistence
    {
        void SetString(string key, string value);
        string GetString(string key);
        void SetObject(string key, object value);
        public T GetObject<T>(string key);
        void Delete(string key);
    }
}
