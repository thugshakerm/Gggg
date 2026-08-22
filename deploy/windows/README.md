# Windows VPS deployment (IIS)

This setup uses a **sparse Git checkout**. The VPS clones only the production folders it needs:

```text
src/
deploy/
```

It does not check out the root research notes or any development-only files.

## One-time install

Run Command Prompt or PowerShell as Administrator:

```bat
mkdir C:\Thugbium
git clone --branch arena/01a02820-gggg --single-branch --filter=blob:none --no-checkout https://github.com/thugshakerm/Gggg.git C:\Thugbium\site
cd /d C:\Thugbium\site
git sparse-checkout init --cone
git sparse-checkout set src deploy
git checkout
```

The local folder is intentionally named:

```text
C:\Thugbium\site
```

The GitHub repository name remains `Gggg`, but that name does not need to appear in your VPS deployment path.

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

The first publish can be run from the sparse checkout:

```bat
dotnet publish src\Thugbium.Web\Thugbium.Web.csproj -c Release -o C:\inetpub\thugbium
```

Use a server-local `appsettings.Production.json` or environment variables for real PostgreSQL and Discord values. Do not commit production passwords, Discord client secrets, or ASP.NET data-protection keys.

## Updating later

Run this as Administrator:

```bat
C:\Thugbium\site\deploy\windows\update-thugbium.bat
```

It performs, in order:

1. `git fetch`
2. `git pull --ff-only origin arena/01a02820-gggg`
3. `dotnet restore`
4. Stops the IIS application pool
5. `dotnet publish`
6. Starts the IIS application pool

Sparse checkout remains active during updates, so the VPS continues to receive only `src/` and `deploy/` from the branch. The `--ff-only` option refuses to overwrite unexpected server-side commits.
