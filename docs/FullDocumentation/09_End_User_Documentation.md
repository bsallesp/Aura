# End-User Documentation

## User Manuals (Beauty)
- Create account and login.
- Add services and policies (deposit, cancel fees).
- Set availability; publish booking link.
- Manage appointments; cancellations and fees.

## User Manuals (Medical)
- Enable HIPAA features; sign BAAs.
- Manage electronic consents; upload secure before/after photos.
- Configure RBAC/MFA for staff; review audit logs.

## FAQs
- Why do I need an Idempotency-Key? Prevents duplicate operations.
- How are deposits charged? A percentage of the service price at booking.
- Are reminders HIPAA-compliant? Yes, content is neutral (no service names/PHI).
 - What happens if I cancel? If within the configured window, a cancellation fee may apply based on the policy set by your provider.

## Tutorials
- Stripe onboarding: generate Account Link and complete hosted flow.
- Configure service policies: set deposit and cancel windows.
- Confirm appointments via PaymentIntents and webhooks.

## Onboarding
- Guided setup: services, availability, Stripe, policies, portal branding.
 - Medical: sign BAAs before enabling consents/media; assign roles and MFA.

## Quick Guides
- Update policies: `Services → Policies` (deposit %, cancel %, window hours).
- Health checks: `/health`, `/health/ready`, `/health/live`.

## Videos (If Applicable)
- Short clips: onboarding, booking, payment confirmation, consents/media handling.
