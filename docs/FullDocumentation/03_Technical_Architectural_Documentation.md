# Technical / Architectural Documentation

## Architecture Diagrams (Conceptual)
- Web API (ASP.NET Core) → Application (MediatR) → Domain (Entities/Interfaces) → Infrastructure (EF Core, Stripe, Storage).
- External: Stripe (Connect, Billing, Webhooks), Postgres (DB), Redis (planned cache), BAA-covered providers for media/e-sign (Medical).

```
             +-------------------+
             |    Frontend SPA   |
             +---------+---------+
                       |
                       v
             +-------------------+
             |   ASP.NET Core    |
             |   Web API Layer   |
             +---------+---------+
                       |
                       v
             +-------------------+
             |   Application     |
             |  (MediatR CQRS)   |
             +---------+---------+
                       |
                       v
             +-------------------+
             |      Domain       |
             | Entities/Interfaces|
             +---------+---------+
                       |
                       v
             +-------------------+
             |   Infrastructure  |
             | EF Core / Stripe  |
             +---------+---------+
                       |
     +-----------------+------------------+
     |                                    |
     v                                    v
 +---------+                      +-----------------+
 | Postgres|                      | Stripe (APIs)   |
 +---------+                      +-----------------+
     |                                    |
     v                                    v
 +---------+                      +-----------------+
 | Backups |                      | Webhooks        |
 +---------+                      +-----------------+
```

## Technology Stack
- Backend: ASP.NET Core, MediatR, EF Core (Npgsql), Serilog, OpenTelemetry.
- Database: PostgreSQL.
- Payments: Stripe Connect (Standard), PaymentIntents, Billing.
- Auth: JWT (consider OAuth2/OIDC future).
- Observability: HealthChecks, tracing/metrics/logging.

## Folder Structure (Backend)
- `backend/src/Aesthetic.API/*` controllers, filters, Program.
- `backend/src/Aesthetic.Application/*` commands, queries, validators.
- `backend/src/Aesthetic.Domain/*` entities and interfaces.
- `backend/src/Aesthetic.Infrastructure/*` persistence, payments, configurations.
- `backend/tests/*` unit/integration tests.

Example:
- `Aesthetic.API/Controllers/PaymentsController.cs`
- `Aesthetic.Application/Payments/Commands/CreatePaymentIntent/*`
- `Aesthetic.Domain/Entities/Service.cs`
- `Aesthetic.Infrastructure/Persistence/Configurations/ServiceConfiguration.cs`

## Design Decisions
- Two product lines (Beauty vs Medical), isolating PHI and compliance.
- Stripe Connect Standard for payment flow legality; deposits/cancel policies per service.
- HIPAA modules are lightweight: consents, secure media, RBAC/MFA, audit; avoid full EHR scope.
- Idempotency required for booking/cancel/pay; rate limiting and health endpoints for resilience.
- ADRs recorded for major choices (payments, HIPAA scope, caching, auth approach).

## Integrations
- Stripe: account onboarding, PaymentIntents, webhooks; signatures verified via `WebhookSecret`.
- Storage/e‑sign (Medical): only through providers with BAAs; disabled otherwise.

## Contracts
- Domain entities: `Service` (with policies), `Appointment`, `Professional`.
- Repositories: `IServiceRepository`, `IAppointmentRepository`, `IProfessionalRepository`.
- Payment service: `IPaymentService.CreatePaymentIntentAsync(amount, currency, description, connectedAccountId, applicationFee, idempotencyKey, metadata)`.
  - Webhook contract: Stripe `Stripe-Signature` header verification; body parsed for `payment_intent.succeeded` and `metadata.AppointmentId`.
