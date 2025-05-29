using './main-infrastructure.bicep'
param appName  = 'datasynchro'

param adminUserObjectId  = '00000000-0000-0000-0000-000000000000' // Replace with actual Object ID of the admin user

param sqlserverAdminPassword  = '' // Replace with a secure password

param tags  = {
  ApplicationName: 'DataSynchro'
  Environment: 'Development'
  DeployedBy: 'Bicep'
}
