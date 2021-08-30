using Newtonsoft.Json;

namespace GoussanFunction.Models
{
    internal class CosmosVideoModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("filename")]
        public string Filename { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("created")]
        public string Created { get; set; }

        [JsonProperty("modified")]
        public string Modified { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("userid")]
        public string Userid { get; set; }

        [JsonProperty("locator")]
        public string Locator { get; set; }

        [JsonProperty("outputasset")]
        public string Outputasset { get; set; }

        [JsonProperty("streamingpaths")]
        public object Streamingpaths { get; set; }

        [JsonProperty("blogid")]
        public object Blogid { get; set; }

        [JsonProperty("title")]
        public object Title { get; set; }

        [JsonProperty("description")]
        public object Description { get; set; }

        [JsonProperty("_rid")]
        public string Rid { get; set; }

        [JsonProperty("_self")]
        public string Self { get; set; }

        [JsonProperty("_etag")]
        public string Etag { get; set; }

        [JsonProperty("_attachments")]
        public string Attachments { get; set; }

        [JsonProperty("_ts")]
        public int Ts { get; set; }
    }
}
