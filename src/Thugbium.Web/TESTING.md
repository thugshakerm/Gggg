# Manual account-flow test

Run these checks after publishing the app and starting it on the VPS.

1. Open `http://localhost:8091/Account/Register`.
2. Register a new username with an 8+ character password and accept the terms checkbox.
3. Confirm the browser redirects to `/Account/Dashboard` and displays the signed-in username.
4. In PowerShell, confirm a password **hash** (never the original password) is stored:

```powershell
psql -U thugbium -d thugbium -c "SELECT user_name, is_discord_verified, created_at FROM users;"
```

5. Use **Log Out** in the navigation.
6. Open `/Account/Login`, log in using the same username/password, and confirm the Dashboard loads again.
7. Open `/Users/YOUR_USERNAME` and confirm the public profile loads.
8. Open `/Account/Discord` and confirm it displays the configuration warning until Discord OAuth values are configured.

Expected behavior:

- Duplicate usernames are rejected.
- Invalid username/password/terms input is rejected.
- Passwords are stored with ASP.NET's password hashing, not plaintext.
- A valid login creates an HTTP-only secure site cookie.

Do not test with a password you use anywhere else.
