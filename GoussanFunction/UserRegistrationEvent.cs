using GoussanFunction.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoussanFunction
{
    public static class UserRegistrationEvent
    {
        [FunctionName("UserRegistrationEvent")]
        public static async Task Run(
            [CosmosDBTrigger(
            databaseName: "GoussanServerless",
            collectionName: "User",
            ConnectionStringSetting = "CosmosDBServerless",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true,
            LeaseCollectionPrefix = "registrationEvent")]IReadOnlyList<Document> input,
            ILogger log,
            [SendGrid(ApiKey = "SendGridAPIKey")] IAsyncCollector<SendGridMessage> messageCollector)
        {
            if (input != null && input.Count > 0)
            {
                log.LogInformation("Documents to be modified: " + input.Count);
                log.LogInformation("Deserializing documents");

                foreach (var document in input)
                {
                    IncDocument incomingDoc = JsonConvert.DeserializeObject<IncDocument>(document.ToString());

                    if (incomingDoc.Status == "Pending")
                    {
                        var confirmURI = new UriBuilder
                        {
                            Scheme = "https",
                            Host = "goussanmedia.com",
                            Path = $"confirm/{incomingDoc.Confirmationcode}"
                        };
                        log.LogInformation(incomingDoc.Email);
                        var message = new SendGridMessage();
                        message.AddTo(incomingDoc.Email);
                        message.AddContent("text/html", $"Hello {incomingDoc.Username} and welcome to Goussanjarga Media Services. Here you can Upload Videos, Images and create Blog Posts and it is all powered by Microsoft Azure and their backend services. To start using our services, you need to first activate your account by clicking this link: {confirmURI}");
                        message.SetFrom(new EmailAddress("support@goussanmedia.com"));
                        message.SetSubject("Activate Account");
                        await messageCollector.AddAsync(message);
                    }
                }
            }
        }
    }
}
