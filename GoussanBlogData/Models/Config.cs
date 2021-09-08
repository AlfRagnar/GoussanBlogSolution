namespace GoussanBlogData.Models
{
    /// <summary>
    /// Here the references to different configurations are defined, to reduce the need for magic strings
    /// </summary>
    public class Config
    {

        /// <summary>
        /// Reference to Azure Storage Connection String
        /// </summary>
        public static string AzureStorageConnectionString { get; set; }
        /// <summary>
        /// Reference to Azure Cosmos DB Connection String
        /// </summary>
        public static string AzureCosmosConnectionString { get; set; }
        /// <summary>
        /// Reference to Azure Storage Blob Endpoint
        /// </summary>
        public static string AzureStorageBlob { get; set; }
        /// <summary>
        /// Reference to Azure Storage Queue Endpoint
        /// </summary>
        public static string AzureStorageQueue { get; set; }
        /// <summary>
        /// Reference to Azure App Insight Endpoint with Key
        /// </summary>
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
        /// <summary>
        /// Azure Active Directory Tenant Domain
        /// </summary>
        public static string AadTenantDomain { get; set; }
        /// <summary>
        /// Azure Active Directory Tenant ID
        /// </summary>
        public static string AadTenantId { get; set; }
        /// <summary>
        /// Name of the Account this AAD instance is tied to
        /// </summary>
        public static string AccountName { get; set; }
        /// <summary>
        /// Name of the Resource Group the AAD Instance is tied to
        /// </summary>
        public static string ResourceGroup { get; set; }
        /// <summary>
        /// Subscription ID the AAD Instance is tied to
        /// </summary>
        public static string SubscriptionId { get; set; }
        /// <summary>
        /// ARM AAD Audience that will consume this product
        /// </summary>
        public static string ArmAadAudience { get; set; }
        /// <summary>
        /// ARM Endpoint to use
        /// </summary>
        public static string ArmEndpoint { get; set; }
        /// <summary>
        /// AAD Client ID for this App
        /// </summary>
        public static string AadClientId { get; set; }
        /// <summary>
        /// AAD Client Secret for this App
        /// </summary>
        public static string AadSecret { get; set; }
        /// <summary>
        /// Another Secret?
        /// </summary>
        public static string Secret { get; set; }
        /// <summary>
        /// Streaming Endpoint
        /// </summary>
        public static string StreamingEndpoint { get; set; }
        /// <summary>
        /// Azure Application Insight Instrument Key, not Endpoint or anything, just key
        /// </summary>
        public static string AzureAppInsightInstrumentKey { get; set; }
    }
}