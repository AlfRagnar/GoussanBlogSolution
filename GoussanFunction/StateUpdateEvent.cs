using GoussanFunction.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GoussanFunction
{
    public static class StateUpdateEvent
    {
        [FunctionName("StateUpdateEvent")]
        public static async Task RunAsync(
            [EventGridTrigger] EventGridEvent gridEvent,
            [CosmosDB(databaseName: "GoussanServerless", collectionName: "Media", ConnectionStringSetting = "CosmosDBServerless")] DocumentClient client,
            ILogger log)
        {
            string database = "GoussanServerless";
            string collection = "Media";

            log.LogInformation("Processing Event Trigger from AMS");
            try
            {
                log.LogInformation($"Event: {gridEvent.EventType}");
                // Extract data from EVENT
                JToken gridDataToken = JObject.FromObject(gridEvent.Data);
                JToken output = gridDataToken.SelectToken("output");
                JToken state = output.SelectToken("state");
                JToken assetName = output.SelectToken("assetName");
                string ID = assetName.ToString().Split("-").First();
                log.LogInformation($"Updating Document: {ID}");

                // Fetching Document from Cosmos DB
                Uri DocumentLink = UriFactory.CreateDocumentUri(database, collection, ID);
                RequestOptions dbVidOptions = new()
                {
                    PartitionKey = new PartitionKey(ID)
                };
                CosmosVideoModel dbVid = await client.ReadDocumentAsync<CosmosVideoModel>(DocumentLink, dbVidOptions);
                log.LogInformation(dbVid.ToString());

                dbVid.Outputasset = assetName.ToString();
                dbVid.State = state.ToString();
                dbVid.Modified = gridEvent.EventTime.ToUniversalTime().ToString();
                log.LogInformation(dbVid.State);


                Uri DocumentCollectionUri = UriFactory.CreateDocumentCollectionUri(database, collection);
                Uri DatabaseLink = UriFactory.CreateDatabaseUri(database);

                // Sending Document to Cosmos DB
                log.LogInformation("Sending Document to Cosmos DB");
                var response = await client.ReplaceDocumentAsync(DocumentLink, dbVid, dbVidOptions);
                log.LogInformation($"Cosmos Response: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
                log.LogError(ex.Message);
            }
        }
    }
}