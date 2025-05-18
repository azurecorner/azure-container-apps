# azure-container-apps

# deploy

$subscriptionId= (Get-AzContext).Subscription.id 

az account set --subscription $subscriptionId 

$resourceGroupName="RG-CONTAINER-APPS"

New-AzResourceGroup -Name $resourceGroupName -Location "francecentral" 
 
New-AzResourceGroupDeployment -Name "container-apps-001" -ResourceGroupName $resourceGroupName -TemplateFile bicep/main.bicep -TemplateParameterFile bicep/main.bicepparam -DeploymentDebugLogLevel All

# 
$acrName="acrdatasynchro"
az acr login --name $acrName

docker build -t "$acrName.azurecr.io/weatherforecast-web-api:latest" .\src\WeatherForecastApp -f .\src\WeatherForecastApp\WeatherForecast.WebApi\Dockerfile --no-cache

docker push "$acrName.azurecr.io/weatherforecast-web-api:latest"

docker build -t "$acrName.azurecr.io/weatherforecast-web-app:latest" .\src\WeatherForecastApp -f .\src\WeatherForecastAppWeatherForecast.WebApp\Dockerfile --no-cache

docker push "$acrName.azurecr.io/weatherforecast-web-app:latest"

#

New-AzResourceGroupDeployment -Name "container-apps-001" -ResourceGroupName $resourceGroupName -TemplateFile bicep/main.bicep -TemplateParameterFile bicep/main.bicepparam -deployApps $true -DeploymentDebugLogLevel All

# test

az containerapp show --name dayasync-weatherforecast-api  --resource-group RG-CONTAINER-APPS


az containerapp logs show --name dayasync-weatherforecast-api  --resource-group RG-CONTAINER-APPS --follow

az containerapp show \
  --name dayasync-weatherforecast-api \
  --resource-group RG-CONTAINER-APPS \
  --query "properties.template.containers[0].image"


az containerapp revision list \
  --name dayasync-weatherforecast-api \
  --resource-group RG-CONTAINER-APPS \
  --query "[].{Name:name, State:properties.active, Reason:properties.healthState, Conditions:properties.conditions}" \
  --output json

 az containerapp show \
  --name dayasync-weatherforecast-api \
  --resource-group RG-CONTAINER-APPS \
  --query "properties.provisioningState"
"Failed"

