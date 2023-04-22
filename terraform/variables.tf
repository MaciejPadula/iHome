variable "env" {
  type        = string
  description = "Environment prefix of resource group"
}

variable "sql_login" {
  type        = string
  description = "Login for SQL database"
}

variable "sql_password" {
  type        = string
  description = "Password for SQL datasbase"
}

variable "resource_group_location" {
  default     = "westeurope"
  description = "Location of the resource group."
}