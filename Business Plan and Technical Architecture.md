# Business Plan and Technical Architecture

## 1. Value Proposition
Build a platform with two clear product lines:

- Beauty (no PHI): scheduling, Stripe Connect payments, availability, reminders, and simple financial reports.
- Medical (with PHI/HIPAA): medical aesthetics (med spas, aesthetic dermatology, laser/injectables clinics) with electronic consents, lightweight clinical notes, secure handling of sensitive media (before/after photos), audit trails, and advanced access controls.

Differentiator: operational simplicity with compliance — payments tokenized by Stripe (PCI), PHI isolated and encrypted, minimum‑necessary access, and Business Associate Agreements (BAAs) with critical providers.

## 2. Customer Segments
Focus on two niches:

- Beauty: traditional salons/spas and aestheticians without clinical records.
- Medical: med spas, aesthetic dermatology, laser/injectables clinics — HIPAA‑driven needs for security and lightweight clinical workflows.

Expansion to barbershops/fitness remains possible through the Beauty line while keeping clinical scope separate.

## 3. Monetization Model
SaaS with segmented plans, aiming for higher ARPU on Medical:

- Beauty: $49/month per professional.
- Medical: $199–$299/month per user or $1k+/month per clinic (HIPAA, consents, secure media, audit, RBAC/MFA, priority support, and BAAs with providers).
- Add‑ons: secure media storage by volume, SMS bundles, advanced analytics, and integrations (telehealth/e‑sign) with BAAs.

Subscriptions via Stripe Billing with lifecycle webhooks to manage access and audit.

## 4. Customer Acquisition
- Online presence: SEO and content in aesthetics niches.
- Social media: targeted campaigns on Instagram/YouTube.
- Local partnerships: associations, equipment vendors, and aesthetics schools.
- Trials/discounts to drive experimentation.
- Optional marketplace play for network effects.

## 5. Competition & Differentiation
- Direct competitors: Fresha, Vagaro, GlossGenius, Mindbody, StyleSeat, Noterro, TherapyNotes.
- Differentiation: focused verticals plus Stripe Connect for legal payment flows, transparent pricing, and a HIPAA‑aware Medical line with secure media, consents, and audit without becoming a full EHR.

### Competitor Analysis (Pros/Cons)
- Fresha (beauty/wellness marketplace)
  - Pros: global marketplace discovery; automated reminders; deposits/no‑show protection; advanced pricing options; low/transparent marketplace fee model (one‑time fee for new clients via Marketplace) [Fresha Pricing, fresha.com/pricing; Marketplace fee FAQ, support.fresha.com].
  - Cons: marketplace fee on first appointment via Marketplace; add‑on charges for extra messaging; not positioned for HIPAA clinical workflows.
- Vagaro (broad salon/fitness; medspa line)
  - Pros: claims HIPAA compliance and BAAs for messaging; medspa EMR features (consents, audit, secure notifications); integrated payments and inventory; telehealth; relatively low base pricing for medspa entry tier [Vagaro Support HIPAA; Vagaro Medical Spa page].
  - Cons: many features are add‑ons; complexity; variable processing fees; breadth may dilute focus on niche clinical aesthetics.
- GlossGenius (beauty/medspa SMB)
  - Pros: strong design/UI; transparent subscription pricing and flat 2.6% processing; free HIPAA add‑on noted; fast payouts; marketing tools and Reserve with Google [GlossGenius Pricing page].
  - Cons: medspa features “coming soon”/advanced package planned; HIPAA add‑on scope limited vs full EMR; may lack deep clinical controls compared to dedicated medical solutions.
- Mindbody (fitness/wellness enterprise)
  - Pros: robust multi‑location, branded apps, marketing suite, large client network [Mindbody Pricing page].
  - Cons: higher subscription tiers; complexity; not targeted at HIPAA clinical workflows.
- StyleSeat (beauty marketplace)
  - Pros: strong client acquisition via marketplace; Smart Pricing to raise revenue on peak slots; one‑time new client fee (30% up to $50) under Premium; dynamic pricing tooling [StyleSeat Premium Plan; Smart Pricing; New Client Connection].
  - Cons: fees on first appointment via marketplace; dynamic pricing may not fit medical compliance contexts; not HIPAA‑oriented.
- Noterro (clinic management for allied health)
  - Pros: HIPAA/PIPEDA messaging and documentation; AWS with BAA; detailed safeguards and automatic logoff; customizable SOAP/forms; mobile workflows [Noterro HIPAA pages].
  - Cons: geared to chiropractic/physio/massage; payments via Square integration; may require tailoring for medical aesthetics revenue ops.
- TherapyNotes (mental health EHR)
  - Pros: HIPAA‑compliant EHR; BAAs required; telehealth; insurance workflows; AI documentation (TherapyFuel) within HIPAA boundaries [TherapyNotes Security/HIPAA; BAA; features].
  - Cons: designed for behavioral health; heavier EHR workflows and billing/claims complexity; cost and learning curve for non‑mental‑health clinics.

### Our Positioning vs Competitors
- Beauty: leverage Stripe Connect, deposits, neutral reminders, simple analytics, and optional marketplace integrations without commission on non‑marketplace bookings.
- Medical: deliver HIPAA‑aware modules (consents/notes/secure media/RBAC/audit) with BAAs and privacy‑by‑design; avoid full EHR complexity while addressing core medspa needs; transparent Billing plans.

References
- Fresha Pricing: https://www.fresha.com/pricing
- Fresha Marketplace Fees: https://www.fresha.com/help-center/knowledge-base/billing-and-fees/188-marketplace-new-client-fees
- Vagaro HIPAA Compliance: https://support.vagaro.com/hc/en-us/articles/360034898234-HIPAA-Compliance
- Vagaro Medical Spa (HIPAA/EMR): https://sales.vagaro.com/business/medical-spa-software
- GlossGenius Pricing/HIPAA add‑on: https://glossgenius.com/pricing
- Mindbody Pricing: https://www.mindbodyonline.com/business/pricing
- StyleSeat Premium/Fees: https://www.styleseat.com/blog/premium-plan/
- StyleSeat Smart Pricing: https://www.styleseat.com/blog/what-is-smart-pricing/
- StyleSeat New Client Fee: https://www.styleseat.com/blog/new-client-connection/
- Noterro HIPAA: https://www.noterro.com/help-articles/26620534201492 and https://www.noterro.com/hipaa
- TherapyNotes HIPAA/BAA: https://support.therapynotes.com/hc/en-us/articles/30661265032219-Business-Associate-Agreement-BAA

## 6. Revenue Projections
- Year 1 (MVP): 100–300 customers · $49–$299/month ⇒ ~$36k–$180k/year (depending on mix).
- Year 2: 1,000–2,000 customers · subscription mix ⇒ ~$360k–$1.2M/year.
- Year 3: 5,000+ customers · subscription mix ⇒ $1.8–$3.6M/year.

Assumptions include typical SaaS churn (5–10%) and upsells.

## 7. MVP & Expansion Roadmap
- MVP (Beauty, 3–4 months): basic scheduling, services/availability, user/professional onboarding, Stripe Connect (Standard), PaymentIntents; neutral reminders and simple reports.
- v1 (Beauty + Billing, 6–8 months): Stripe Billing (plans/webhooks), recurring schedules, admin panel, customer portal.
- Medical line (HIPAA, 6–12 months): electronic consents, lightweight clinical notes, secure sensitive media, RBAC/MFA, immutable audit, policies/training, BAAs with providers, clinical‑financial reports, disaster recovery.
- Expansions: white‑labeling, executive analytics, premium integrations (e‑sign/telehealth) with BAAs.

## 8. Technical Architecture
- Backend (ASP.NET Core): API → Application → Domain → Infrastructure; EF Core/Npgsql; OpenTelemetry/Serilog.
- Frontend (React/Blazor): SPA using Stripe.js/Elements/Checkout; dashboards for professionals/customers.
- Database (PostgreSQL): at‑rest encryption for PII/PHI on the Medical line; salted hashes for passwords.
- Stripe Connect (Standard) and Billing: onboarding via Account Links; PaymentIntents with non‑sensitive metadata; Billing for subscriptions and access control.
- AuthZ/AuthN: JWT with expiration; RBAC/ABAC and minimum‑necessary on Medical modules.
- Core flows: booking and post‑payment confirmation; financial (Beauty) and clinical‑operational (Medical) reporting; recurring scheduling via jobs; subscriptions via Billing.
- Security/Compliance:
  - PCI: Stripe tokenization; no local storage of payment card data.
  - HIPAA (Medical): PHI isolation; TLS 1.2+ in transit and AES‑256 at rest; immutable audit; minimal retention; encrypted backups and DR; hardening/WAF/scans.
- Third‑parties/BAA: only providers with BAAs for PHI (cloud/storage/e‑sign/telehealth); segregation to prevent PHI through services without BAAs.

Modular, privacy/security‑by‑design architecture to enable safe evolution.

## 9. HIPAA Compliance
Consolidated requirements and practices for the Medical line based on official HHS/OCR sources.

- Applicable Rules and Scope
  - Privacy Rule: national standards for PHI use/disclosure and individual rights; minimum necessary (45 CFR Part 160; Subparts A and E of Part 164).
  - Security Rule: administrative, physical, and technical safeguards for ePHI; applies to covered entities and business associates (45 CFR Part 160; Subparts A and C of Part 164).
  - Breach Notification Rule: notifications to individuals, the Secretary, and in certain cases the media for PHI incidents (HITECH Act).

- Platform Role
  - Business Associate (BA) in the Medical line with BAAs signed with customers and PHI‑processing providers.
  - Beauty line out of HIPAA scope by not handling PHI.

- Safeguards (Security Rule)
  - Administrative: continuous risk assessments; policies/procedures; training; incident response; sanctions; Privacy/Security Officers; documentation and periodic reviews.
  - Physical: facility/device access control; media protection; secure disposal.
  - Technical: access control (RBAC/ABAC), strong auth/MFA, encryption (TLS 1.2+ in transit, AES‑256 at rest), integrity, immutable audit trails, session timeouts, segregation.

- Privacy Rule & Minimum Necessary
  - Uses/disclosures per authorizations and applicable permissions; access limited to minimum necessary.
  - Operational support for individual rights via clinics (access/correction) using platform features.

- Breach Notification
  - Formal incident response: risk assessment, logging, required communications, coordination with covered entities and OCR.

- Third‑Parties & BAAs
  - Select only providers with BAAs for any PHI service (cloud, storage, e‑sign, telehealth, monitoring). Maintain system/data inventory and risk matrices.
  - Prohibit PHI in payment integrations (e.g., Stripe); carry only non‑sensitive metadata.

- Operations & Design
  - Minimization/retention: collect only what is necessary; limited retention; periodic access reviews.
  - Audit/monitoring: access/change logs with appropriate retention; anomaly detection; compliance reports.
- DR/Backups: encrypted backups, restore testing, documented procedures; hardening/WAF.

Official References (HHS/OCR)
- Summary of the HIPAA Privacy Rule — https://www.hhs.gov/hipaa/for-professionals/privacy/laws-regulations/index.html
- Privacy (general section) — https://www.hhs.gov/hipaa/for-professionals/privacy/index.html
- Summary of the HIPAA Security Rule — https://www.hhs.gov/hipaa/for-professionals/security/laws-regulations/index.html
- HIPAA Home — https://www.hhs.gov/hipaa/index.html

## 10. Competitive Playbook (HIPAA‑Safe)
- HIPAA‑First Medical Line: BAAs by default; HIPAA‑compliant messaging (exclude service names, neutral language); secure portal for all PHI access.
- Secure Media Module: role‑based access, encrypted storage, immutable audit; watermarked exports; no PHI in filenames/URLs.
- Consent Management: electronic consents via providers with BAAs; standardized templates; audit of consent lifecycle.
- Pricing & Policies: deposits at booking; configurable late‑cancel/no‑show fees; optional peak‑slot pricing without exposing treatment details.
- Migration Concierge: secure import of clients, appointments, and media; attestations for PHI handling; zero‑downtime go‑live.
- HIPAA‑Safe AI: on‑prem or BAA‑covered vendor; no PHI to non‑BAA services; features limited to summarization/templates within the walled environment.
- Transparent Subscriptions: Stripe Billing; no marketplace commissions; clear add‑ons for secure media, SMS bundles, analytics, and integrations (with BAAs when PHI is involved).
- Modern UX: frictionless booking, portal UX with MFA optional, mobile‑friendly dashboards, Reserve with Google (Beauty only, no PHI), fast payouts.
- Enterprise Ops: RBAC/ABAC, MFA, immutable audit, configurable retention, DR/BCP, vulnerability scanning.

## 11. Immediate Actions (Non‑Code Plan)
- Stripe Connect Alignment: create Standard accounts during onboarding; maintain destination transfers.
- Webhook Secrets: standardize configuration; verify signature on payment/account events.
- Billing: implement plans/webhooks for subscription lifecycle; gate Medical features by active plan.
- Deposits & Fees: add booking deposit and late‑cancel/no‑show policies to services; ensure neutral notifications.
- HIPAA Messaging Policy: enforce neutral reminder templates (no service names); in‑portal sensitive communications only.
- Media & Consents: define data model and storage requirements for secure media and consent records; select BAA‑covered storage/e‑sign.
- Audit & RBAC: specify access matrices; log reads/writes of PHI; periodic access reviews and reports.

## 12. Architecture Standards Alignment
- Architecture/Design
  - API Versioning: in place (URL + header). Maintain backward compatibility.
  - Circuit Breaker + Retry + Timeout: in place via Polly pipeline for Stripe.
  - Idempotency: PaymentIntents accept header; extend to booking/cancellation.
  - Load Balancer / API Gateway / Service Discovery: not required for single service; plan for gateway if we split services.
- Communication
  - Event‑driven: Stripe webhooks + domain events; expand internal events.
  - Outbox pattern: planned for reliable webhook/event publishing and eventual consistency.
  - Saga/Process Manager: planned for multi‑step flows (booking → payment → confirmation).
  - Async messaging: optional backlog (RabbitMQ/Service Bus) for jobs/media.
  - Schema versioning: maintain DTO/OpenAPI changes with version sets.
- Observability
  - Centralized Logging: Serilog.
  - Distributed Tracing & Metrics: OpenTelemetry.
  - Health Checks: in place; add readiness/liveness probes for orchestration.
- Security
  - Authentication: JWT (consider OAuth2/OIDC later).
  - Authorization: RBAC/ABAC planned (Medical line minimum necessary).
  - Secrets management: externalize to vault and rotate regularly.
  - mTLS: not applicable now; revisit with multi‑service.
  - Rate limiting/throttling: planned middleware for abusive patterns.
- Resilience
  - Graceful degradation/fallbacks: define defaults for payment/connect outages.
  - Chaos testing: backlog.
  - DLQs/backpressure: with messaging adoption.
- Data
  - CQRS: in place.
  - Caching (Redis): planned for slots/services queries.
  - Eventual consistency: adopt with outbox + sagas.
  - DB per service / read replicas: not needed now; plan read replicas for scale.
- Versioning & Compatibility
  - API versioning: in place; ensure backward compatibility on changes.
  - Feature flags: planned for gradual rollouts.
  - Canary/Blue‑Green: planned in deployment strategy.
- DevOps/Operations
  - CI/CD: planned pipelines with quality gates.
  - IaC: planned (infra provisioning + secrets/config externalization).
  - Auto‑scaling: planned with orchestration.
- Governance & Quality
  - Code quality/static analysis/dependency scanning: planned in CI.
  - ADRs: maintain decision records for key architecture choices.
  - Common code patterns: DTOs, anti‑corruption layer, policy pipelines, cross‑cutting behaviors, domain events (in place and expanding).
