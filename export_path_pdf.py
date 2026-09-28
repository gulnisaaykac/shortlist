from pathlib import Path

from fpdf import FPDF

OUT = Path(__file__).resolve().with_name("cs-to-kubernetes-learning-path.pdf")


class PathPDF(FPDF):
    def header(self) -> None:
        self.set_x(self.l_margin)
        self.set_font("Helvetica", "B", 9)
        self.set_text_color(90, 90, 90)
        self.cell(
            0,
            8,
            "Learning path: CS to Kubernetes",
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
    pdf.set_font("Helvetica", "B", 14)
    pdf.set_text_color(20, 20, 20)
    pdf.ln(2)
    pdf.multi_cell(0, 8, text)
    pdf.ln(1)


def body(pdf: PathPDF, text: str) -> None:
    pdf.set_x(pdf.l_margin)
    pdf.set_font("Helvetica", "", 11)
    pdf.set_text_color(35, 35, 35)
    pdf.multi_cell(0, 6, text)
    pdf.ln(2)


def phase(pdf: PathPDF, title: str, topics: str, done: str) -> None:
    pdf.set_x(pdf.l_margin)
    pdf.set_font("Helvetica", "B", 11)
    pdf.set_text_color(20, 20, 20)
    pdf.multi_cell(0, 6, title)
    pdf.set_x(pdf.l_margin)
    pdf.set_font("Helvetica", "", 11)
    pdf.set_text_color(35, 35, 35)
    pdf.multi_cell(0, 6, topics)
    pdf.set_x(pdf.l_margin)
    pdf.set_font("Helvetica", "I", 10)
    pdf.set_text_color(70, 70, 70)
    pdf.multi_cell(0, 6, f"Done when: {done}")
    pdf.ln(3)


def main() -> None:
    pdf = PathPDF(format="A4")
    pdf.set_auto_page_break(auto=True, margin=18)
    pdf.set_margins(18, 16, 18)
    pdf.add_page()

    pdf.set_x(pdf.l_margin)
    pdf.set_font("Helvetica", "B", 20)
    pdf.set_text_color(15, 15, 15)
    pdf.multi_cell(0, 9, "Learning path: from CS to Kubernetes")
    pdf.ln(2)
    body(
        pdf,
        "Broad map, not a syllabus. Order follows the instructor: CS, network, "
        "cybersecurity, system administration, cloud computing, DevOps, then "
        "Kubernetes. One heading at a time.",
    )

    heading(pdf, "How we work")
    body(
        pdf,
        "You ask \"what do I learn now?\". We open a single heading and apply it "
        "(internship style). No lecture dump until that heading is open. "
        "The Library app (API + UI on Minikube) is the spine from DevOps/Kubernetes "
        "onward. Earlier phases use small labs.",
    )

    heading(pdf, "Phases (instructor order)")
    phase(
        pdf,
        "0 - Computer science (foundations)",
        "OS ideas: process, memory, files, CPU vs I/O. Enough programming comfort to read scripts and APIs.",
        "You can explain what a process, a file, and a port are without mixing them up.",
    )
    phase(
        pdf,
        "1 - Network",
        "TCP/IP, DNS, HTTP/HTTPS, ports, routing vs switching at a practical level, firewalls.",
        "You can trace a request from browser to a server (name, IP, port, TLS).",
    )
    phase(
        pdf,
        "2 - Cybersecurity",
        "CIA triad, identity and access, least privilege, TLS as a concept, common failure modes (weak secrets, open ports, over-permission).",
        "You default to who can do what and what is secret before you deploy anything.",
    )
    phase(
        pdf,
        "3 - System administration",
        "Linux daily: users, permissions, systemd/services, packages, logs, SSH, disk, networking on the box.",
        "You can run and debug a service on a Linux machine without a GUI.",
    )
    phase(
        pdf,
        "4 - Cloud computing",
        "IaaS vs PaaS vs SaaS, regions, VMs, IAM, object storage, one provider at a glance (AWS or Azure or GCP).",
        "You can place a VM and a network in a cloud and say what you are paying for.",
    )
    phase(
        pdf,
        "5 - DevOps",
        "Git, CI/CD idea, containers (Docker: images, Compose, ship), infra as code as a concept.",
        "Library (or a small app) builds as an image and runs with Compose; a pipeline idea is clear.",
    )
    phase(
        pdf,
        "6 - Kubernetes",
        "Apps on a cluster, then storage, Helm, operate, GitOps, cluster security, cluster admin, one managed cloud.",
        "You treat the cluster as how the app runs in production, not as a YAML museum.",
    )

    heading(pdf, "Kubernetes (phase 6) headings only")
    body(
        pdf,
        "6A Kubernetes apps. 6B Workloads and storage. 6C Helm. 6D Operate. "
        "6E Delivery (CI + GitOps). 6F Security on the cluster. 6G Clusters (kind/kubeadm). "
        "6H Managed Kubernetes (AKS or EKS or GKE). "
        "Later: service mesh, operators, Terraform-everything, CKS cram.",
    )

    heading(pdf, "Current heading")
    body(
        pdf,
        "Phase 0 - Computer science foundations. Closed until you say we start.",
    )

    pdf.output(OUT)


if __name__ == "__main__":
    main()
