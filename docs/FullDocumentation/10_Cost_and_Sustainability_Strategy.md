# Cost & Sustainability Strategy

## Overview
This document outlines the infrastructure cost strategy for Aura, balancing strict **HIPAA compliance** requirements with financial viability for the launch phase. The strategy adopts a "Scale-as-you-grow" approach, utilizing Azure's flexible tiers while maintaining security non-negotiables.

## 1. Launch Architecture (MVP)
**Target Cost:** ~$65 - $100 USD / month
**Focus:** Essential compliance, functional validation, low traffic (< 100 concurrent users).

| Service | SKU / Configuration | Estimated Cost | Justification |
| :--- | :--- | :--- | :--- |
| **App Service** | **B1 (Basic Linux)** | ~$13.00 | Supports SSL and Custom Domains. No auto-scale (manual scaling only). |
| **Database** | **PostgreSQL Flexible (B1ms)** | ~$35.00 | **Critical:** Must use Flexible Server to support VNET integration (Private Access) for HIPAA. |
| **Frontend** | **Static Web Apps (Free)** | $0.00 | Global distribution, free SSL, sufficient bandwidth for launch. |
| **Storage** | **Standard LRS** | ~$5.00 | Encrypted storage for media/documents. |
| **Security** | **Key Vault (Standard)** | ~$1.00 | Managing secrets and encryption keys. |
| **Monitoring** | **Log Analytics (Pay-as-you-go)** | ~$10.00 | Essential audit logs ingestion (~2-3GB/mo). |
| **Total** | | **~$64.00 USD** | |

### Trade-offs
- **No Automatic Scaling**: Requires manual intervention if traffic spikes.
- **No Dedicated Cache**: Relies on in-memory app cache; effective for low volume.
- **Basic WAF**: Relies on basic network rules; no advanced DDoS protection (Front Door).

---

## 2. Production Architecture (Growth Phase)
**Target Cost:** ~$250 - $300 USD / month
**Trigger:** > 500 active users, > 50 concurrent requests/sec, or need for global acceleration.

| Service | SKU / Configuration | Estimated Cost | Upgrade Benefit |
| :--- | :--- | :--- | :--- |
| **App Service** | **Standard S1** | ~$73.00 | Auto-scale, Staging Slots (Zero-downtime deploy), Daily Backups. |
| **Database** | **PostgreSQL Flexible (B2ms)** | ~$65.00 | 2x CPU/RAM. Better performance for complex queries. |
| **CDN / WAF** | **Azure Front Door** | ~$35.00 + usage | Global WAF (OWASP rules), DDoS protection, latency reduction. |
| **Cache** | **Redis Cache (Basic/Std)** | ~$41.00 | Distributed caching for session/data offloading. |
| **Total** | | **~$244.00+ USD** | |

---

## 3. Cost Optimization Tactics

### Reserved Instances (RI)
- **Strategy**: Once traffic stabilizes (post-launch month 3-6), purchase 1-year or 3-year reservations for:
  - **App Service**: ~30-50% savings.
  - **Database**: ~40-60% savings.

### Log Retention Policies
- **Audit Logs**: Move logs older than 90 days from "Hot Analytics" to "Cold Storage" (Blob Archive tier) to reduce ingestion/storage costs significantly while meeting retention regulations.

### Development Environment
- **Local-First**: Developers use Docker for DB and Redis locally.
- **Spot Instances**: Use Azure Spot instances for non-production CI/CD runners if applicable.

## 4. Compliance Non-Negotiables
Regardless of the tier (MVP or Production), we **never compromise** on:
1.  **VNET Integration**: Database must never have a public IP.
2.  **Encryption**: At rest (Storage/DB) and in transit (TLS 1.2+).
3.  **Backups**: Automated daily backups (Postgres default) are maintained.
