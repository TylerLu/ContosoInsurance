param($StorageAccountName, $StorageAccountKey)

$ContainerName = "vehicle-images"
$PSScriptRoot = (Split-Path -Parent -Path $MyInvocation.MyCommand.Definition) + "\"

$ctx = New-AzureStorageContext -StorageAccountName $StorageAccountName -StorageAccountKey $StorageAccountKey

New-AzureStorageContainer -Name "claim-images" -Permission Off -Context $ctx
New-AzureStorageContainer -Name "other-party-card-images" -Permission Off -Context $ctx
New-AzureStorageContainer -Name "other-party-license-images" -Permission Off -Context $ctx
New-AzureStorageContainer -Name "other-party-plate-images" -Permission Off -Context $ctx
New-AzureStorageContainer -Name "vehicle-images" -Permission Off -Context $ctx

New-AzureStorageQueue -Name "mobile-claims" -Context $ctx
New-AzureStorageQueue -Name "new-claims" -Context $ctx