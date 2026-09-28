from pathlib import Path

from fpdf import FPDF

OUT = Path(__file__).resolve().with_name("beacon-project.pdf")


class PathPDF(FPDF):
    def header(self) -> None:
        self.set_x(self.l_margin)
        self.set_font("Helvetica", "B", 9)
        self.set_text_color(90, 90, 90)
        self.cell(
            0,
            8,
            "Beacon - project brief",
            align="L",
            new_x="LMARGIN",
            new_y="NEXT",
        )
        self.ln(4)

    def footer(self) -> None:
        self.set_y(-14)
        self.set_font("Helvetica", "", 8)
        self.set_text_color(120, 120, 120)
        self.cell(0, 8, f"{self.page_no()}", align="R")


def heading(pdf: PathPDF, text: str) -> None:
    pdf.set_x(pdf.l_margin)
    pdf.set_font("Helvetica", "B", 13)
    pdf.set_text_color(20, 20, 20)
    pdf.ln(2)
    pdf.multi_cell(0, 7, text)
    pdf.ln(1)


def body(pdf: PathPDF, text: str) -> None:
    pdf.set_x(pdf.l_margin)
    pdf.set_font("Helvetica", "", 10)
    pdf.set_text_color(35, 35, 35)
    pdf.multi_cell(0, 5.5, text)
    pdf.ln(1.5)


def main() -> None:
    pdf = PathPDF(format="A4")
    pdf.set_auto_page_break(auto=True, margin=18)
    pdf.set_margins(18, 16, 18)
    pdf.add_page()

    pdf.set_x(pdf.l_margin)
    pdf.set_font("Helvetica", "B", 20)
    pdf.multi_cell(0, 9, "Beacon")
    pdf.set_x(pdf.l_margin)
    pdf.set_font("Helvetica", "I", 11)
    pdf.set_text_color(70, 70, 70)
    pdf.multi_cell(0, 6, "Self-hosted uptime and status platform")
    pdf.ln(3)
    pdf.set_x(pdf.l_margin)
    pdf.set_text_color(35, 35, 35)

    body(
        pdf,
        "Independent of the internship Library app. One product from CS through Kubernetes.",
    )

    heading(pdf, "What it is")
    body(
        pdf,
        "You run Beacon. It watches websites and APIs. If they fail, it opens an incident "
        "and can alert you. The public sees a status page (operational / degraded / down). "
        "You see a control panel (login, monitors, history).",
    )

    heading(pdf, "Pieces you will build")
    body(
        pdf,
        "api - users, monitors, incidents, history. probe - worker that checks HTTP/TCP/DNS. "
        "alerter - webhook (later email). web-public - status page, no login. web-admin - login and CRUD. "
        "postgres - source of truth. redis - job queue. edge - nginx or Ingress later. "
        "You do not start with all of these; you grow into them.",
    )

    heading(pdf, "How a check works")
    body(
        pdf,
        "1. Save a monitor (URL, interval, HTTP GET). 2. API enqueues a job on Redis. "
        "3. Probe dequeues, resolves DNS, connects, records latency and status. "
        "4. If up flips to down, an incident opens and alerter fires. "
        "5. Public status page reads latest state (no secrets).",
    )

    heading(pdf, "Data")
    body(
        pdf,
        "users; monitors (name, type, target, interval); results (when, ok, latency_ms, error); "
        "incidents; api_keys (hashed).",
    )

    heading(pdf, "Stack")
    body(
        pdf,
        "API + probe + alerter: Python (FastAPI) or Go - pick one and stay. PostgreSQL. Redis. "
        "Small separate UI (or server-rendered first). Git from day one.",
    )

    heading(pdf, "Phase by phase - what you build")
    body(
        pdf,
        "Phase 0 CS: CLI beacon-check - one URL, ok/fail, write a result line to a file, exit code. No Docker, no Kubernetes.",
    )
    body(
        pdf,
        "Phase 1 Network: CLI grows - HTTP vs TCP vs DNS, resolved IP, port, TLS cert expiry, timeouts.",
    )
    body(
        pdf,
        "Phase 2 Cybersecurity: tiny API - register/login, hashed passwords, hashed API keys, never log tokens.",
    )
    body(
        pdf,
        "Phase 3 System administration: Postgres + Redis + API as systemd services, nginx, logs, Postgres backup, SSH-only admin.",
    )
    body(
        pdf,
        "Phase 4 Cloud: same on one cloud VM, security group (80/443 public, SSH yours), object storage for logo, know the bill.",
    )
    body(
        pdf,
        "Phase 5 DevOps: Dockerfiles, Compose for the full stack, CI test + image build.",
    )
    body(
        pdf,
        "Phase 6 Kubernetes: Deployments, Services, Ingress, Secrets/ConfigMaps, probe replicas, Helm, GitOps, NetworkPolicy.",
    )

    heading(pdf, "Later")
    body(
        pdf,
        "Not now: mobile app, operators, service mesh, multi-region probes.",
    )

    heading(pdf, "Current heading")
    body(pdf, "Phase 0 - CLI check. Closed until you say we start.")

    pdf.output(OUT)


if __name__ == "__main__":
    main()
