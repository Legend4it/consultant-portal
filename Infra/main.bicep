targetScope = 'subscription'
param rgName string = 'ConsultantPortalen-rg'
param location string = 'swedencentral'
param cosmosAccountName string = 'cosmosdbaccount20250425'
param databaseName string = 'ConsultantDB'


resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
    name: rgName
    location: location
  }
  
  module cosmosModule 'cosmos.bicep' = {
    name: 'cosmosDeployment'
    scope: resourceGroup(rg.name)
    params: {
      cosmosAccountName: cosmosAccountName
      databaseName: databaseName
      location: location
    }
  }
