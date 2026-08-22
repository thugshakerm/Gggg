# 2018-era Roblox-inspired platform — research notes

**Source studied:** [Daniel-176/Roblox-Reverse-Engineering-Wiki](https://github.com/Daniel-176/Roblox-Reverse-Engineering-Wiki), retrieved 2026-08-22.

## What the repository is

This is a community-curated, PHP-rendered wiki collecting historical notes, archival resources, and ORC community guides. It contains three particularly useful categories:

1. **Historical architecture references** — legacy web API descriptions, join configuration, client tickets, networking, and RCCService notes.
2. **Era references** — Windows, mobile, and Studio material organized by year, including 2018/2018+.
3. **Historical assets/resources** — CoreScripts archive, old site-template archives, and links to other archival projects.

The repository itself explicitly says that many guides were preserved from the ORC guide server and may be incomplete or unreliable. Treat it as a research index, validate every claim, and do not treat it as a production implementation.

## Material relevant to a 2018-era platform

### 1. Service boundaries

The strongest useful finding is the separation of responsibilities across services. The wiki's legacy API and subdomain documentation groups functionality into domains such as:

- identity/authentication and account settings
- users, friends, presence, chat, and text moderation
- places/games, joins, publishing, and persistence
- avatar, inventory, catalog, thumbnails, badges, and economy
- asset delivery and temporary upload storage
- client configuration and crash reporting

That is a good model for a new platform, even if the endpoint names and payloads should not be copied exactly. Start with a modular monolith, then separate the asset, game-join, and realtime systems only when scale needs it.

### 2. Join flow

`articles/infrastructure/api/joinscript.md` identifies the information a legacy launch configuration carried: player identity, avatar/appearance, server endpoint, base URL, creator context, membership, and a client authentication ticket.

**Design takeaway:** have the website issue a short-lived, single-use join token after it authorizes a user to enter a specific game instance. The game server verifies that token and then loads the player's approved appearance and permissions from trusted backend services.

For a new implementation, use modern cryptography and strict expiry/audience checks (for example, an asymmetric JWT or PASETO plus server-side replay protection). Do not copy historical SHA-1 ticket formats or embedded client signing keys.

### 3. Multiplayer networking

`articles/infrastructure/network/raknet.md` documents that the historical stack used UDP-based RakNet with reliability features, and `network-server.md` describes a server/replicator relationship.

**Design takeaway:** a realtime game protocol should be separate from the website API and should use an authoritative game server. Clients can send input and requests, but servers must validate movement, combat, inventory, permissions, purchases, and game-state changes.

A modern revival-inspired project can use a maintained networking library/framework instead of reproducing historical protocol details.

### 4. Creator and content model

The legacy API documentation includes asset versioning, place ownership/management checks, badge awards, avatar fetches, catalog/inventory concepts, and thumbnail services.

**MVP data model:**

- `users`, `sessions`, `roles`, `bans`, `audit_events`
- `places`, `place_versions`, `place_permissions`, `game_instances`
- `assets`, `asset_versions`, `asset_reviews`, `asset_blobs`
- `avatars`, `avatar_items`, `inventories`
- `friendships`, `friend_requests`, `reports`

Assets and places should be immutable/versioned after publishing. Store content in object storage, save hashes and metadata in PostgreSQL, and use signed upload/download URLs.

### 5. Client settings and telemetry

The repository references client-settings/feature-flag delivery and crash-report endpoints.

**Design takeaway:** build controlled configuration from the start, but use it responsibly: signed configuration, per-version compatibility checks, privacy-conscious crash reporting, opt-in analytics where required, and no hidden collection of IP addresses or personal data.

## Security lessons from the archive

The historical vulnerability page is valuable precisely because old software had severe problems: unsafe scripting paths, crashable chat behavior, IP disclosure risks, malformed-content crashes, and signature-verification flaws.

Requirements for a public project:

- Do not expose player IP addresses to game scripts or other players.
- Apply message-length limits, rate limits, abuse detection, and server-side text filtering.
- Sandbox creator scripts: no process execution, filesystem access, unrestricted network access, or native-module loading.
- Enforce CPU, memory, object-count, recursion, and execution-time quotas for games and scripts.
- Validate all uploaded content; scan, size-limit, hash, and moderate it before publication.
- Use TLS, modern signatures/hashes, key rotation, secure session cookies, CSRF defenses, MFA for staff, and audit trails.
- Keep economy/inventory state server-authoritative and make purchase operations idempotent.
- Run each game server in an isolated process/container with minimal privileges.

## Material to avoid using in a public deployment

The repository includes binary-patching, trust-check/authentication bypass, SSL-verification bypass, and old-client signature material. These sections may be relevant as historical evidence, but they are not appropriate building blocks for a safe public platform.

Do not:

- redistribute or modify proprietary Roblox binaries, CoreScripts, art, audio, or site templates without clear permission;
- disable TLS, signature, authentication, or trust checks;
- reuse historical shared keys or obsolete SHA-1-based authentication;
- point software at Roblox-owned endpoints, impersonate Roblox, or collect credentials;
- use old client builds on the public internet.

Use original branding, original assets, and a new client/runtime or appropriately licensed technology.

## Recommended implementation sequence

1. Build an original small sandbox client with a block-building aesthetic.
2. Build account authentication, profiles, and an admin/audit foundation.
3. Create place save/load and versioned asset upload with moderation gates.
4. Implement one authoritative multiplayer game-server template.
5. Implement join authorization with short-lived, one-time tokens.
6. Add friends, presence, chat, reporting, and moderation tooling.
7. Add creator publishing, discovery, thumbnails, and avatar inventory.
8. Consider an economy only after security, moderation, and ownership are mature.

## Priority source files for continued study

- `articles/client/windows/2018.md` and `2018-plus.md` — era indexing only; do not use the patch instructions for production.
- `articles/infrastructure/api/api-docs.md` — legacy feature and payload reference.
- `articles/infrastructure/api/joinscript.md` — launch-configuration concepts.
- `articles/infrastructure/api/games-relay.md` — legacy game-service responsibilities.
- `articles/infrastructure/network/client-tickets.md` — historical ticket model; replace with modern auth.
- `articles/infrastructure/network/raknet.md` — historical network architecture overview.
- `articles/infrastructure/network/playerbeta-2018-subdomains.md` — historical service inventory.
- `articles/knowledge/vulnerabilities.md` — security regression checklist.
