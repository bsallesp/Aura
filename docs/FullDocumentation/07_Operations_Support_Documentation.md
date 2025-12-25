# Operations / Support Documentation

## Health Checks
- `GET /health`: overall health.
- `GET /health/ready`: readiness probe for orchestrators.
- `GET /health/live`: liveness probe.

## SLAs / SLOs
- SLA: availability target (e.g., 99.9% monthly for API).
- SLOs: p95 latency for critical endpoints; webhook processing time thresholds.
  - Examples: p95 latency ≤ 300ms on booking/payment; webhook success ≥ 99.5% within 2 minutes.

## Monitoring & Alerts
- Metrics/traces via OpenTelemetry; logs via Serilog.
- Alerts on 5xx rate, latency breaches, rate-limit spikes, DB connectivity, webhook signature failures.
  - Thresholds: 5xx > 1% over 5 min; p95 > target; rate-limit 429 spikes; DB connection pool saturation.

## Runbooks
- Webhook failures: validate secrets, retry via queue, confirm appointment state.
- DB outage: failover procedure, restore, reconnect.
- Rate-limit incidents: identify abusive IPs, adjust policies temporarily.
 - Cache invalidation: clear `services_active` when service CRUD occurs.

## Troubleshooting
- Payment issues: check Stripe dashboard events, Metadata `AppointmentId` consistency.
- Booking conflicts: review availability and `HasConflictAsync` outcomes.
- Auth problems: inspect JWT issuer/audience, token expiry.
 - Idempotency: verify header presence and key uniqueness; check duplicate prevention logs.
