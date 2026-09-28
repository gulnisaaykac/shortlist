# Beacon — self-hosted uptime and status platform

Independent of the internship Library app. One product from CS through Kubernetes.

## What it is

You run **Beacon**. It watches websites and APIs you care about. If they fail, it opens an incident and can alert you. The public sees a **status page** (operational / degraded / down). You see a **control panel** (login, monitors, history).

This is a real product shape: API, workers, database, queue, reverse proxy, secrets, later many replicas on Kubernetes.

## Why this project

It forces every instructor heading:

- CS — processes, files, exit codes, concurrency (workers vs API)
- Network — DNS, TCP, HTTP, TLS, timeouts, status codes
- Cybersecurity — auth, password hashing, API keys, secrets, least privilege
- System administration — Linux, Postgres, Redis, nginx, systemd, logs, backups
- Cloud — VM, firewall/security groups, object storage, identity
- DevOps — Docker, Compose, CI
- Kubernetes — split services, probes, HPA, Ingress, Helm, GitOps

## What you will build (pieces)

1. **api** — users, monitors, incidents, history. JSON API.
2. **probe** — background worker. Actually checks targets (HTTP, TCP, DNS).
3. **alerter** — sends webhook (and later email) when a monitor fails or recovers.
4. **web-public** — status page, no login. Must look good; this is the demo.
5. **web-admin** — login, CRUD monitors, view results.
6. **postgres** — source of truth.
7. **redis** — job queue between api and probe/alerter.
8. **edge** — later nginx or Ingress TLS.

You do not start with all eight. You grow into them.

## How a check works

1. You save a monitor: `https://example.com/health`, every 60s, HTTP GET.
2. API enqueues a job on Redis.
3. Probe dequeues, resolves DNS, connects, speaks HTTP, records latency and status.
4. Result is stored. If it flips from up to down, an incident opens and alerter fires.
5. Public status page reads latest state (no secrets).

## Data you will own (simple)

- users
- monitors (name, type, target, interval)
- results (when, ok, latency_ms, error)
- incidents (opened_at, resolved_at, summary)
- api_keys (hashed)

## Stack (recommended, not Library)

- API + probe + alerter: Python (FastAPI) or Go — pick one and stay
- DB: PostgreSQL
- Queue: Redis
- Public + admin UI: separate small frontend (or server-rendered first, UI later)
- Git from day one

## Phase by phase — what you *build* (not a lecture list)

**Phase 0 CS**  
CLI `beacon-check`: one URL, print ok/fail, write a result line to a file, process exit code. No Docker. No Kubernetes.

**Phase 1 Network**  
Same CLI grows: HTTP vs TCP vs DNS check, show resolved IP, port, TLS cert expiry, timeouts. You can explain the path of one request.

**Phase 2 Cybersecurity**  
Tiny API: register/login, hashed passwords, session or JWT, API keys hashed at rest, never log tokens, HTTPS in mind even if local HTTP first.

**Phase 3 System administration**  
Postgres + Redis + API as Linux services (systemd). nginx reverse proxy. Logs on disk. Backup Postgres. SSH-only admin.

**Phase 4 Cloud**  
Same stack on one cloud VM. Security group: 80/443 public, SSH yours only. Object storage for status-page logo. Know the bill.

**Phase 5 DevOps**  
Dockerfile per app. Compose: api, probe, alerter, postgres, redis, nginx. CI: test + image build. Compose is how you develop.

**Phase 6 Kubernetes**  
Each app a Deployment. Services. Ingress for public + admin. Secrets/ConfigMaps. Probe replicas (HPA later). Helm. Then GitOps. NetworkPolicy between api/probe/db.

## Out of scope until late

Mobile app, Kubernetes operators, service mesh, multi-region probes, SLOs as a product. Can grow after phase 6.

## Current heading

Phase 0 — CLI check. Closed until you say we start.
