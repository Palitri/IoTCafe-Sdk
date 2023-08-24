using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.Tools.Utils
{
    internal class MimeUtils
    {
        private static List<Tuple<byte[], Dictionary<string, string>>> signatures = new List<Tuple<byte[], Dictionary<string, string>>>()
        {
            new Tuple<byte[], Dictionary<string, string>>(new byte[] { 77, 90 }, new Dictionary<string, string>() { { "exe", "application/octet-stream" }, { "dll", "application/octet-stream" } }),
            new Tuple<byte[], Dictionary<string, string>>(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82 }, new Dictionary<string, string>() { { "png", "image/png" } }),
            new Tuple<byte[], Dictionary<string, string>>(new byte[] { 255, 216, 255 }, new Dictionary<string, string>() { { "jpg", "image/jpeg" }, {"jpeg", "image/jpeg" } }),
            new Tuple<byte[], Dictionary<string, string>>(new byte[] { 71, 73, 70, 56 }, new Dictionary<string, string>() { { "gif", "image/gif" } }),
            new Tuple<byte[], Dictionary<string, string>>(new byte[] { 0, 0, 1, 0 }, new Dictionary<string, string>() { { "ico", "image/x-icon" } }),
            new Tuple<byte[], Dictionary<string, string>>(new byte[] { 73, 73, 42, 0 }, new Dictionary<string, string>() { { "tiff", "image/tiff" } }),
            new Tuple<byte[], Dictionary<string, string>>(new byte[] { 66, 77 }, new Dictionary<string, string>() { { "bmp", "image/bmp" } }),
            new Tuple<byte[], Dictionary<string, string>>(new byte[] { 0, 1, 0, 0, 0 }, new Dictionary<string, string>() { { "ttf", "application/x-font-ttf" } }),
            new Tuple<byte[], Dictionary<string, string>>(new byte[] { 37, 80, 68, 70, 45, 49, 46 }, new Dictionary<string, string>() { { "pdf", "application/pdf" } }),
            new Tuple<byte[], Dictionary<string, string>>(new byte[] { 208, 207, 17, 224, 161, 177, 26, 225 }, new Dictionary<string, string>() { { "doc", "application/msword" } }),
            new Tuple<byte[], Dictionary<string, string>>(new byte[] { 255, 251, 48 }, new Dictionary<string, string>() { { "mp3", "audio/mpeg" } }),
            new Tuple<byte[], Dictionary<string, string>>(new byte[] { 79, 103, 103, 83, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0 }, new Dictionary<string, string>() { { "ogg", "audio/ogg" }, { "oga", "audio/ogg" }, { "ogx", "application/ogg" }, { "ogm", "video/ogg" } }),
            new Tuple<byte[], Dictionary<string, string>>(new byte[] { 82, 73, 70, 70 }, new Dictionary<string, string>() { { "avi", "video/x-msvideo" }, { "wav", "audio/x-wav" } }),
            new Tuple<byte[], Dictionary<string, string>>(new byte[] { 48, 38, 178, 117, 142, 102, 207, 17, 166, 217, 0, 170, 0, 98, 206, 108 }, new Dictionary<string, string>() { { "wma", "audio/x-ms-wma" }, { "wmv", "video/x-ms-wmv" } }),
            new Tuple<byte[], Dictionary<string, string>>(new byte[] { 80, 75, 3, 4 }, new Dictionary<string, string>() { { "zip", "application/x-zip-compressed" }, { "docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" } }),
            new Tuple<byte[], Dictionary<string, string>>(new byte[] { 82, 97, 114, 33, 26, 7, 0 }, new Dictionary<string, string>() { { "rar", "application/x-rar-compressed" } }),
            new Tuple<byte[], Dictionary<string, string>>(new byte[] { 70, 87, 83 }, new Dictionary<string, string>() { { "swf", "application/x-shockwave-flash" } }),
            new Tuple<byte[], Dictionary<string, string>>(new byte[] { 100, 56, 58, 97, 110, 110, 111, 117, 110, 99, 101 }, new Dictionary<string, string>() { { "torrent", "application/x-bittorrent" } }),
        };

        public string GetMime(IEnumerable<byte> file, string fileName)
        {
            string mime = "application/octet-stream"; //DEFAULT UNKNOWN MIME TYPE

            string extension = String.IsNullOrEmpty(fileName) || Path.GetExtension(fileName) == null ? string.Empty : Path.GetExtension(fileName).ToLower().Trim('.');

            foreach (var signature in signatures)
            {
                if (signature.Item1.SequenceEqual(file.Take(signature.Item1.Length)))
                {
                    if (!signature.Item2.TryGetValue(extension, out mime))
                        mime = signature.Item2.Values.First();

                    break;
                }
            }

            return mime;
        }
    }
}
