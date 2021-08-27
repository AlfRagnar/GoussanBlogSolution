using GoussanFunction.Models;
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
}
