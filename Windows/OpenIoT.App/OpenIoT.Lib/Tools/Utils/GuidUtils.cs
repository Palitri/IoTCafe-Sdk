using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenIoT.Lib.Tools.Utils
{
    // Class is provided for comatibility with other system components (web api, board),
    // for fixing ambiguity between multiple existing Guid formats
    // and for serialization reasons, because OpenIoT device de/serializes guid as a stream, disregarding guid segments endianness
    public class GuidUtils
    {
        static public string Zero()
        {
            return "00000000-0000-0000-0000-000000000000";
        }

        static public bool IsZero(string guid)
        {
            return GuidUtils.Zero().Equals(GuidUtils.Normalize(guid));
        }

        static public bool IsValid(string guid)
        {
            return new Regex("^[0-9A-Fa-f]+$").IsMatch(guid);
        }

        static public string FromBytes(byte[] bytes)
        {
            return GuidUtils.Normalize(StringUtils.BytesToHex(bytes));
        }

        static public byte[] ToBytes(string guid)
        {
            return StringUtils.HexToBytes(GuidUtils.Shorten(guid));
        }

        static public string Shorten(string guid)
        {
            return new Regex("[^0-9A-Fa-f]").Replace(guid, "");
            //return guid.Replace("-", "");
        }

        static public string Normalize(string guid)
        {
            guid = GuidUtils.Shorten(guid);

            guid = guid.Insert(8, "-");
            guid = guid.Insert(13, "-");
            guid = guid.Insert(18, "-");
            guid = guid.Insert(23, "-");

            return guid;
        }

        static public Guid ToGuid(string guid)
        {
            return new Guid(GuidUtils.Normalize(guid));
        }

        static public string ToString(Guid guid)
        {
            return GuidUtils.Normalize(guid.ToString());
        }
    }
}
