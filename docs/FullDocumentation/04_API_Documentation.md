# API Documentation

## Authentication
- JWT Bearer via `Authorization: Bearer <token>`.
- Idempotency: `Idempotency-Key: <uuid>` required for booking, cancel, and payment intent creation.

## Versioning
- Path/versioning headers (e.g., `Accept: application/json; version=1`). Backward compatibility maintained.

## Rate Limits
- Global: 100 requests/min per IP (`429 Too Many Requests` on exceed).

## Endpoints (Examples)
- POST `/auth/register` → register user (Professional/Client).
  - Professional capabilities require `role=Professional` in JWT.
- POST `/auth/login` → obtain JWT.
- POST `/connect/start` → initiate Stripe onboarding, returns Account Link URL.
- POST `/payments/create-intent` → create PaymentIntent for `AppointmentId`. Uses deposit if configured.
- POST `/payments/webhook` → Stripe webhook endpoint (signature required).
- POST `/appointments` → create appointment (Pending) after availability validation.
- PUT `/appointments/{appointmentId}/cancel` → cancel; applies fee if in window.
- GET `/services` → list active services (cached 60s).
- PUT `/services/{serviceId}/policies` → update deposit/cancel policies.
  - Requires role: `Professional`.
- GET `/health`, `/health/ready`, `/health/live` → health probes.

## Request/Response (Samples)
- Create PaymentIntent
```http
POST /payments/create-intent
Headers:
  Authorization: Bearer <token>
  Idempotency-Key: 4f5e3c3f-... 
Body:
  { "AppointmentId": "c0a1..." }
Response:
  { "ClientSecret": "pi_..._secret_..." }
```

- Update Service Policies
```http
PUT /services/{serviceId}/policies
Headers:
  Authorization: Bearer <token>
Body:
  { "DepositPercentage": 0.25, "CancelFeePercentage": 0.50, "CancelFeeWindowHours": 24 }
Response: 204 No Content

- Cancel Appointment
```http
PUT /appointments/{appointmentId}/cancel
Headers:
  Authorization: Bearer <token>
  Idempotency-Key: 1e2d...
Response: 204 No Content
Notes:
- If within `CancelFeeWindowHours`, a fee is calculated as `CancelFeePercentage * PriceAtBooking` and persisted.
```
```

## Errors
- 400: validation errors, missing `Idempotency-Key`.
- 401/403: auth/authorization failures.
- 404: resource not found.
- 409: slot conflict.
- 429: rate limit exceeded.
- 500/502: internal/provider errors.

### Error Codes
- `E-VAL-001`: Validation error (details in payload).
- `E-AUTH-001`: Missing/invalid token.
- `E-BOOK-409`: Booking conflict.
- `E-PAY-502`: Payment provider error.
- `E-RATE-429`: Rate limit exceeded.

## OpenAPI
- The API is documented via Swagger/OpenAPI and exposed in development.

### Minimal OpenAPI Snippet
```yaml
openapi: 3.0.3
info:
  title: Aesthetic API
  version: 1.0.0
servers:
  - url: https://api.aesthetic.example.com
components:
  securitySchemes:
    bearerAuth:
      type: http
      scheme: bearer
      bearerFormat: JWT
  schemas:
    Error:
      type: object
      properties:
        code:
          type: string
        error:
          type: string
security:
  - bearerAuth: []
paths:
  /payments/create-intent:
    post:
      summary: Create PaymentIntent
      parameters:
        - in: header
          name: Idempotency-Key
          required: true
          schema:
            type: string
      requestBody:
        required: true
        content:
          application/json:
            schema:
              type: object
              properties:
                AppointmentId:
                  type: string
      responses:
        '200':
          description: OK
          content:
            application/json:
              schema:
                type: object
                properties:
                  ClientSecret:
                    type: string
        '400':
          description: Bad Request
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/Error'
        '429':
          description: Too Many Requests
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/Error'
```
