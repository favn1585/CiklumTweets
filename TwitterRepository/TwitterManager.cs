using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TwitterRepository.Entities;

namespace TwitterRepository
{
    public class TwitterManager
    {
        public static List<Tweet> GetTweets(int count, long? maxId)
        {
            var urlBuilder = new StringBuilder(String.Format(Constants.TimelineUrl, count, Constants.CiklumAccount));

            if (maxId != null)
                urlBuilder.Append(String.Format("&max_id={0}", maxId));

            var jsonTweets = RequestManager.GetRequestUrl(urlBuilder.ToString());

            if(String.IsNullOrEmpty(jsonTweets))
                return new List<Tweet>();

            try
            {
                //make correct Json format
                jsonTweets = "{ \"tweets\" : " + jsonTweets + "}";

                var jobj = Newtonsoft.Json.Linq.JObject.Parse(jsonTweets);
                var tweetsArr = JsonConvert.DeserializeObject<Tweet[]>(jobj["tweets"].ToString(), 
                    new IsoDateTimeConverter() { DateTimeFormat = "ddd MMM dd HH:mm:ss zzzz yyyy" });
                return tweetsArr.ToList();
            }
            catch(Exception)
            {
                return new List<Tweet>();
            }
        }
    }
}
