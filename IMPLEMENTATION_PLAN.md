# Thugbium: what to do first

## Research basis

This plan is based on the historical material indexed by [Daniel-176/Roblox-Reverse-Engineering-Wiki](https://github.com/Daniel-176/Roblox-Reverse-Engineering-Wiki), especially:

- `articles/client/windows/2018.md`
- `articles/infrastructure/network/playerbeta-2018-subdomains.md`
- `articles/infrastructure/api/joinscript.md`
- `articles/infrastructure/api/games-relay.md`
- `articles/infrastructure/network/client-tickets.md`
- `articles/knowledge/vulnerabilities.md`

The wiki is a collection of community research, not a secure production foundation. Its own research notes warn that preserved ORC guides may be incomplete or unreliable.

## First decision: one target build, local only

Before writing a real website, choose **one exact 2018 Windows client build** and record its version/date/hash in a private development note. Do not try to support 2016, 2017, 2018, mobile, and Studio all at once.

For initial research, a 2018M Windows build is the most sensible scope because the wiki identifies it separately from later VM-protected 2018+ builds and documents its historical service references. That does **not** mean distributing, modifying, or exposing an old Roblox executable publicly.

The first target is a private local development environment with test accounts and no public users.

## First implementation milestone: `join one private test place`

Do not begin with a catalog, economy, forums, groups, trades, or every website page. The first end-to-end milestone should be:

> A locally authenticated test user can choose one private place on the Thugbium site, receive a short-lived join authorization, and reach one isolated test game server.

If this is working, the project has proved the core connection between website, account, game launch, and server. Everything else can be added around it.

### Deliverable checklist

1. **Thugbium web shell**
   - Keep the Bootstrap prototypes as reference only.
   - Add a minimal real app with Home, Register, Login, and a single Place page.
   - Use original Thugbium branding/assets before any public launch.

2. **Accounts and Discord linking**
   - PostgreSQL users table.
   - Password hashing using Argon2id or bcrypt.
   - Secure server-side sessions; `HttpOnly`, `Secure`, and `SameSite` cookies.
   - Discord OAuth linking design; the current button is UI-only until the callback, CSRF/state validation, account-link uniqueness, and staff controls exist.
   - No economy, password-reset shortcut, or moderator privileges in the first version.

3. **One place record**
   - `places`: id, owner, name, description, visibility, current_version.
   - `place_versions`: immutable file/version metadata and content hash.
   - One pre-approved local test place. Do not permit public uploads yet.

4. **Join service contract**
   - Website checks the signed-in user can join the requested place.
   - Website creates a random, one-time join record with user id, place id, target instance, expiry, and consumed timestamp.
   - Game-server gateway consumes it exactly once.
   - Keep the authorization server-side. Historical ticket formats described in the wiki use legacy SHA-1/RSA mechanisms; use modern authenticated, short-lived tokens instead.

5. **One isolated game-server process**
   - Start a single server instance on a private network/port.
   - Run it as an unprivileged process/container with CPU and memory limits.
   - Ensure scripts/content cannot access the host filesystem, shell, or unrestricted network.
   - Log joins, disconnects, crashes, and authorization failures without exposing player IPs.

6. **Minimal operations controls**
   - Rate limit registration, login, and join creation.
   - Audit log authentication, Discord link attempts, place publish events, and staff actions.
   - Add a simple admin-only test-user/place screen rather than a full control panel.
   - Keep database backups from day one.

## Why this comes before the other features

The wiki shows that historical clients depend on an ecosystem of site services: account/auth context, place authorization, a launch/join payload, asset delivery, and a game endpoint. Building only a catalog or visual site proves none of that.

Conversely, trying to reproduce every legacy API endpoint first creates a huge, insecure compatibility surface. Start with the exact calls your selected test build genuinely needs, document them, and return controlled responses for the rest.

## Explicitly defer

- Public registration and public client downloads
- Catalog purchases, Robux/Tix equivalents, trading, and cash-out
- User-uploaded models/scripts/audio/images
- Friends, DMs, groups, forums, feeds, and notifications
- Multiple client eras, mobile, and Studio hosting
- RCC render fleets and automated thumbnails
- Matching every legacy Roblox URL or web API

## Immediate next coding task

Create a small backend skeleton with a database migration for `users`, `discord_links`, `places`, `place_versions`, and `join_authorizations`, plus a Bootstrap-backed Thugbium Register/Login/Place UI. The initial Place page needs one button: **Join Test Place**.

When that app is ready, connect the Discord button to a real OAuth flow and build the single-use join service. Only then begin client-specific compatibility work in the private lab.
