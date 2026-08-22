@echo off
setlocal EnableExtensions

REM Run this file as Administrator when using IIS.
REM Change these values once for your VPS.
set "REPO=C:\Thugbium\Gggg"
set "PUBLISH=C:\inetpub\thugbium"
set "APP_POOL=Thugbium"
set "BRANCH=arena/01a02820-gggg"
set "PROJECT=src\Thugbium.Web\Thugbium.Web.csproj"

echo.
echo === Updating Thugbium from %BRANCH% ===
if not exist "%REPO%\.git" (
    echo ERROR: Repository was not found at %REPO%
    exit /b 1
)

cd /d "%REPO%" || exit /b 1
git fetch origin %BRANCH% || goto :failed
git checkout %BRANCH% || goto :failed
git pull --ff-only origin %BRANCH% || goto :failed

echo.
echo === Stopping IIS application pool ===
%windir%\System32\inetsrv\appcmd stop apppool /apppool.name:"%APP_POOL%" || goto :failed

echo.
echo === Publishing ASP.NET Core application ===
dotnet restore "%PROJECT%" || goto :restart_failed
dotnet publish "%PROJECT%" --configuration Release --no-restore --output "%PUBLISH%" || goto :restart_failed

echo.
echo === Starting IIS application pool ===
%windir%\System32\inetsrv\appcmd start apppool /apppool.name:"%APP_POOL%" || goto :failed

echo.
echo Thugbium was updated and restarted successfully.
exit /b 0

:restart_failed
%windir%\System32\inetsrv\appcmd start apppool /apppool.name:"%APP_POOL%" >nul 2>&1
goto :failed

:failed
echo.
echo UPDATE FAILED. Check the output above; the previous site build was restarted when possible.
exit /b 1
