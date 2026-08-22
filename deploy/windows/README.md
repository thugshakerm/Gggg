# Windows VPS deployment (IIS)

This deployment setup assumes one Windows VPS, IIS, PostgreSQL on the same VPS, and the `Thugbium` IIS application pool.

## One-time install

Run Command Prompt or PowerShell as Administrator:

```bat
mkdir C:\Thugbium
git clone --branch arena/01a02820-gggg --single-branch https://github.com/thugshakerm/Gggg.git C:\Thugbium\Gggg
```

Install these first:

- Git for Windows
- .NET 10 Hosting Bundle
- PostgreSQL
- IIS with the ASP.NET Core Module from the Hosting Bundle

Create the application publish directory:

```bat
mkdir C:\inetpub\thugbium
```

Create an IIS application pool named `Thugbium`, configure it as **No Managed Code**, and point the IIS site/application physical path at:

```text
C:\inetpub\thugbium
```

The first publish can be run from the repository root:

```bat
dotnet publish src\Thugbium.Web\Thugbium.Web.csproj -c Release -o C:\inetpub\thugbium
```

Use a server-local `appsettings.Production.json` or environment variables for real PostgreSQL and Discord values. Do not commit production passwords, Discord client secrets, or ASP.NET data-protection keys.

## Updating later

Copy `deploy\windows\update-thugbium.bat` somewhere convenient, such as `C:\Thugbium\update-thugbium.bat`, set its paths/app-pool name if needed, then run it as Administrator.

It performs, in order:

1. `git fetch`
2. `git pull --ff-only origin arena/01a02820-gggg`
3. `dotnet restore`
4. `dotnet publish`
5. IIS application-pool recycle

The `--ff-only` option refuses to overwrite unexpected server-side commits. Keep server-only configuration outside the Git checkout.
