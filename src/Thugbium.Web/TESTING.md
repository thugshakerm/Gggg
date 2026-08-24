# Manual account-flow test

1. Open `https://YOUR-SHRIMP-DOMAIN/`.
2. Use the **Register** tab to create a new username with an 8+ character password and accept the terms checkbox.
3. Confirm the browser redirects to `/home` and displays the signed-in username.
4. In PowerShell, confirm a password **hash** (never the original password) is stored:

```powershell
psql -U thugbium -d thugbium -c "SELECT user_name, is_discord_verified, created_at FROM users;"
```

5. Use **Log Out** in the account dropdown.
6. Use the **Login** tab on the landing page with the same test account and confirm `/home` loads again.
7. Open `/Users/YOUR_USERNAME` and confirm the public profile loads.
8. Open `/settings/preferences`, enable Dark Mode, and confirm the setting persists after a refresh.

Expected behavior:

- Duplicate usernames are rejected on the landing page.
- Invalid registration and login details remain on the landing page with an error message.
- Passwords are stored with ASP.NET password hashing, not plaintext.
- A valid login creates an HTTP-only secure site cookie.
