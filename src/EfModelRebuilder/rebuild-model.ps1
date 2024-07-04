docker-compose up -d

$connectionString = "Server=127.0.0.1;Port=3306;Database=db;Uid=user;Pwd=password;"
$outputDir = "../MyLab.EmailManager.Infrastructure/EfModels"
$dataContextName = "ReadDbContext"

Write-Output "Waiting for MySQL..."

do
{
	Start-Sleep -Seconds 2
	Write-Output "`ttry to ping ..."
	docker exec ef-model-builder-db mysql -e "SELECT VERSION()" > $null 2> $nul
}while(!$?)
Write-Output "MySQL is ready now!"

dotnet ef database update `
	--project ../Migrations/Migrations.csproj `
	--startup-project ../Migrations/Migrations.csproj `
	--connection $connectionString

$scaffoldInitialArgs ="ef dbcontext scaffold `"$($connectionString)`" Pomelo.EntityFrameworkCore.MySql --no-build --force --output-dir $($outputDir) --data-annotations --context $($dataContextName) --namespace MyLab.EmailManager.Infrastructure.EfModels --no-onconfiguring"

$tableList = docker exec ef-model-builder-db mysql -e "SELECT table_name FROM information_schema.tables WHERE table_schema = 'db';" -s `
	| where { $_ -ne '__EFMigrationsHistory' }
$tableListArgs = ($tableList | % { "--table " + $_ }) -join ' '

"Tables: $tableList"
"Call: dotnet $scaffoldInitialArgs $tableListArgs"

Start-Process -Wait -NoNewWindow -FilePath "dotnet" -ArgumentList "$scaffoldInitialArgs $tableListArgs"

docker-compose down `
	--volumes

Get-ChildItem $outputDir -Filter *.cs | 
Foreach-Object {
    
	$skipRenaming = $_.FullName -match '\\' + $dataContextName + '.cs$'

	$initialContent = Get-Content -path $_.FullName
	$resultContent = $initialContent
	$outputFilename = $_.FullName 

	if(!$skipRenaming)
	{
		$resultContent = $initialContent | % { $_ -Replace 'public partial class (\w+)$', 'public partial class Db$1' }
		$outputFilename = $_.FullName -Replace '\\(\w+.cs)$', '\Db$1'
		Remove-Item $_.FullName
	}

	$resultContent = $resultContent `
							| % { $_ -Replace 'public virtual DbSet<EfmigrationsHistory> EfmigrationsHistories { get; set; }', '// EfmigrationsHistories link removed!' }

	foreach($table in $tableList)
	{
		$resultContent = $resultContent `
							| % { $_ -Replace "DbSet<($table)>", 'DbSet<Db$1>' } `
							| % { $_ -Replace "Collection<($table)>", 'Collection<Db$1>' } `
							| % { $_ -Replace "List<($table)>", 'List<Db$1>' } `
							| % { $_ -Replace "\s($table\??\s)", ' Db$1' } `
							| % { $_ -Replace "Entity<($table)>", 'Entity<Db$1>' }
	}
						
	$resultContent | Out-File $outputFilename
}