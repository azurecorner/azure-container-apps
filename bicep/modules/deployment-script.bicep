param location string
param sqlServerName string
param databaseName string
@secure()
param sqlAdminUsername string
@secure()
param sqlAdminPassword string
param runScript string
param createTablesScriptBase64 string

#disable-next-line BCP081
resource runSqlDeployment 'Microsoft.Resources/deploymentScripts@2023-08-01' = {
  name: 'run-sql-deployment'
  location: location
  kind: 'AzurePowerShell'
  properties: {
    azPowerShellVersion: '9.7'
    retentionInterval: 'PT1H'
    timeout: 'PT15M'
    forceUpdateTag: '1'
    scriptContent: runScript

    arguments: '-sqlServerName "${sqlServerName}" -databaseName "${databaseName}" -sqlAdminUsername "${sqlAdminUsername}" -sqlAdminPassword "${sqlAdminPassword}" -sqlScriptBase64 "${createTablesScriptBase64}"'
  }
}

output scriptStatus string = runSqlDeployment.properties.provisioningState
