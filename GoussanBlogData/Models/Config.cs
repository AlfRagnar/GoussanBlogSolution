namespace GoussanBlogData.Models
{
    public class Config
    {

        public static string AzureStorageConnectionString { get; set; }
        public static string AzureCosmosConnectionString { get; set; }
        public static string AzureStorageBlob { get; set; }
        public static string AzureStorageQueue { get; set; }
        public static string AzureAppInsight { get; set; }
        /// <summary>
        /// Name of the application
        /// </summary>
        public static string AppName { get; set; }
        /// <summary>
        /// Region where the Application is running
        /// </summary>
        public static string AppRegion { get; set; }
        /// <summary>
        /// Cosmos Database name
        /// </summary>
        public static string CosmosDBName { get; set; }
        /// <summary>
        /// User Container in Cosmos DB
        /// </summary>
        public static string CosmosUser { get; set; }
        /// <summary>
        /// Media Container in Cosmos DB
        /// </summary>
        public static string CosmosMedia { get; set; }
        /// <summary>
        /// Chat Container in Cosmos DB
        /// </summary>
        public static string CosmosChat { get; set; }
        public static string AadTenantDomain { get; set; }
        public static string AadTenantId { get; set; }
        public static string AccountName { get; set; }
        public static string ResourceGroup { get; set; }
        public static string SubscriptionId { get; set; }
        public static string ArmAadAudience { get; set; }
        public static string ArmEndpoint { get; set; }
        public static string AadClientId { get; set; }
        public static string AadSecret { get; set; }
        public static string Secret { get; set; }
        public static string StreamingEndpoint { get; set; }
        public static string AzureAppInsightInstrumentKey { get; set; }
    }
}