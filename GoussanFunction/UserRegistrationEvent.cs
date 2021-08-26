using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SendGrid.Helpers.Mail;

namespace GoussanFunction
{
    public static class UserRegistrationEvent
    {
        [FunctionName("UserRegistrationEvent")]
        public static async Task Run(
            [CosmosDBTrigger(
            databaseName: "GoussanDatabase",
            collectionName: "User",
            ConnectionStringSetting = "CosmosDbCon",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true,
            LeasesCollectionThroughput = 400, LeaseCollectionPrefix = "regEvent")]IReadOnlyList<Document> input, 
            ILogger log,
            [SendGrid(ApiKey = "SendGridAPIKey")] IAsyncCollector<SendGridMessage> messageCollector)
        {
            if (input != null && input.Count > 0)
            {
                log.LogInformation("Documents modified " + input.Count);
                log.LogInformation("Deserializing documents");

                foreach(var document in input)
                {
                    IncDocument incomingDoc = JsonConvert.DeserializeObject<IncDocument>(document.ToString());
                    log.LogInformation(incomingDoc.email);
                    
                    var message = new SendGridMessage();
                    message.AddTo(incomingDoc.email);
                    message.AddContent("text/html", "SENT FROM AZURE FUNCTIONS USING SENDGRID INTEGRATION");
                    message.SetFrom(new EmailAddress("noreply@goussanjarga.com"));
                    message.SetSubject("Activate Account");
                    await messageCollector.AddAsync(message);
                }
            }
        }
    }

    internal class IncDocument
    {
        
            public string id { get; set; }
            public string created { get; set; }
            public string username { get; set; }
            public string password { get; set; }
            public string email { get; set; }
            public object medias { get; set; }
            public string _rid { get; set; }
            public string _self { get; set; }
            public string _etag { get; set; }
            public string _attachments { get; set; }
            public int _ts { get; set; }
            public int _lsn { get; set; }
    }

    internal class OutgoingEmail
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

    }
}
