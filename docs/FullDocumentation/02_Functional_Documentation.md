# Functional Documentation

## Use Cases
- Register/Login (JWT), Professional onboarding (Stripe Connect Standard).
- Create services and policies (deposit %, cancel fee, window hours).
- Client booking with availability checks and conflict prevention.
- Payment intent creation with deposit/full amount; webhook confirmation.
- Cancel appointment with policy-based fee.
- Medical: electronic consents and secure media management.

## Flows
- Booking
  - Client selects service/date/time → availability validated → appointment created (Pending) → PaymentIntent (deposit or full) → on success, appointment remains Pending until webhook confirmation.
- Payment Webhook
  - Verify signature → parse `payment_intent.succeeded` → read metadata `AppointmentId` → confirm appointment → audit trail entry.
- Cancelation
  - Client requests cancel → if within `CancelFeeWindowHours`, apply `CancelFeePercentage` of price; else free. Notifications are neutral.
- Professional Onboarding
  - Create Standard account → Account Link → redirect to Stripe-hosted onboarding → update StripeAccountId → status updated upon account webhook.

## Validation Rules
- Service: name required; price > 0; duration 1–480 minutes.
- Policies: percentages in [0,1]; window hours ≥ 0.
- Booking: time within availability; no conflicts; future date/time.
- Payments: `Idempotency-Key` header required; metadata contains only non‑sensitive IDs.

## Error Scenarios
- 404: Service/Professional/Appointment not found.
- 409: Time slot conflict or outside working hours.
- 400: Missing `Idempotency-Key`, invalid payload/percentages.
- 401/403: Unauthorized/forbidden actions.
- 502: Payment provider error (circuit breaker fallback).

## Acceptance Criteria
- Booking prevents conflicts and respects availability.
- PaymentIntents use deposit when set; webhook confirms appointment.
- Cancel applies fee correctly per policy; notifications are neutral.
- Idempotency enforced on booking/cancel/payment endpoints.
- Stripe webhook signature verified with configured secret.
 - No PHI in payment metadata or external notifications.
 - Admin can update service policies with validation and persistence.
