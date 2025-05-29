@description('Main Bicep file for deploying Azure resources for the DataSync application.')
param location string = resourceGroup().location

@description('The name of the resource group that will be used to deploy the resources.')
param appName string 

param keyVaultName string = 'kv${appName}'

param backendApiImage string = 'acrdatasynchro.azurecr.io/weatherforecast-web-api:latest'
param frontendUIImage string = 'acrdatasynchro.azurecr.io/weatherforecast-web-app:latest'

param containerRegistryName string = 'acr${appName}'

param containerEnvironmentName string = 'env${appName}'

param sqlserverName string = 'sqlserver-${appName}'

param sqlserverAdminLogin string = 'logcorner'

@secure()
param sqlserverAdminPassword string 

param databaseName string = 'WeatherForecastDb'
param userAssignedIdentityName string = 'uami-${appName}'

param tags object 

param runScript string = loadTextContent('./scrips/run.ps1')
var createTablesScriptRaw = loadTextContent('./scrips/createTables.sql')
var createTablesScriptBase64 = base64(createTablesScriptRaw)

module backend 'modules/backend-api.bicep' =  {
  name: 'web-api'
  params: {
    containerAppEnvName: containerEnvironmentName
    containerRegistryName: containerRegistryName
    keyVaultName: keyVaultName
    location: location
    tags: tags
    imageName: backendApiImage
    userAssignedIdentityName: userAssignedIdentityName
  }
  dependsOn: [

    deploymentScript
  ]
}


module frontend 'modules/frontend-ui.bicep' =  {
  name: 'web-app'
  params: {
    containerAppEnvName: containerEnvironmentName
    containerRegistryName: containerRegistryName
    keyVaultName: keyVaultName
    location: location
    userAssignedIdentityName: userAssignedIdentityName
    tags: tags
    imageName: frontendUIImage
    backendFqdn: backend.outputs.fqdn
  }
  dependsOn: [
    deploymentScript
  ]
}


module deploymentScript 'modules/deployment-script.bicep' =  {
  name: 'deployment-script'
  params: {
    location: location
    sqlServerName: '${sqlserverName}.database.windows.net'
    databaseName: databaseName
    sqlAdminUsername: sqlserverAdminLogin
    sqlAdminPassword: sqlserverAdminPassword
    runScript: runScript
    createTablesScriptBase64: createTablesScriptBase64
  }
  
}
