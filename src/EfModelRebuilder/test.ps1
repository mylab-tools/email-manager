docker-compose up -d

$connectionString = "Server=127.0.0.1;Port=3306;Database=db;Uid=user;Pwd=password;"
$outputDir = "../MyLab.EmailManager.Infrastructure/EfModels"
$dataContextName = "ReadDbContext"

Write-Output "Waiting for MySQL..."

$tableList = @()

do
{
	Start-Sleep -Seconds 2
	Write-Output "`ttry to ping ..."
	$tableList = docker exec ef-model-builder-db mysql -e "SELECT table_name FROM information_schema.tables WHERE table_schema = 'db';" -s 2> $nul
}while(!$?)
Write-Output "MySQL is ready now!"

dotnet ef database update `
	--project ../Migrations/Migrations.csproj `
	--startup-project ../Migrations/Migrations.csproj `
	--connection $connectionString

$scaffoldInitialArgs ="ef dbcontext scaffold `"$($connectionString)`" Pomelo.EntityFrameworkCore.MySql --no-build --force --output-dir $($outputDir) --data-annotations --context $($dataContextName) --namespace MyLab.EmailManager.Infrastructure.EfModels --no-onconfiguring"

$tableListArgs = ($tableList | where { $_ -ne '__EFMigrationsHistory' } | % { "--table " + $_ }) -join ' '

"Call: dotnet $scaffoldInitialArgs $tableListArgs"

Start-Process -NoNewWindow -FilePath "dotnet" -ArgumentList "$scaffoldInitialArgs $tableListArgs"