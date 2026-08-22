@echo off
setlocal EnableExtensions

REM Thugbium one-VPS updater. No IIS required.
REM Run from an elevated Command Prompt or PowerShell.
set "REPO=C:\Thugbium\site"
set "PUBLISH=C:\inetpub\thugbium"
set "BRANCH=arena/01a02820-gggg"
set "PROJECT=src\Thugbium.Web\Thugbium.Web.csproj"
set "PORT=8091"
set "PID_FILE=%PUBLISH%\thugbium.pid"

echo.
echo === Pulling Thugbium updates ===
if not exist "%REPO%\.git" (
    echo ERROR: Repository was not found at %REPO%
    exit /b 1
)

cd /d "%REPO%" || exit /b 1
git fetch origin %BRANCH% || goto :failed
git checkout %BRANCH% || goto :failed
git pull --ff-only origin %BRANCH% || goto :failed

echo.
echo === Stopping the current Thugbium process ===
powershell -NoProfile -ExecutionPolicy Bypass -Command "$pidFile='%PID_FILE%'; if (Test-Path $pidFile) { $id=(Get-Content $pidFile -ErrorAction SilentlyContinue | Select-Object -First 1); if ($id) { Stop-Process -Id $id -Force -ErrorAction SilentlyContinue }; Remove-Item $pidFile -Force -ErrorAction SilentlyContinue }"

echo.
echo === Publishing ASP.NET Core application ===
dotnet restore "%PROJECT%" || goto :failed
dotnet publish "%PROJECT%" --configuration Release --no-restore --output "%PUBLISH%" || goto :failed

echo.
echo === Starting Thugbium on port %PORT% ===
powershell -NoProfile -ExecutionPolicy Bypass -Command "$publish='%PUBLISH%'; $pidFile='%PID_FILE%'; $process=Start-Process -FilePath 'dotnet' -ArgumentList ('Thugbium.Web.dll --urls http://0.0.0.0:%PORT%') -WorkingDirectory $publish -WindowStyle Hidden -PassThru; $process.Id | Set-Content $pidFile"

if errorlevel 1 goto :failed

echo.
echo Thugbium was updated and restarted successfully.
echo Local address: http://localhost:%PORT%
exit /b 0

:failed
echo.
echo UPDATE FAILED. Check the output above.
exit /b 1
