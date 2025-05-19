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

CREATE TABLE [dbo].[Location] (
    [Id]                   INT            IDENTITY (1, 1) NOT NULL,
    [Department]           NVARCHAR (100) NULL,
    [DepartmentCode]       INT            NULL,
    [City]                 NVARCHAR (100) NULL,
    [PostalCode]           INT            NULL,
    [Latitude]             FLOAT (53)     NULL,
    [Longitude]            FLOAT (53)     NULL,
    [GenerationTimeMs]     FLOAT (53)     NULL,
    [UtcOffsetSeconds]     INT            NULL,
    [Timezone]             NVARCHAR (50)  NULL,
    [TimezoneAbbreviation] NVARCHAR (10)  NULL,
    [Elevation]            FLOAT (53)     NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


CREATE TABLE [dbo].[Weather] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [LocationId]    INT           NULL,
    [Time]          NVARCHAR (50) NULL,
    [Interval]      INT           NULL,
    [Temperature]   FLOAT (53)    NULL,
    [Windspeed]     FLOAT (53)    NULL,
    [Winddirection] INT           NULL,
    [IsDay]         INT           NULL,
    [Weathercode]   INT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Location] ([Id])
);



# 

az containerapp show --name dayasync-weatherforecast-api  --resource-group RG-CONTAINER-APPS




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


