[CmdletBinding()]
param
(
	[Parameter(Position = 1, Mandatory = $true)]
	[string] $SolutionDir
)

Function Compress-CssFile
{
    [CmdletBinding()]
    param 
    (
        [string] $CssFilePath
    )

    $cssFile = Get-Item -Path $CssFilePath
    $content = [System.IO.File]::ReadAllText($cssFile.FullName)
    $body = @{input = $content}
    $response = Invoke-WebRequest -Uri "http://cssminifier.com/raw" -Method Post -Body $body
    
    if ($response.StatusCode -ne 200)
    {
        throw ("Could not compress CSS file $CssFilePath" +
           [System.Environment]::NewLine +
           [System.Environment]::NewLine +
           "Response:" + 
           [System.Environment]::NewLine +
           $response.RawContent)
    }

    $compressedContent = $response.Content
    $newFilePath = $CssFilePath.Replace(".css", ".min.css")

    Set-Content -Path $newFilePath -Value $compressedContent -Force
}

# -----------------
# BEGIN:

$ErrorActionPreference = "Stop"

Write-Output "-----"
Write-Output "Compressing CSS Files"
Write-Output "-----"

Get-ChildItem $SolutionDir -Recurse -Include *.css -Exclude *.min.css | % {
	Write-Output "Compressing $_"
	Compress-CssFile -CssFilePath $_ 
}