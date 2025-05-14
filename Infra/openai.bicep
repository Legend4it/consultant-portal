// openai.bicep

targetScope = 'resourceGroup'

param openAIAccountName string
param location string


resource openAIAccount 'Microsoft.CognitiveServices/accounts@2025-04-01-preview' = {
  name: '${openAIAccountName}-deployment'
  location: location
  kind: 'OpenAI'
  tags:{ Environment: 'dev'}
  sku: {
    name: 'S0'
  }
  properties: {
    publicNetworkAccess: 'Enabled'
    customSubDomainName: '${openAIAccountName}-deployment'
  }
}

output openAIEndpoint string = openAIAccount.properties.endpoint
// output openAIKey string      = listKeys(openAIAccount.id,'2024-10-01').key1
