# Authentication and Authorization

The API identifies callers with JWT bearer tokens and controls access with role-based access control (RBAC). Users and roles are stored with ASP.NET Core Identity in the same SQL Server database as the cafes and employees.

## Roles and access

There are two roles, defined in `Domain/Constants/Roles.cs`:

| Role | Can do |
|---|---|
| `User` | Read cafes and employees |
| `Admin` | Everything `User` can do, plus create, update and delete cafes and employees, and assign roles |

| Endpoint | Access |
|---|---|
| `POST /api/Auth/register` | Anyone |
| `POST /api/Auth/login` | Anyone |
| `POST /api/Auth/assign-role` | `Admin` |
| `GET /api/Cafe`, `GET /api/Cafe/{id}` | Any logged-in user |
| `POST`, `PUT`, `DELETE /api/Cafe...` | `Admin` |
| `GET /api/Employee`, `GET /api/Employee/{id}` | Any logged-in user |
| `POST`, `PUT`, `DELETE /api/Employee...` | `Admin` |
| `/Uploads/*` (cafe logos) | Anyone |

Self-registered users always get the `User` role, so nobody can make themselves an admin. An admin promotes a user with `assign-role`.

## Using the API

### 1. Get a token

Log in (or register) and read `data.token` from the response:

```
curl -X POST https://localhost:7199/api/Auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@cafe.local","password":"Admin@12345"}'
```

```json
{
  "success": true,
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIs...",
    "expiresAt": "2026-10-04T13:21:31Z",
    "email": "admin@cafe.local",
    "roles": ["User", "Admin"]
  },
  "message": "Login successful",
  "errors": []
}
```

`register` takes the same body and returns the same shape, so a new user is logged in straight away.

### 2. Send the token on every request

```
curl https://localhost:7199/api/Cafe -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIs..."
```

In Swagger UI (`https://localhost:7199/swagger`), click **Authorize** and paste only the token. Swagger adds the `Bearer ` prefix itself.

Tokens expire after 60 minutes (`Jwt:ExpiryMinutes`). There is no refresh token, so log in again to get a new one.

### Promoting a user to admin

Call this with an admin's token:

```
curl -X POST https://localhost:7199/api/Auth/assign-role \
  -H "Authorization: Bearer <admin token>" \
  -H "Content-Type: application/json" \
  -d '{"email":"jane@example.com","role":"Admin"}'
```

Roles are stored inside the token, so the change takes effect the next time that user logs in.

## How a request is checked

Every request passes through the middleware registered in `Program.cs`, in this order:

```
request → ExceptionMiddleware → HttpsRedirection → StaticFiles → CORS
        → UseAuthentication   (who are you?)
        → UseAuthorization    (are you allowed?)
        → controller action
```

Before these steps run, routing has already matched the URL to a controller action, so the app knows that action's `[Authorize]` attributes.

### Step 1: Authentication (who are you?)

`UseAuthentication` runs the JWT bearer handler configured in `API/Extensions/AuthenticationExtensions.cs`. It looks for an `Authorization: Bearer <token>` header.

- **No header:** the caller stays anonymous, so `HttpContext.User.Identity.IsAuthenticated` is `false`.
- **Header present:** the handler checks the token:
  - the signature, using `Jwt:Key`, so a forged or edited token is rejected;
  - the issuer (`Jwt:Issuer`) and audience (`Jwt:Audience`);
  - the expiry, with 1 minute of tolerance for clock differences.

  If every check passes, it builds `HttpContext.User` from the token's claims: `sub` (user id), `email`, and one `role` claim per role. If any check fails, the caller stays anonymous.

This step never rejects a request by itself. It only works out who is calling.

### Step 2: Authorization (are you allowed?)

`UseAuthorization` compares `HttpContext.User` with the action's attributes:

| Attribute | Requirement | If it fails |
|---|---|---|
| `[Authorize]` (on `CafeController` and `EmployeeController`) | The user is authenticated | 401 Unauthorized |
| `[Authorize(Roles = Roles.Admin)]` (create, update, delete, assign-role) | `User.IsInRole("Admin")`, which checks the `role` claims | 403 Forbidden |
| `[AllowAnonymous]` (register, login) | None | — |

- **401** means "I don't know who you are": no token, or an invalid one.
- **403** means "I know who you are, but you're not allowed."

Both errors use the API's usual response shape:

```json
{
  "success": false,
  "data": null,
  "message": "Unauthorized",
  "errors": ["A valid bearer token is required."]
}
```

The API keeps no session and doesn't look anything up in the database on these checks. The signed token alone proves who the caller is and what roles they have.

### Troubleshooting a 401

If you send a token and still get 401, check for these:

- The token is more than 60 minutes old.
- The prefix is doubled (`Bearer Bearer ...`). This happens when you type "Bearer" into Swagger's Authorize box.
- The token was issued while the API had a different `Jwt:Key`.

## Login and registration

The handlers live in `Application/Features/Auth/Commands/` and use two interfaces implemented in `Infrastructure/Identity/`:

- `IIdentityService` (`IdentityService`) wraps Identity's `UserManager` and `RoleManager`. It creates users, checks passwords and assigns roles.
- `IJwtTokenGenerator` (`JwtTokenGenerator`) signs tokens with HMAC-SHA256.

Rules enforced:

- Passwords need at least 8 characters, an uppercase letter, a lowercase letter, a digit and a symbol. Emails must be unique.
- After 5 failed logins an account is locked for 5 minutes.
- A failed login always returns "Invalid email or password.", whether the email is unknown, the password is wrong or the account is locked, so the response doesn't reveal which accounts exist.
- Login and register requests implement `ISensitiveRequest`, so `LoggingBehaviour` logs only the request name and never the password or token.

Validation failures from FluentValidation, such as an empty email or a password shorter than 8 characters, return 500 like every other validation error in this API. Password rules that Identity enforces itself return 400.

## Configuration

`appsettings.json`:

```json
"Jwt": {
  "Issuer": "CafeEmployeeManagement.API",
  "Audience": "CafeEmployeeManagement.Web",
  "Key": "",
  "ExpiryMinutes": 60
}
```

`Jwt:Key` is the signing key. It is empty in `appsettings.json`, and the app refuses to start unless a key of at least 32 bytes is supplied. Development uses the dev-only key in `appsettings.Development.json`. In any other environment, supply it with an environment variable (`Jwt__Key`) or user secrets:

```
dotnet user-secrets set "Jwt:Key" "<a random string of 32+ characters>" --project CafeEmployeeManagement.API
```

Changing the key invalidates every token already issued.

### Admin account

On startup, `IdentitySeeder` creates an admin account from the `SeedAdmin` section if that account doesn't exist yet. Development config uses:

| Email | Password |
|---|---|
| `admin@cafe.local` | `Admin@12345` |

The seeder skips this when `SeedAdmin` isn't configured. If seeding fails, for example because the migration hasn't been applied, it logs the error and the app still starts.

### Database

The `AddIdentity` migration creates the `AspNet*` tables and seeds the `Admin` and `User` roles. Apply it like any other migration:

```
dotnet ef database update --project CafeEmployeeManagement.Infrastructure --startup-project CafeEmployeeManagement.API
```

## Adding a protected endpoint

- To an existing controller: it inherits the class-level `[Authorize]`, so any logged-in user can call it. Add `[Authorize(Roles = Roles.Admin)]` to limit it to admins.
- To a new controller: add `[Authorize]` at class level. Without it, the endpoint is public, because there is no global fallback policy. One isn't used, so that `/Uploads` stays public.
- For a new role: add it to `Roles.cs` and `Roles.All`, seed it in `SeedDataStore.cs`, and add a migration.
