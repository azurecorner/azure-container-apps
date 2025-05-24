# azure-container-apps

Azure Container Apps is a serverless platform that simplifies the deployment of containerized applications. It helps reduce infrastructure management and costs by eliminating the need to handle server configuration, container orchestration, and deployment details. Container Apps automatically provides the necessary, up-to-date server resources to ensure your applications remain stable and secure.

To learn more, visit the official documentation: [Azure Container Apps Overview](https://learn.microsoft.com/en-us/azure/container-apps/overview)

## Tutorial Overview

In this tutorial, we’ll guide you through the process of getting started with Azure Container Apps. We’ll create a **public-facing frontend application** that connects to a **private web app**, which in turn **queries or inserts data into a SQL Server database**. The entire setup will be **fully automated using Azure Bicep**.

## Deploy Infrastructure

This guide explains how to deploy Azure infrastructure for Container Apps using PowerShell and Bicep.

---

## Prerequisites

- Azure CLI and Azure PowerShell must be installed.
- You must be signed in to your Azure account (`az login`).
- Bicep templates (`main.bicep` and `main.bicepparam`) should be available in the `bicep/` folder.

---

## Step 1: Set the Azure Subscription

Retrieve the current subscription ID and set it as the active subscription:

```powershell
$subscriptionId = (Get-AzContext).Subscription.Id
az account set --subscription $subscriptionId

```


# ###########################################
$resourceGroupName="RG-CONTAINER-APPS"

New-AzResourceGroup -Name $resourceGroupName -Location "francecentral" 
 
New-AzResourceGroupDeployment -Name "container-apps-001" -ResourceGroupName $resourceGroupName -TemplateFile bicep/main.bicep -TemplateParameterFile bicep/main.bicepparam -DeploymentDebugLogLevel All

# build and deploy apps to container registry
$acrName="acrdatasynchro"
az acr login --name $acrName

docker build -t "$acrName.azurecr.io/weatherforecast-web-api:latest" .\src\WeatherForecastApp -f .\src\WeatherForecastApp\WeatherForecast.WebApi\Dockerfile --no-cache

docker push "$acrName.azurecr.io/weatherforecast-web-api:latest"

docker build -t "$acrName.azurecr.io/weatherforecast-web-app:latest" .\src\WeatherForecastApp -f .\src\WeatherForecastApp\WeatherForecast.WebApp\Dockerfile --no-cache

docker push "$acrName.azurecr.io/weatherforecast-web-app:latest"

# deploy container apps

New-AzResourceGroupDeployment -Name "container-apps-001" -ResourceGroupName $resourceGroupName -TemplateFile bicep/main.bicep -TemplateParameterFile bicep/main.bicepparam -deployApps $true -DeploymentDebugLogLevel All

# test

# create sql server tables

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



# see logs

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


