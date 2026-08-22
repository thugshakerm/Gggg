# 2018 website research: source map

Research performed 2026-08-22. This is an acquisition/reference map for recreating an original 2018-era browser experience. It does **not** mean the archived Roblox HTML, JavaScript, logos, icon sheets, CSS, imagery, or trademarks are cleared to redistribute. Preserve references, then write new implementation code and create original visual assets.

## The most complete directly-clonable source found

### `RobloxLabs/web`

```bash
git clone https://github.com/RobloxLabs/web.git vendor/robloxlabs-web
```

- **Contents:** an ASP.NET Core fan remake with Razor page templates, navigation partials, login/landing/client-installer views, more than 600 JS/CSS files, and more than 1,000 image/SVG/GIF assets.
- **Useful paths:**
  - `Roblox.Website/Pages/Shared/Navigation/` — header, footer, menus, alerts
  - `Roblox.Website/Pages/Shared/Game/` — installer and place-launcher markup
  - `Roblox.Website/Pages/Landing/Animated.cshtml` — landing page
  - `Roblox.Website/wwwroot/CSS/Base/CSS/` — common/page CSS
  - `Roblox.Website/wwwroot/CSS/Pages/` — page-specific styles
  - `Roblox.Website/wwwroot/Images/Icons/` and `Images/Logo/` — icon/logo reference
  - `Roblox.Website/wwwroot/JS/` — legacy-style browser behavior
- **Important limitation:** its README identifies it as a **2013** fan-made version, not a verified 2018 snapshot. It is valuable for source organization and pages/assets, but must not be treated as era-accurate by default.
- **Repository size at research time:** about 246 MB after clone; do not vendor it into this repository unless we decide its licensing/provenance and the required subset.

## 2018 visual/style references that can be cloned

### `tersiswilvin/Roblox-Legacy-Old-Theme`

```bash
git clone https://github.com/tersiswilvin/Roblox-Legacy-Old-Theme.git vendor/legacy-old-theme
```

Useful for browser-side legacy styling and screenshots/thumbnails. The project includes archived CSS, a `src` directory, and configurable 2018 styling. It is a strong reference for spacing, cards, side navigation, currency presentation, social/footer patterns, and old visual variants.

### `ADUploafd/ADUploafd.github.io`

```bash
git clone https://github.com/ADUploafd/ADUploafd.github.io.git vendor/old-revival-ui
```

Small HTML/CSS/JS old-revival project with an `assets/images` directory. It is a starter/reference only, not a historical source of record.

### `Daniel-176/Roblox-Reverse-Engineering-Wiki`

```bash
git clone https://github.com/Daniel-176/Roblox-Reverse-Engineering-Wiki.git vendor/re-wiki
```

Contains `Resources/2014_site_template.zip` and `Resources/2016_site_template.zip`. These are older than the target era but useful for identifying legacy UI components and how an archived site template may be organized.

## Confirmed Internet Archive 2018 captures

The following Wayback captures resolved during research. Save page source and linked resource URLs to a separate **private research cache** if you have rights/permission; do not add retrieved Roblox-owned assets to the product repository.

| Area | 2018 capture | What it contributes |
|---|---|---|
| Games/discovery | `https://web.archive.org/web/20180927231608id_/https://www.roblox.com/games` | Discovery filter structure and game-list terminology. |
| Catalog | `https://web.archive.org/web/20181012170920id_/https://www.roblox.com/catalog/` | Catalog-era chrome and launcher modal behavior. |
| Creator landing page | `https://web.archive.org/web/20181109021456id_/https://www.roblox.com/create` | The clearest captured content page: Studio call-to-action, marketing sections, and resource URLs. |
| Login/launcher | `https://web.archive.org/web/20180928104706id_/https://www.roblox.com/Login` | Historical install/join modal language and presentation. |

Wayback sometimes changes the page URL to an available nearby capture and can block embedded resources via CSP. The `id_` form is still useful when viewing source/resource URLs.

## 2018 page inventory to capture/reference

This is the minimum browser product surface for a 2018-inspired platform. For each page, record: the capture URL/date, screenshot, DOM structure, linked CSS files, linked JS bundles, icon/image URLs, and behavior notes. Then implement an original equivalent.

### Public/account-facing

- Landing: `/`, `/Login`, `/newlogin`, `/signup`
- Discovery: `/games`, genre/filter views, search results
- Game detail/place: `/games/{placeId}/...`
- Catalog: `/catalog/`, item detail
- User profile: `/users/{userId}/profile`
- Groups: group detail, member list, roles
- Avatar/customization: `/my/avatar`, outfits
- Inventory: `/users/{userId}/inventory`
- Social: friends, messages, notifications, followers/following
- Economy: transactions/purchase confirmation (only as reference; do not reproduce branding)

### Creator-facing

- Creator landing: `/create`
- Game management: `/develop` and place configuration
- Create/upload forms: places, badges, game passes, thumbnails, icons
- Studio installer / game-launch modal

### Shared UI components

- Header/navigation in signed-in and signed-out states
- Universal search, dropdowns, toast/alert area, footer
- Login/sign-up and generic confirmation modals
- Game card, user card, item card, avatar thumbnail, badge, pagination
- Tabs, filter popovers, buttons/form controls, loading spinners
- Icon systems, currency indicator, social-media and rating icons

## Useful historical asset leads

- A Roblox DevForum topic documents several legacy website SVG URLs, including a branded-dark SVG identified with an `11062018` filename: `https://devforum.roblox.com/t/svg-files-of-roblox-website-icons/1381916`.
- The `gamesinc/RBX` GitHub repository is an old public HTML snapshot/reference containing a 2018 footer and references to 2018 `images.rbxcdn.com` and `js.rbxcdn.com` bundles. It is presently not a complete source tree in a shallow clone, but it is a valuable lead for finding historical bundle filenames: `https://github.com/gamesinc/RBX`.
- The `Firebladedoge229` legacy URL gist gives a wide historical list of CSS paths, page URLs, forum image paths, and named resources: `https://gist.github.com/Firebladedoge229/3b360e476a4ffd00e0a2d1db1f0b2d6e`.

## Recommended safe workflow

1. Use the verified 2018 Wayback pages for screenshots, layout measurements, and behavior notes.
2. Use `RobloxLabs/web` only as a **reference** for legacy page/component organization, not as an era-authoritative source.
3. Build a clean original component system: `Header`, `Sidebar`, `GameCard`, `ItemCard`, `ProfileHeader`, `Modal`, `Tabs`, and `Pagination`.
4. Draw a fresh icon set and create original branding. Do not ship Roblox word marks, logos, Robux marks, proprietary sprite sheets, thumbnails, or captured HTML/JS bundles.
5. Keep a `sources/` log with original URL, capture date, asset type, and whether the final implementation is a redraw, a licensed asset, or a reference-only observation.
