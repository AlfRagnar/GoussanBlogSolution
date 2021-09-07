
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GoussanBlogData.Models.DatabaseModels;
/// <summary>
/// Model used to store Chat Messages in Database
/// </summary>
public class LoggedMessage
{
    [JsonProperty("User")]
    public string User { get; set; }

    [JsonProperty("Message")]
    public string Message { get; set; }
    [JsonProperty("creationtime")]
    public string Creationtime { get; set; }

    [JsonProperty("id")]
    public string Id { get; set; }

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
