# Thugbium Web

Initial single-VPS website application for Thugbium.

## Stack

- C# / ASP.NET Core Razor Pages (.NET 10)
- Bootstrap 4.6
- PostgreSQL + Entity Framework Core
- Cookie-based site sessions
- Discord OAuth2 account linking

## Included pages

- `/` — landing page
- `/` — landing, registration, and login
- `/Account/Discord` — Discord-link entry point
- `/Account/Dashboard` — signed-in home/dashboard
- `/Account/Settings` — basic settings
- `/Catalog` — catalog placeholder
- `/Places` — place directory placeholder
- `/Users/{username}` — user profile

## Windows VPS setup

1. Install .NET 10 Hosting Bundle and PostgreSQL.
2. Create the `thugbium` PostgreSQL database and execute `sql/001_initial.sql` for the first development schema.
3. Copy `appsettings.json` values into environment variables or a server-local secrets file. Never commit production database passwords or Discord secrets.
4. Configure the Discord application callback as `https://YOUR-DOMAIN/Account/DiscordCallback`.
5. Run behind Caddy or IIS over HTTPS. PostgreSQL should bind only to `127.0.0.1`, never a public network interface.

## Before production

- Replace the temporary navbar image with the real Thugbium wordmark.
- Replace the raw schema file with reviewed EF Core migrations.
- Configure persisted ASP.NET data-protection keys.
- Add CSRF-aware Discord OAuth state monitoring, rate limiting, logging, backups, password reset, moderation, and terms/privacy pages.
- Do not enable public client downloads or place uploads yet.
