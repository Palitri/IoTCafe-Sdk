using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.Web.Network
{
    internal class NetworkRequest
    {
        public string BearerToken { get; set; }
        public string RequestBody { get; set; }
        public string HttpMethod { get; set; }

        public NetworkRequest()
        {
            this.HttpMethod = "GET";
        }

        public async Task<string> SendRequestAsync(string requestUrl)
        {
            WebRequest request = WebRequest.Create(requestUrl);
            request.Method = this.HttpMethod;
            if (!String.IsNullOrWhiteSpace(this.RequestBody))
            {
                Stream inputStream = request.GetRequestStream();
                StreamWriter inputStreamWriter = new StreamWriter(inputStream);
                inputStreamWriter.Write(this.RequestBody);

                inputStream.Close();
                inputStreamWriter.Close();
            }
            if (!String.IsNullOrWhiteSpace(this.BearerToken))
                request.Headers.Set("Authorization", " Bearer " + this.BearerToken);


            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            Console.WriteLine(response.StatusDescription);
            Stream outputStream = response.GetResponseStream();
            StreamReader outputStreamReader = new StreamReader(outputStream);
            string responseString = outputStreamReader.ReadToEnd();


            outputStreamReader.Close();
            outputStream.Close();
            response.Close();

            return responseString;
        }
    }
}
