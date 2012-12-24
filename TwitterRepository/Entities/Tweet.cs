using System;
using Newtonsoft.Json;

namespace TwitterRepository.Entities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Tweet
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("created_at")]
        public DateTime PublishingDate { get; set; }

        public Tweet(long id, string text, DateTime publishingDate)
        {
            Id = id;
            Text = text;
            PublishingDate = publishingDate;
        }
    }
}