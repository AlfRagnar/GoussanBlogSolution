# GoussanBlogData
This is a project I'm currently working on. Feel free to use at your own risk.

Link to API for preview: https://goussanblogdata.azurewebsites.net/


## Backend API in .NET 6.0 Preview

This API is to handle interaction from Client App to Azure Services, including interaction with Cosmos DB, Azure Media Services and Azure Blob Storage.

## Setup
### Requirements
* Azure Account
* Cosmos DB Account
* Azure Media Services
* Azure AD ( need to register app in Azure AD to use Azure Media Services API )
* Azure Functions / App Service Plan
* Optional: Azure Key Vault

Before initializing/building:
1. Create a <b>appsettings.json</b> file
2. populate it with necessary information. Example: 
````

{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "GoussanBlogSecret":"JWT_MASTER_KEY",
  "GoussanCosmos": "COSMOS_DB_CONNECTION_STRING"
  "CosmosDb": {
    "DatabaseName": "COSMOS_DB_NAME",
    "Containers": {
      "User": {
        "containerName": "User",
        "paritionKeyPath": "/id"
      },
      "Media": {
        "containerName": "Media",
        "paritionKeyPath": "/id"
      }
    }
  },
  "AadClientId":"AZURE_AD_CLIENT_ID",
  "AadSecret":"AZURE_AD_CLIENT_SECRET",
  "AadTenantId":"AZURE_AD_TENANT_ID",
  "AADAccountName":"AZURE_AD_ACCOUNT_NAME",
  "AADResourceGroup":"AZURE_AD_RESOURCE_GROUP", // Resource group Azure AD is tied to
  "AADSubscriptionId":"AZURE_AD_SUBSCRIPTION_ID",
  "AzureAd": {
    "AadTenantDomain": "YOUR_AZURE_AD_DOMAIND",
    "ArmAadAudience": "https://management.core.windows.net",
    "ArmEndpoint": "https://management.azure.com"
  },
  "GoussanStorage":"AZURE_BLOB_STORAGE_CONNECTION_STRING",
  "AppInsightConString":"APPLICATION_INSIGHT_CONNECTION_STRING"
}


````
I would strongly suggest setting up your own Azure Key Vault for testing purposes and using it for storing secrets and connection strings. With it you can ensure vital information is only available at runtime and you can control access to it along with other security options available.

3. Start the API ( GoussanBlogData )

The API will on start up configure back end services to ensure that it can run properly and have access to all resources it needs. So it will go through a step-by-step process where it checks these services in sequence:
1. Cosmos DB ( InitializeCosmosClientInstanceAsync Task)
2. Azure Media Services ( InitializeMediaService Task )
3. Azure Blob Storage ( InitializeStorageClientInstance Task )


## Current Services in Use:
* Cosmos DB
* Azure Media Services
* Azure Blob Storage


## Project Overview

* Program - Startup, configure settings at runtime and configure Client Instances
* Services - Folder containing Interfaces and Service definition
* Models - Data models used in interactions the API handles, Models it accepts and models it sends out
* Controllers - API Endpoint definition and interactions. Action definition is handled here along with where services are called
* Utils - Misc Utilities used within the project, mainly definition of stuff like AutoMapper, JWT middleware and attribute modification.
 
