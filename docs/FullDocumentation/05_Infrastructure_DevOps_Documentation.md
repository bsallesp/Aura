# Infrastructure / DevOps Documentation

## Environments
- Development: local Postgres via Docker; Swagger enabled.
- Staging/Production (Azure): Azure App Service (API), Azure Database for PostgreSQL Flexible Server, Azure Cache for Redis, Azure Storage (media), Azure Key Vault, Azure Front Door + WAF, Azure Monitor/Application Insights/Log Analytics.
- HIPAA: operate only on Azure services covered under Microsoft’s Business Associate Agreement (BAA); maintain tenant separation and minimum necessary access.

## IaC
- Plan: provision databases, caches, storage, and app services via IaC (e.g., Terraform/Bicep).
  - Modules: API app service, Postgres, Redis (cache), storage (media), secrets manager, monitoring.
  - Azure specifics: private endpoints (Postgres/Storage), VNET integration for App Service, diagnostic settings to Log Analytics, autoscale rules, Front Door routing/WAF policies.

## CI/CD
- Build, test, quality gates, security scans, deploy with canary/blue‑green.
  - Steps: lint/static analysis → unit/integration tests → container build → scan dependencies/images → deploy to staging → smoke tests → canary to prod → full rollout.
  - Azure pipelines: GitHub Actions or Azure DevOps Pipelines with environments and approvals.

## Environment Variables
- `ConnectionStrings__DefaultConnection`
- `JwtSettings__Secret`, `JwtSettings__ExpiryMinutes`, `JwtSettings__Issuer`, `JwtSettings__Audience`
- `StripeSettings__SecretKey`, `StripeSettings__PublishableKey`, `StripeSettings__WebhookSecret`
- Provider-specific secrets for Medical integrations (only with BAAs).
 - Azure settings: `APPINSIGHTS_CONNECTIONSTRING`, `AZURE_STORAGE_CONNECTION_STRING` (or SAS with Key Vault), `POSTGRES_CONNECTION` (Key Vault secret), `REDIS_CONNECTION`, `FRONTDOOR_ENDPOINT`.

## Secrets
- Store in a secrets manager (Key Vault/Parameter Store). Rotate regularly.
  - Rotation: 90-day cycle or on incident; audit access logs.
  - Azure Key Vault: RBAC-enabled access; use Managed Identity from App Service; disable public network access and enforce private endpoint.

## Scaling
- Horizontal scaling of API; cache hot paths (services/slots) with Redis.
- Rate limiting and health probes configured.
 - Azure autoscale: scale based on CPU, requests, or custom metrics (App Insights).

## Backup & Restore
- Encrypted DB backups daily; test restores periodically.
  - Retention: 30 days; weekly full backups; point-in-time restore capability.
  - Azure PostgreSQL: PITR configured; storage redundancy (ZRS/GRS) as required.

## Disaster Recovery
- Document RTO/RPO; cross-region failover for DB and storage where applicable.
  - Targets: RPO ≤ 15 min; RTO ≤ 2 hours.
  - Runbook: promote replica → update connection strings → verify health/probes → notify stakeholders.
  - Azure Site Recovery (as needed) and geo‑redundant Azure Storage.

## Compliance (Azure HIPAA)
- Microsoft provides HIPAA Business Associate Agreement (BAA) for eligible Azure services.
- Use only BAA‑covered services for PHI and configure encryption, access controls, logging, and auditing.
- References: Microsoft Trust Center (HIPAA/HITECH), Azure compliance documentation.
