# Windows VPS deployment (direct ASP.NET Core)

Thugbium does not need IIS. The site runs directly with ASP.NET Core/Kestrel on port `8091`. Add Caddy later when a public Thugbium subdomain and HTTPS are ready.

The sparse checkout contains only:

```text
src/
deploy/
```

## One-time clone

```bat
mkdir C:\Thugbium
git clone --branch arena/01a02820-gggg --single-branch --filter=blob:none --no-checkout https://github.com/thugshakerm/Gggg.git C:\Thugbium\site
cd /d C:\Thugbium\site
git sparse-checkout init --cone
git sparse-checkout set src deploy
git checkout
```

Install Git for Windows, the .NET 10 SDK, and PostgreSQL. Configure the database and `appsettings.Production.json` as described in `src\Thugbium.Web\README.md`.

## Update and restart

Run this from an elevated Command Prompt or PowerShell:

```bat
C:\Thugbium\site\deploy\windows\update-thugbium.bat
```

The script:

1. Fetches and pulls `arena/01a02820-gggg` with `--ff-only`.
2. Stops the exact Thugbium process recorded in `C:\inetpub\thugbium\thugbium.pid`.
3. Restores and publishes the Razor Pages application.
4. Starts it in the background on port `8091`.
5. Writes the new process ID to the PID file for the next update.

Open the local test site at `http://localhost:8091`.

## First run

The updater can perform the first publish/start too. Run it after the database configuration exists:

```bat
C:\Thugbium\site\deploy\windows\update-thugbium.bat
```

Do not run a second manual `dotnet Thugbium.Web.dll` process at the same time. The updater owns the process on port `8091`.
