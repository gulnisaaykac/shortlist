# Learning path: from CS to Kubernetes

Broad map, not a syllabus. Order follows the instructor: CS, network, cybersecurity, system administration, cloud computing, DevOps, then Kubernetes. One heading at a time. You ask what to learn now; we open that heading and apply it, internship-style.

## How we work

Do not binge this list. One open heading. Research + hands-on, then the next heading. No lecture dump until a heading is open.

## Spine project: Beacon

Independent of the internship Library app. Beacon is a self-hosted uptime and status platform (API, probe workers, alerts, public status page). See `beacon-project.md`. Early phases grow a CLI then a small API; Compose and Kubernetes come in phases 5-6.

## Phases (instructor order)

**0 - Computer science (foundations)**  
OS ideas: process, memory, files, CPU vs I/O. Enough programming comfort to read scripts and APIs.  
Done when: you can explain what a process, a file, and a port are without mixing them up.

**1 - Network**  
TCP/IP, DNS, HTTP/HTTPS, ports, routing vs switching at a practical level, firewalls.  
Done when: you can trace a request from browser to a server (name, IP, port, TLS).

**2 - Cybersecurity**  
CIA triad, identity and access, least privilege, TLS certificates as a concept, common failure modes (weak secrets, open ports, over-permission).  
Done when: you default to "who can do what" and "what is secret" before you deploy anything.

**3 - System administration**  
Linux daily: users, permissions, systemd/services, packages, logs, SSH, disk, networking on the box.  
Done when: you can run and debug a service on a Linux machine without a GUI.

**4 - Cloud computing**  
IaaS vs PaaS vs SaaS, regions, VMs, identity (IAM), object storage, one provider at a glance (AWS or Azure or GCP).  
Done when: you can place a VM and a network in a cloud and say what you are paying for.

**5 - DevOps**  
Git, CI/CD idea, containers (Docker: images, Compose, ship), infra as code as a concept.  
Done when: Library (or a small app) builds as an image and runs with Compose; a pipeline idea is clear.

**6 - Kubernetes**  
Apps on a cluster: workloads, config, probes, services, ingress; then storage, Helm, operate, GitOps, security, cluster admin, one managed cloud.  
Done when: you treat the cluster as how the app runs in production, not as a YAML museum.

## Kubernetes (phase 6) headings only

6A Kubernetes apps - workloads, config, probes, services, ingress  
6B Workloads and storage - Jobs, StatefulSets, PV/PVC  
6C Helm  
6D Operate - debug, metrics, logs  
6E Delivery - CI + GitOps  
6F Security on the cluster  
6G Clusters - kind/kubeadm  
6H Managed Kubernetes - AKS or EKS or GKE  

Out of scope until after 6: service mesh, writing operators, Terraform-everything, CKS cram.

## Current heading

Phase 0 - Computer science foundations. Closed until you say we start.
