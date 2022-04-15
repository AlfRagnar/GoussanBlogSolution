variable "resource_group_name_prefix" {
  default     = "rg"
  description = "Prefix of the resource group name that's combined with a random ID so name is unique"
}

variable "resource_group_location" {
  default     = "westeurope"
  description = "Location of the resource group"
}

variable "resource_group_id" {
  default     = "goussanMedia"
  description = "ID of the resource group"
}
