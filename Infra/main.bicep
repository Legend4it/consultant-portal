targetScope = 'subscription'
param rgName string = 'ConsultantPortalen-rg'
param location string = 'swedencentral'
param cosmosAccountName string = 'cosmosdbaccount20250425'
param databaseName string = 'ConsultantDB'
param openAIAccountName string = 'openaiaccount20250425'

resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
    name: rgName
    location: location
  }
  
  // Deploy Cosmos DB via nested module
  module cosmosModule 'cosmos.bicep' = {
    name: 'cosmosDeployment'
    scope: resourceGroup(rg.name)
    params: {
      cosmosAccountName: cosmosAccountName
      databaseName: databaseName
      location: location
    }
  }


  // Deploy Azure OpenAI via nested module
  module openAIModule 'openai.bicep' = {
    name: 'openAIDeployment'
    scope: resourceGroup(rg.name)
    params: {
      openAIAccountName: openAIAccountName
      location: location
    }
    // dependsOn: [cosmosModule]
  }
