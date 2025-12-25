# Product Documentation

## Overview
- Aesthetic is a modern web platform for Beauty (no PHI) and Medical Aesthetics (with PHI/HIPAA) to manage booking, payments, and lightweight clinical workflows.
- Key focus: operational simplicity, Stripe-based payments, and HIPAA‑aware modules for medical aesthetics without full EHR complexity.

## Problem Solved
- Fragmented tools for scheduling, payments, and compliance slow clinics and pros.
- Existing marketplace apps add commissions or lack HIPAA modules; full EHRs are heavy and costly.
- Aesthetic unifies booking and payments, with optional HIPAA features for medical aesthetics.

## Personas
- Professional (Beauty): individual stylist/esthetician; needs simple booking, deposits, reminders, and payouts.
- Clinic Admin (Medical): med spa/dermatology; needs secure consents, media, audit, RBAC/MFA, and Stripe Billing.
- Client/Patient: books appointments, pays deposits, receives neutral reminders; accesses a secure portal for sensitive information.

## High-Level Roadmap
- MVP Beauty: booking, services, availability, Stripe Connect, PaymentIntents, neutral reminders.
- v1 Beauty + Billing: subscriptions, recurring schedules, admin panel, customer portal.
- Medical Line: HIPAA modules (consents, secure media, RBAC/MFA, audit), DR/backup, BAAs.
- Expansions: white‑label, executive analytics, premium integrations (e‑sign/telehealth).

## KPIs
- Conversion (booking completion rate), deposit capture rate, cancellation/no‑show reduction.
- ARPU per line (Beauty vs Medical), churn, uptime (SLA), p95 latency.

## Limitations
- Not a full EHR or insurance/claims system.
- Initial OAuth2/OIDC out of scope; JWT used. mTLS/service mesh deferred until multi‑service.
- Initial storage media/consent providers must support BAAs; otherwise disabled in Medical line.
 - Marketplace acquisition optional; primary model is direct SaaS.
