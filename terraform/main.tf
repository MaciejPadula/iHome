resource "azurerm_resource_group" "ihome-rg" {
  name     = "${var.env}-ihome-rg"
  location = var.resource_group_location
}

resource "azurerm_storage_account" "ihome-sa" {
  name                     = "${var.env}ihomesa"
  resource_group_name      = azurerm_resource_group.ihome-rg.name
  location                 = azurerm_resource_group.ihome-rg.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_storage_queue" "device-data-events" {
  name                     = "device-data-events"
  storage_account_name     = azurerm_storage_account.ihome-sa.name
}

resource "azurerm_mssql_server" "ihome-database" {
  name                         = "${var.env}-ihome-database"
  resource_group_name          = azurerm_resource_group.ihome-rg.name
  location                     = azurerm_resource_group.ihome-rg.location
  version                      = "12.0"
  administrator_login          = var.sql_login
  administrator_login_password = var.sql_password
}

resource "azurerm_mssql_database" "ihome" {
  name         = "ihome"
  server_id    = azurerm_mssql_server.ihome-database.id
  collation    = "SQL_Latin1_General_CP1_CI_AS"
  license_type = "LicenseIncluded"
  max_size_gb  = 2
  sku_name     = "Basic"
}
