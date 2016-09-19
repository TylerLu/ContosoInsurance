param($StorageAccountName, $StorageAccountKey)

$ContainerName = "vehicle-images"
$PSScriptRoot = (Split-Path -Parent -Path $MyInvocation.MyCommand.Definition) + "\"

$ctx = New-AzureStorageContext -StorageAccountName $StorageAccountName -StorageAccountKey $StorageAccountKey

$images = "vehicle-983cbb99-44c7-4ddd-800c-aa3360f217d4", "vehicle-db11eaa3-58a2-453d-ac9b-aea8fefbcffa", `
		  "vehicle-032c600e-bea9-46e3-ab4a-ec399a3ff0b1", "vehicle-dc716f63-f99b-45f4-87e3-4296e812845c"

foreach($image in $images){
    $localFile = $PSScriptRoot + $image 
    Set-AzureStorageBlobContent -File $localFile -Container $ContainerName -Blob $image -Context $ctx
}