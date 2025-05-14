targetScope = 'resourceGroup'

param cosmosAccountName string
param databaseName string
param location string

resource cosmosAccount 'Microsoft.DocumentDB/databaseAccounts@2023-04-15' = {
  name: cosmosAccountName
  location: location
  tags:{ Environment: 'dev'}
  kind: 'GlobalDocumentDB'
  properties: {
    databaseAccountOfferType: 'Standard'
    locations: [
      {
        locationName: location
        failoverPriority: 0
        isZoneRedundant: false
      }
    ]
    consistencyPolicy: {
      defaultConsistencyLevel: 'Session'
    }
  }
}

resource database 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2023-04-15' = {
  name: databaseName
  parent: cosmosAccount
  properties: {
    resource: {
      id: databaseName
    }
    options: {}
  }
}


output cosmosAccountEndpoint string = cosmosAccount.properties.documentEndpoint
// output cosmosAccountKey      string = listKeys(cosmosAccount.id, '2021-04-15').primaryMasterKey
