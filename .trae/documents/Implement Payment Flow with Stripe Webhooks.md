# Implement Payment Flow with Webhooks

## 1. Update Payment Service Interface & Implementation
- **Goal**: Enable passing metadata (AppointmentId) to Stripe Payment Intents.
- **Action**: 
    - Modify `IPaymentService.CreatePaymentIntentAsync` to accept `Dictionary<string, string>? metadata`.
    - Update `StripePaymentService.CreatePaymentIntentAsync` to map this metadata to `PaymentIntentCreateOptions.Metadata`.

## 2. Refactor CreatePaymentIntent Command
- **Goal**: Securely create payment intents based on actual Appointment data, not client-provided amounts.
- **Action**:
    - Update `CreatePaymentIntentCommand` to replace `Amount/Currency` with `AppointmentId`.
    - Update `CreatePaymentIntentCommandHandler`:
        - Fetch `Appointment` by ID.
        - Validate Appointment status (must be Pending).
        - Use `Appointment.PriceAtBooking` for the amount.
        - Fetch `Professional` to get `StripeAccountId` (for destination charges/transfers).
        - Call `_paymentService.CreatePaymentIntentAsync` with `AppointmentId` in metadata.

## 3. Implement Stripe Webhook Handling
- **Goal**: Asynchronously confirm appointments when payment succeeds.
- **Action**:
    - Add `WebhookSecret` to `StripeSettings`.
    - Create `StripeWebhookEndpoint` in `PaymentsController` (or dedicated `WebhooksController`).
    - Logic:
        - Verify Stripe Signature.
        - Handle `payment_intent.succeeded` event.
        - Extract `AppointmentId` from metadata.
        - Dispatch `ConfirmAppointmentCommand` via MediatR.

## 4. Update API Controller
- **Goal**: Expose the new secure payment intent creation logic.
- **Action**:
    - Update `PaymentsController.CreatePaymentIntent` to accept `AppointmentId`.
    - Ensure Idempotency Key is still handled.

## 5. Verification
- **Tests**:
    - Unit test for `CreatePaymentIntentCommandHandler` (verifying amount matches appointment).
    - Unit test for `ConfirmAppointmentCommandHandler`.
