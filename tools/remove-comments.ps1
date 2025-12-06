# PowerShell script to remove comments from code files in workspace
# Processes .cs, .cshtml, .js, .css, .html files

$extensions = @('*.cs','*.cshtml','*.js','*.css','*.html')
$root = Resolve-Path .
Write-Host "Removing comments in: $root"

foreach ($ext in $extensions) {
    Get-ChildItem -Path $root -Recurse -Filter $ext -File | ForEach-Object {
        $file = $_.FullName
        $text = Get-Content -Raw -Encoding UTF8 $file

        # remove C-style block comments /* ... */
        $text = [regex]::Replace($text, '/\*.*?\*/', '', 'Singleline')

        # remove C# single-line comments // but not URLs (http://)
        $text = [regex]::Replace($text, '(?<!:)//.*$', '', 'Multiline')

        # remove Razor comments @* *@
        $text = [regex]::Replace($text, '@\*.*?\*@', '', 'Singleline')

        # remove HTML comments <!-- ... -->
        $text = [regex]::Replace($text, '<!--.*?-->', '', 'Singleline')

        # Trim trailing whitespace from lines
        $lines = $text -split "\r?\n"
        $lines = $lines | ForEach-Object { $_.TrimEnd() }
        $newText = ($lines -join "`r`n").TrimStart()`n

        if ($newText -ne $text) {
            Write-Host "Updating: $file"
            Set-Content -Path $file -Value $newText -Encoding UTF8
        }
    }
}
Write-Host "Done."