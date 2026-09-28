# Shortlist

Private tracker for internships and jobs: company, role, status. Later: reminders, files, login, Linux, cloud, Docker, Kubernetes.

Phase 0 is a .NET console app. Data stays in `Shortlist/data/applications.json` (not committed).

```text
cd Shortlist
dotnet run -- add --company "Acme" --role "Intern"
dotnet run -- list
dotnet run -- status --id <id> --to waiting
```

Valid `--to`: applied, waiting, interview, offer, rejected.
