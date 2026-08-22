# Revival-source study — architecture and lessons

**Research date:** 2026-08-22  
**Goal:** learn from publicly released revival codebases without copying unsafe, unmaintained, or proprietary implementation details into this project.

## Scope and sources examined

There is no trustworthy finite list of *every* revival. The ecosystem is large, frequently renamed, and many projects are private, defunct, missing source, or have unclear provenance. The public source/archives below are the useful, verifiable sample.

| Project/source | What was examined | Primary lesson |
|---|---|---|
| [Finobe.net / Aesthetiful](https://github.com/finobenet/finobenet) | Laravel 11 application, MySQL/Redis configuration, jobs, models, frontend templates, database export, static sprite/SVG/font directories | A cohesive modular-monolith site can own community, catalog, creation, moderation, and video processing without an immediate microservice split. |
| [Fobe / Alphaland](https://github.com/gtoriadotnet/fobe-web) | Legacy PHP site, page folders, API subdomain folders, asset/game routes, studio/client settings, administration, render tools and default places | A historical compatibility layer often becomes a very large collection of endpoint-specific code. Do not make this the public product architecture. |
| [Hexagon](https://github.com/suush-ii/hexagon-public) | SvelteKit/Bun frontend, TypeScript, Drizzle migrations, Postgres, Docker Compose, Caddy, monitoring, 2FA/auth dependencies, image/model conversion helpers | Modern developer tooling, migrations, containerization, observability, and typed services are a dramatically stronger foundation. Its own README says it should be treated as a learning resource, not copied into a new revival. |
| [Rovival / Economy Simulator](https://github.com/fxleons/rovival) | Node API, Go asset-validation service, .NET website/services/rendering, admin UI, Postgres/Redis, game-server component | The best architectural pattern observed: split asset validation and game services from the account/catalog web application, while retaining shared DTOs/models and explicit data ownership. |
| [Rainway](https://github.com/Flofy-Dev/rainway-source) | Laravel/PHP project structure and setup guidance | The maintainer explicitly describes it as old, unmaintained, and insecure. It is useful only as a historical reference—not as a dependency or production base. |
| [Graphictoria website source](https://github.com/JohanBLU/GraphictoriaWebsite) / [GT3 archive](https://archive.org/details/graphictoria-source-code) | Public warning from the source host plus historical archive metadata | Do not reuse it. The source host explicitly warns of vulnerabilities. |
| [Watrbx](https://github.com/WatrLabs/watrbx) and [Void](https://github.com/ethanm2502/void) | Public repository inventory and structure | They demonstrate broad endpoint coverage, but old flat endpoint trees and archival secrets/configuration risks are not acceptable production patterns. |
| [ORC revival list](https://orcrevivals.github.io/) and [Revival List resources](https://revival-list.com/Misc) | Current/historical project and source index | Useful discovery indexes only. Listings are not quality, security, license, or safety endorsements. |

### Fossci and Graphictoria

- **Fossci:** public search results point to a source-code archive rather than a maintained official Git repository. It is appropriate for historical comparison only; do not vendor a 500+ MB unverified archive into this repository.
- **Graphictoria:** historical code exists in archives, but its source host warns it is vulnerable. Use screenshots/route inventories for research, not implementation.

## What is worth adopting

### Product boundaries

The projects repeatedly converge on these product domains:

1. **Identity and account safety** — registration, sessions, email verification, password reset, roles, device/session controls.
2. **Social/moderation** — friends, messages, reports, bans, rate limits, audit events.
3. **Content** — assets, asset versions, place versions, permissions, thumbnails, reviews.
4. **Creator/game operations** — publish/place settings, game instances, join authorization, server health.
5. **Economy** — inventory, purchases, currency ledger, entitlements; this should be introduced last.
6. **Admin** — moderation queue, account actions, content review, feature flags, operational logs.

Keep these modules separate in code and database ownership even if the initial service deploys as one application.

### Recommended modern stack for this repository

```text
Web frontend:       TypeScript + React/Next.js or SvelteKit
API:                TypeScript (Fastify/NestJS) or ASP.NET Core
Primary database:   PostgreSQL with versioned migrations
Cache/jobs:         Redis plus a durable queue
Assets:             S3-compatible object storage (MinIO locally)
Game servers:       isolated, authoritative processes/containers
Observability:      structured logs, metrics, traces, backups, alerts
Deployment:         Docker Compose locally; managed containers later
```

This takes the good operational separation shown in Rovival/Hexagon without inheriting their historical endpoint formats.

### Build order informed by the study

1. Original branded browser frontend and design tokens.
2. Account/session system, user roles, rate limits, CSRF protection, audit log.
3. Postgres migrations and object storage for uploaded content.
4. Place/asset versioning plus an explicit moderation state machine.
5. Authoritative multiplayer server with short-lived, single-use join authorization.
6. Social and moderation tools.
7. Creator publishing/dashboard.
8. Economy only after an append-only transaction ledger, idempotent purchases, and staff controls exist.

## Patterns to reject

### Do not reuse old client-facing security schemes

Historical revivals commonly mirror old web API layouts, TLS/signature handling, session rules, and client assumptions. New code should use current HTTPS/TLS, modern password hashing, CSRF defenses, short-lived tokens, server-side authorization, and key rotation.

### Do not expose secrets or copy archived configuration

Several archival sources contain outdated configuration patterns and potentially sensitive material. Do not copy `.env` files, databases, key material, generated deployment files, or bundled credentials. Treat every downloaded archive as untrusted input.

### Do not make browser routes equal trusted authorization

Old projects often use many GET-style action endpoints or broad route handlers. Use explicit REST/typed actions, validate input server-side, require POST/PUT/DELETE for mutations, use authorization checks in every domain service, and protect state changes from replay.

### Do not run creator content with host privileges

Asset conversion, thumbnail rendering, scripts, and game instances must be isolated. Apply file type/size limits, scanning, timeouts, resource quotas, container/process sandboxing, and network restrictions.

## Frontend direction

The frontend need not be a pixel-perfect historical clone. The useful pattern across successful projects is recognizable information architecture:

- top navigation and account controls
- discoverable game/place cards
- a clear creator dashboard
- catalog/inventory cards
- profile/social pages
- moderation/admin workflows hidden behind roles

Use the archived 2017–2018 Style Guide for component behavior and layout consistency, while making the application name, icons, images, colors, terminology, and assets original.

## Current project implication

The current `catalog-2018-demo.html` should remain a visual experiment, not the production frontend base. The next proper implementation should start with a small component library and original assets, then attach it to a secure account/catalog API.
