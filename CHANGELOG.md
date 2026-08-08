## [1.1.0] - 2026-07-31

### Added
- `SaveTokens` configuration support for managing token persistence in authentication properties.
- `EnableServerSideSessions` configuration support for server-side ticket storage.
- `ITicketStore` integration support to offload session state from client-side cookies.
- Eager startup options validation via `BackendForFrontendOptionsValidator` (`ValidateOnStart`).

### Changed
- **BREAKING CHANGE**: Configuration section name changed from `BffAuthentication` to `Methrax:BffAuthentication`. Existing `appsettings.json` files must update their root key.

---

## [1.0.1] - 2026-07-25

### Improved
- Updated README documentation and architecture details.

## [1.0.0] - 2026-07-20

### Initial Release
- Core Cookie Authentication and OIDC integration.
- `AddBffAuthentication()` service collection extension methods.