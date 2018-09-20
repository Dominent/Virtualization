[string]$CONFIGURATION = "Debug";
[string]$PLATFORM = "x86";
[string]$PROJECT_EXTENSION = ".csproj";
[string]$ARCHIVE_EXTENSION = ".zip";
[string]$OUTPUT_PATH = "bin\Debug\";
[string]$COMMANDS_DIRECTORY_NAME = "Commands";

function GetCommandsPath() {
	# This is the main build folder - project/bin/debug
	[string]$buildFolderPath = [System.IO.Directory]::GetCurrentDirectory();

	[System.IO.DirectoryInfo]$buildFolderDirectory = [System.IO.DirectoryInfo]::new( $buildFolderPath );

	[string]$solutionPath = $buildFolderDirectory.Parent.Parent.Parent.FullName;

	[string]$commandsPath = [System.IO.Path]::Combine($solutionPath, $COMMANDS_DIRECTORY_NAME);

	return $commandsPath;
}

function GetMsBuildPath() {
	[string]$runtimeDirectory = [System.Runtime.InteropServices.RuntimeEnvironment]::GetRuntimeDirectory();

	[string]$msBuildPath = [System.IO.Path]::Combine($runtimeDirectory, "MSBuild.exe");

	return $msBuildPath;
}

[string]$msBuildPath = GetMSBuildPath;

[string]$commandsPath = GetCommandsPath;

$commands = [System.IO.Directory]::EnumerateDirectories( $commandsPath );

[string]$currentDirectory = [System.IO.Directory]::GetCurrentDirectory();
[string]$destinationCommandsPath = [System.IO.Path]::Combine($currentDirectory, $COMMANDS_DIRECTORY_NAME)

if(!(Test-Path $destinationCommandsPath))
{
	New-Item -ItemType directory -Path $destinationCommandsPath  | Out-Null
}

# Clean destination commands directory
Remove-Item "$destinationCommandsPath\*" -Recurse -Force | Out-Null

foreach($command in $commands){
	[string]$project = [System.IO.DirectoryInfo]::new($command).Name;
	[string]$projectName = $project + $PROJECT_EXTENSION;

	Write-Output "Building $project"
	
    # Build project
	[string]$projectPath = [System.IO.Path]::Combine($command, $projectName);
	try {
		& $msBuildPath ('{0}' -f $projectPath) | Out-Null

		Write-Output "Build Succeeded";
	} catch {
		Write-Error "Build Error: " + $_.exception.message;
	}

	# Create temporary directory
	[string]$tmpProjectPath = [System.IO.Path]::Combine($env:TEMP, $project);
	if(Test-Path $tmpProjectPath)
	{
		Remove-Item $tmpProjectPath -Recurse -Force;
	}

	New-Item -ItemType directory -Path $tmpProjectPath  | Out-Null

	# Copy files to temporary directory
	[string]$buildPath = [System.IO.Path]::Combine( $command, "bin", $CONFIGURATION);
	Copy-Item "$buildPath\*" $tmpProjectPath -Recurse 

	 # Zip project
	[string]$zipProject = ($project + $ARCHIVE_EXTENSION);
	[string]$destinationPath = [System.IO.Path]::Combine($destinationCommandsPath,  $zipProject);

	Write-Output "Archiving $zipProject"

    Compress-Archive -Path $tmpProjectPath -DestinationPath $destinationPath -Update;
}