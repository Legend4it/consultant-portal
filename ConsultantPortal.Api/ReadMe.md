### To Add new Secret
`dotnet user-secrets set`
### To Remove secret
`dotnet user-secret remove`
### To add Section to secret
`dotnet user-secret set "sectionname:keyname" "value"`
### To Deploy Bicep template
`az deployment sub create --location swedencentral  --template-file main.bicep --query properties.outputs`