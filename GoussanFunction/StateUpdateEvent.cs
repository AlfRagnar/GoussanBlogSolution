using Azure.Messaging.EventGrid;
using GoussanFunction.Models;
using Microsoft.Azure.Cosmos;
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
            [CosmosDB(
                databaseName: "GoussanServerless",
                containerName:"Media",
                Connection = "CosmosDBServerless")]
            CosmosClient cosmosClient,
            ILogger log)
        {
            string database = "GoussanServerless";
            string collection = "Media";

            log.LogInformation("Processing Event Trigger from AMS");
            try
            {
                var containerClient = cosmosClient.GetContainer(database, collection);
                log.LogInformation($"Event: {gridEvent.EventType}");
                // Extract data from EVENT
                JToken gridDataToken = JObject.FromObject(gridEvent.Data);
                JToken output = gridDataToken.SelectToken("output");
                JToken state = output.SelectToken("state");
                JToken assetName = output.SelectToken("assetName");
                string ID = assetName.ToString().Split("-").First();
                log.LogInformation($"Updating Document: {ID}");

                CosmosVideoModel dbVid = await containerClient.ReadItemAsync<CosmosVideoModel>(ID, new PartitionKey(ID));
                log.LogInformation(dbVid.ToString());

                dbVid.Outputasset = assetName.ToString();
                dbVid.State = state.ToString();
                dbVid.Modified = gridEvent.EventTime.ToUniversalTime().ToString();
                log.LogInformation(dbVid.State);

                // Sending Document to Cosmos DB
                log.LogInformation("Sending Document to Cosmos DB");
                var response = await containerClient.UpsertItemAsync<CosmosVideoModel>(dbVid, new PartitionKey(ID));
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