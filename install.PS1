New-Service -Name Commander -DisplayName Commander -Description Commander -BinaryPathName $PWD\Commander.exe -StartupType Automatic

if ($?) {"Setup successfully."}

