Write-Host "🚀 Starting FlowTracker infrastructure…" -ForegroundColor Cyan

# Obtener la ruta raíz del proyecto
$rootPath = Split-Path -Parent $PSScriptRoot

# Levantar la base de datos usando el compose de la raíz
docker compose -f "$rootPath\docker-compose.yml" up -d

Write-Host "⏳ Waiting for SQL Server to initialize..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

Write-Host "✅ Database should now be available at localhost,1433" -ForegroundColor Green
Write-Host "💡 Use 'docker compose down' to stop everything." -ForegroundColor Yellow
