namespace Goussanjarga.Models
{
    public class Config
    {

        public static string AzureStorageConnectionString { get; set; }
        public static string AzureCosmosConnectionString { get; set; }
        public static string AzureStorageBlob { get; set; }
        public static string AzureStorageQueue { get; set; }
        public static string AzureAppInsight { get; set; }
        public static string AppName { get; set; }
        public static string AppRegion { get; set; }
        public static string CosmosDBName { get; set; }
        public static string CosmosVideos { get; set; }
        public static string AadTenantDomain { get; set; }
        public static string AadTenantId { get; set; }
        public static string AccountName { get; set; }
        public static string ResourceGroup { get; set; }
        public static string SubscriptionId { get; set; }
        public static string ArmAadAudience { get; set; }
        public static string ArmEndpoint { get; set; }
        public static string AadClientId { get; set; }
        public static string AadSecret { get; set; }
    }
}