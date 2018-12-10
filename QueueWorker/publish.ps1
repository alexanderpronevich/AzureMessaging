dotnet publish -c Release -o publish/content

Add-Type -AssemblyName System.IO.Compression.FileSystem

$webSite = "AzurePublishApi20181206122527"

$compressionLevel = [System.IO.Compression.CompressionLevel]::Optimal

$base = pwd
$path = Join-Path $base "publish/content"
$packageFile = Join-Path $base "publish/webJob.zip"

rm $packageFile

[System.IO.Compression.ZipFile]::CreateFromDirectory($path, $packageFile, $compressionLevel, $false)

New-AzureWebsiteJob -Name $webSite -JobName 'rabbitMqQueueWorkder' -JobType Continuous -JobFile $packageFile

