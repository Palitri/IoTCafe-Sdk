using OpenIoT.Lib.Board.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.Board.Scanner
{
    public class BoardScanInfo
    {
        public string Port { get; private set; }

        public new Dictionary<int, byte[]> Records { get; private set; }

        public BoardScanInfo()
        {
            this.Port = String.Empty;
            this.Records = new Dictionary<int, byte[]>();
        }

        public BoardScanInfo(string port, Dictionary<int, byte[]> info)
        {
            this.Port = port;
            this.Records = info;
        }

        public override string ToString()
        {
            return String.Join(" ", this.Records.Select(r =>
            {
                string name;
                object value;
                OpenIoTProtocol.GetDevicePropertyFriendly(r.Key, r.Value, out name, out value);
                return value.ToString();
            }));
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return this.GetHashCode() == obj.GetHashCode();
        }
    }
}
