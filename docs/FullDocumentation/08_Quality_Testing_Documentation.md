# Quality / Testing Documentation

## Test Strategy
- Unit tests for handlers (booking, payment intent, cancel).
- Integration tests for end-to-end flows (register → auth → create service → create intent).
- Contract tests for webhook signature handling.
 - Performance tests (latency through critical paths) and chaos tests (controlled fault injection).

## Test Types
- Unit, integration, API/contract, performance (latency/throughput), security (authz, rate limits).

## Test Data
- Synthetic appointments/services; randomized emails; deposits/cancel windows; PaymentIntent mocks.
 - PHI-like fields used only in Medical sandbox; anonymized and non-identifiable.

## Test Environments
- Dev: local DB and Stripe test keys.
- CI: ephemeral DB; webhooks simulated; secrets injected via CI vault.

## Automated Tests
- CI runs unit/integration suites; code coverage gating.
- Linting/static analysis; dependency scanning.
 - Coverage target: ≥ 80% lines/branches on application layer; track critical paths.
