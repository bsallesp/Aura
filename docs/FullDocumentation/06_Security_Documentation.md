# Security Documentation

## Authentication / Authorization
- Authentication: JWT Bearer.
- Authorization: RBAC enforced for Professional/Admin endpoints; ABAC planned for Medical minimum necessary access.
- MFA optional for Medical portal access.

## Encryption
- In transit: TLS 1.2+.
- At rest: AES‑256 for PII/PHI in Medical.

## Secrets Management
- External secrets manager; rotation; audit of access.

## Threat Model
- Attack vectors: credential stuffing, brute force, rate abuse, webhook forgery, data exfiltration.
- Mitigations: rate limiting, strong JWT, webhook signature validation, idempotency, audit trails, role-based access.

### STRIDE Mapping
- Spoofing: enforce JWT validation, optional MFA, IP-based rate controls.
- Tampering: immutable audit logs, integrity checks, restricted write access.
- Repudiation: audit trails for administrative and PHI access events.
- Information Disclosure: minimum necessary access, encryption in transit/at rest.
- Denial of Service: rate limiting, backpressure, circuit breakers.
- Elevation of Privilege: RBAC/ABAC with least privilege, reviews.

## Audit Logs
- Immutable logs of PHI read/write (Medical), security events, and administrative actions.
- Retention: configurable per tenant (e.g., 1–7 years) per regulatory guidance.

## Compliance
- HIPAA: Privacy Rule, Security Rule, Breach Notification; BAAs with customers and PHI providers; neutral notifications.
- PCI: Stripe tokenization; no card data stored.

### References (HHS/OCR)
- Summary of the HIPAA Privacy Rule — https://www.hhs.gov/hipaa/for-professionals/privacy/laws-regulations/index.html
- Summary of the HIPAA Security Rule — https://www.hhs.gov/hipaa/for-professionals/security/laws-regulations/index.html

### Azure HIPAA
- Microsoft signs BAAs for eligible Azure services; configure Key Vault, App Service, PostgreSQL, Storage, Monitor per HIPAA safeguards.
- Use private endpoints, encryption, role-based access, immutable audit logs, and least-privilege with Managed Identity.
