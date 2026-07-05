# Methrax.Bff.AspNetCore

A lightweight **Backend-for-Frontend (BFF) authentication library for ASP.NET Core** built on top of **Cookie Authentication + OpenID Connect (OIDC)**.

It provides a simple and opinionated way to implement secure BFF-style authentication for modern web applications (Angular, SPA, server-backed frontends).

---

## ✨ Features

- Cookie-based authentication
- OpenID Connect (OIDC) integration
- BFF-friendly secure defaults
- Centralized configuration via `appsettings.json`
- Minimal setup API
- Customizable authentication endpoints
- Secure cookie handling (HttpOnly, SameSite, SecurePolicy)

---

## 🚀 Installation

```bash
dotnet add package Methrax.Bff.AspNetCore

```

## 🧩 Enable BFF Middleware

To enable Backend-for-Frontend (BFF) authentication in your application, register the BFF authentication services in `Program.cs`.

This sets up cookie authentication and OpenID Connect handling required for secure BFF flows.

```csharp
// Add BFF Middleware to handle authentication and authorization flows in a BFF architecture.
builder.Services.AddBffAuthentication();
```
---
## ⚙️ Configuration Reference

All settings are defined under the `BffAuthentication` section in `appsettings.json`.

If a value is not provided, the following **default values** are used.

---

### 🔹 Root Configuration

| Property | Type | Required | Default Value | Description |
|----------|------|----------|---------------|-------------|
| Authority | string | ✅ | `""` | OpenID Connect authority URL (Identity Provider) |
| ClientId | string | ✅ | `""` | OAuth2 / OIDC client identifier |
| ClientSecret | string | ✅ | `""` | OAuth2 client secret |
| RequireHttpsMetadata | bool | ❌ | `true` | Enforces HTTPS metadata validation |
| Scopes | string[] | ❌ | `["openid", "profile", "offline_access"]` | Requested OIDC scopes |
| Cookie | object | ❌ | See Cookie defaults below | Cookie authentication configuration |
| Endpoints | object | ❌ | See Endpoint defaults below | Authentication endpoints |

---

### 🔹 Scopes Default Behavior

If `Scopes` is not configured, the following are used:

```text id="1nq9v2"
openid
profile
offline_access
```

### 🔹 Cookie Configuration

| Property     | Type                         | Required | Default Value | Description                                                  |
|--------------|------------------------------|----------|---------------|--------------------------------------------------------------|
| SameSite     | `SameSiteMode`              | ❌        | `Lax`         | Controls cookie cross-site behavior                          |
| SecurePolicy | `CookieSecurePolicy`        | ❌        | `Always`      | Defines when the cookie should only be sent over HTTPS       |
| HttpOnly     | `bool`                      | ❌        | `true`        | Prevents JavaScript access to the authentication cookie      |

### 🔹 Endpoint Configuration

| Property         | Type   | Required | Default Value    | Description                    |
|------------------|--------|----------|------------------|--------------------------------|
| LoginPath        | string | ❌        | `/login`         | Endpoint used to trigger login |
| LogoutPath       | string | ❌        | `/logout`        | Endpoint used to sign out user |
| AccessDeniedPath | string | ❌        | `/access-denied` | Redirect path for access denied |

### Example Configurations

```
{
  "BffAuthentication": {
    "Authority": "<Authority_URL>",
    "ClientId": "<Client_ID>",
    "ClientSecret": "<Client_Secret>",
    "RequireHttpsMetadata": true,

    "Scopes": [
      "openid",
      "profile",
      "offline_access"
    ],

    "Cookie": {
      "SameSite": "None",
      "SecurePolicy": "Always",
      "HttpOnly": true
    },

    "Endpoints": {
      "LoginPath": "/login",
      "LogoutPath": "/logout",
      "AccessDeniedPath": "/access-denied"
    }
  }
}
```


## 📖 Deep Dive (Architecture & Security Model)

This library is based on a detailed exploration of modern authentication evolution and BFF architecture.

👉 **Full article:**  
[Beyond PKCE: Building Secure Backend-for-Frontend (BFF) Authentication with Angular, .NET & Keycloak](https://tharaka-madhusanka.medium.com/beyond-pkce-building-secure-backend-for-frontend-bff-authentication-with-angular-net-ff29d5a42a49)
---

### 🧠 What the Article Covers

This library is the practical implementation of the concepts explained in the article below:

---

### 1. Evolution of Modern Authentication
- OAuth 2.0 fundamentals
- PKCE ("Pixie") flow
- OAuth 2.1 improvements and security tightening

---

### 2. What is PKCE Authentication?
- How public clients (SPA/mobile) authenticate securely
- How code challenge & verifier work
- Why PKCE became the default for SPAs

---

### 3. Why PKCE Alone Is Not Fully Secure
- Token storage risks in browsers
- XSS-based token theft
- Refresh token exposure issues
- Limitations of client-side OAuth

---

### 4. What is Backend-for-Frontend (BFF)?
- Moving authentication from browser → server
- Using secure cookies instead of tokens
- Server-controlled session management
- Eliminating token exposure in frontend apps

---

### 5. Key Requirements for a Secure BFF Implementation

#### 5.1 SameSite + Secure Cookie Strategy
- Preventing CSRF attacks
- Cross-site request behavior
- Secure cookie transmission rules

#### 5.2 Recommended BFF Configuration
- Cookie policies
- OIDC flow setup
- Secure defaults for production environments

---

### 6. Implementing BFF Authentication

- Transition from cross-domain SPA auth
- Moving to same-domain secure BFF model
- ASP.NET Core implementation patterns
- Angular integration without token handling

---

## 🔗 Relationship to This Library

This NuGet package is the **production-ready implementation** of the architecture described above:

- 📘 Article = Theory & Security model
- 📦 Library = Practical implementation
- 🧪 Sample project = Real-world usage

---

## 📜 License

> © 2026 Tharaka Madhusanka - Methrax  
> This project is open source and free to use under the MIT License.