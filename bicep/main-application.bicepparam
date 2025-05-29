using './main-application.bicep'
param appName  = 'datasynchro'

param sqlserverAdminPassword  = '' // Replace with a secure password

param tags  = {
  ApplicationName: 'DataSynchro'
  Environment: 'Development'
  DeployedBy: 'Bicep'
}
