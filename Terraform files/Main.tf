resource "goussanMedia" "rg-name" {
  prefix = var.resource_group_name_prefix
  id     = var.resource_group_id
}

resource "azurerm_resource_group" "rg" {
  name     = goussanMedia.rg-name.id
  location = var.resource_group_location
}

resource "azurerm_cosmosdb_account" "goussanCosmos" {
  name                = "${azurerm_resource_group.rg.name}-cosmosdb"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  offer_type          = "serverless"
  kind                = "GlobalDocumentDb"
}

resource "azurerm_key_vault" "goussanKeyVault" {
  name                        = "${azurerm_resource_group.rg.name}-keyvault"
  location                    = azurerm_resource_group.rg.location
  resource_group_name         = azurerm_resource_group.rg.name
  enabled_for_disk_encryption = true
  tenant_id                   = data.azurerm_client_config.current.tenant_id
  soft_delete_retention_days  = 7
  purge_protection_enabled    = false
  enabled_for_deployment      = true

  sku_name = "standard"

  access_policy = {
    tenant_id = data.azurerm_client_config.current.tenant_id
    object_id = data.azurerm_client_config.current.object_id

    key_permissions = [
      "Get",
    ]

    secret_permissions = [
      "Get",
    ]

    storage_permissions = [
      "Get"
    ]
  }
}

resource "azurerm_container_registry" "acr" {
  name                = "${azurerm_resource_group.rg.name}-containerregistry"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
}

resource "azurerm_kubernetes_cluster" "aks" {
  name                = "${azurerm_resource_group.rg.name}-aks"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  dns_prefix          = "goussan"

  default_node_pool {
    name       = "aks-pool"
    node_count = 1
    vm_size    = "Standard_D2_v2"
  }

  identity {
    type = "SystemAssigned"
  }

  tags = {
    Environments = "Dev"
  }
}

resource "azurerm_role_assignment" "azure-role-assignment" {
  principal_id                     = azurerm_kubernetes_cluster.aks.kubelet_identity[0].object_id
  role_definition_name             = "AcrPull"
  scope                            = azurerm_container_registry.acr.id
  skip_service_principal_aad_check = true
}
