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

                foreach (var document in input)
                {
                    IncDocument incomingDoc = JsonConvert.DeserializeObject<IncDocument>(document.ToString());
                    log.LogInformation(incomingDoc.email);

                    var message = new SendGridMessage();
                    message.AddTo(incomingDoc.email);
                    message.AddContent("text/html", $"Hello {incomingDoc.username} and welcome to Goussanjarga Media Services. Here you can Upload Videos, Images and create Blog Posts and it is all powered by Microsoft Azure and their backend services. To start using our services, you need to first activate your account by clicking this link: INSERT_LINK");
                    message.SetFrom(new EmailAddress("support@goussanmedia.com"));
                    message.SetSubject("Activate Account");
                    await messageCollector.AddAsync(message);
                }
            }
        }
    }
}
