using System;
using System.IO;
using System.Net;
namespace TwitterRepository
{
    internal class RequestManager
    {
        /// <summary>
        /// Gets request string from url
        /// </summary>
        /// <param name="url">url address</param>
        /// <returns>response string</returns>
        public static string GetRequestUrl(string url)
        {
            var responseString = String.Empty;

            try
            {
                var request = WebRequest.Create(url);
                request.ContentType = "application/json";
                request.Method = "GET";

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    if (response != null && response.StatusCode == HttpStatusCode.OK)
                        using (var reader = new StreamReader(response.GetResponseStream()))
                        {
                            responseString =  reader.ReadToEnd();
                        }

                }
            }
            catch(Exception)
            {
                return null;
            }

            return responseString;
        }
    }
}