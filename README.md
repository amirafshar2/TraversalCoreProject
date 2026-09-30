# 🌍 Traversal – Reisebuchungs-Plattform mit ASP.NET Core

Traversal ist ein Reiseverwaltungs- und Buchungssystem auf Basis von **ASP.NET Core MVC (.NET 8)**.
Ausgangspunkt war ein rein statisches HTML-Template – daraus habe ich eine vollständige, **mehrschichtige Webanwendung** mit Datenbank, Geschäftslogik, Admin-Panel, Kundenbereich und Reservierungssystem entwickelt.

> 🎯 **Live-Demo ohne Registrierung**
> - **Admin-Panel:** `/Demo/Admin`
> - **Kundenbereich:** `/Demo/Member`
> - Übersicht: `/Demo`
>
> Alle Änderungen sind erlaubt – die Demo-Datenbank wird automatisch alle 6 Stunden zurückgesetzt.

---

## ✨ Funktionen

### 🌐 Öffentliche Website
- Startseite mit Schnellsuche, beliebten Reisezielen, Kennzahlen, Top-Angeboten, Reiseführern und Kundenstimmen
- Reiseziel-Liste mit Suche und Sortierung, Detailseite mit Bildern, Fakten und ähnlichen Reisezielen
- Bewertungen (Kommentare) per AJAX – nur für angemeldete Benutzer
- Seiten „Über uns“, „Reiseführer“ und „Kontakt“ (Kontaktformular mit Validierung)
- Newsletter-Anmeldung, Impressum-/Datenschutz-Hinweise, eigene 404-/403-Seiten
- Registrierung und Anmeldung (ASP.NET Core Identity)

### 🛠️ Admin-Panel (Rollen: Admin, teilweise Moderator)
- **Dashboard** mit Kennzahlen, Umsatz und Diagrammen (ApexCharts)
- **Reiseziele:** anlegen, bearbeiten, löschen, Sichtbarkeit per Schalter (AJAX), Bild-Upload mit Vorschau
- **Reservierungen:** filtern (ausstehend / bestätigt / storniert), bestätigen, stornieren, Reiseführer zuweisen
- **Kommentare:** freischalten, ausblenden, löschen (auch für Moderatoren)
- **Reiseführer:** CRUD mit FluentValidation
- **Benutzer & Rollen:** Benutzer bearbeiten/löschen, Rollen anlegen und zuweisen
- **Website-Inhalte:** „Über uns“, Startseiten-Banner, Vorteile, Kontaktdaten, Top-Angebote, Kundenstimmen
- **Nachrichten** aus dem Kontaktformular und **Newsletter** (CSV-Export)

### 🧳 Kundenbereich
- Übersicht mit Countdown zur nächsten Reise und Empfehlungen
- Neue Reservierung in zwei Schritten mit Live-Preisberechnung
- Eigene Reservierungen bearbeiten oder stornieren (mit Besitzprüfung)
- Eigene Kommentare verwalten, Profil und Passwort ändern

---

## 🏗️ Architektur

```
TraversalCoreProje.sln
├── EntityLayer          → Entitäten (Destiniton, Reservition, Comment, Guide, User, Roll, …)
├── DataAccessLayer      → EF Core Context, Migrationen, GenericRepository, Ef…DAL-Klassen
├── BusinessLayer        → Services (I…Service) & Manager, FluentValidation, DI-Registrierung
└── TraversalCoreProje   → ASP.NET Core MVC: Controller, Areas (Admin, Member), View Components
```

- **N-Tier-Architektur** mit klarer Trennung von Daten, Logik und Präsentation
- **Repository-Pattern** (`GenericRepository<T>`) und **generische Manager** (`GenericManager<T>`)
- **Dependency Injection** – ein `Context` pro Request (Scoped)
- **Areas** für Admin- und Kundenbereich, **View Components** für wiederverwendbare Bausteine
- **ASP.NET Core Identity** mit Rollen (Admin, Moderator, Member)

---

## 🧩 Technologien
- C#, ASP.NET Core MVC (.NET 8)
- Entity Framework Core (Code First, Migrationen)
- **SQLite** – die Datenbank liegt direkt im Projekt
- ASP.NET Core Identity, FluentValidation, X.PagedList
- HTML, CSS, JavaScript, jQuery, Bootstrap 4, ApexCharts
- Docker, Render

---

## 🚀 Lokal starten

```bash
git clone https://github.com/amirafshar2/TraversalCoreProject.git
cd TraversalCoreProject/TraversalCoreProje
dotnet run
```

- **Keine Datenbank-Installation nötig:** Die SQLite-Datenbank liegt unter `TraversalCoreProje/App_Data/traversal.db`.
- Beim Start werden fehlende Migrationen automatisch angewendet. Ist die Datenbank leer, werden Beispieldaten aus `App_Data/seed/seed-data.json` eingespielt.
- **Demo-Konten:** `admin@traversal.demo` (Admin) und `gast@traversal.demo` (Kunde), Passwort `Demo123!`
- Einstellungen in `appsettings.json` → Abschnitt `Demo` (Demo-Modus, Reset-Intervall, Portfolio- und Impressum-Links)

### Neue Migration anlegen
```bash
dotnet ef migrations add <Name> --project DataAccessLayer --startup-project TraversalCoreProje
```

### KI-Reiseassistent (optional)
Der Assistent nutzt Google Gemini. Der API-Schlüssel wird **nicht** im Code gespeichert:
```bash
dotnet user-secrets set "Gemini:ApiKey" "<Ihr-Schlüssel>" --project TraversalCoreProje
```

---

## ☁️ Deployment (Render, kostenlos)
`Dockerfile` und `render.yaml` sind enthalten:
1. Repository bei [Render](https://render.com) als **Blueprint** verbinden
2. Region *Frankfurt*, Plan *Free*
3. Optional die Umgebungsvariable `Gemini__ApiKey` setzen

Im Container wird die Demo-Datenbank bei jedem Start und danach alle 6 Stunden zurückgesetzt.

---

## 🔒 Sicherheit & Datenschutz
- Anti-Forgery-Token für alle Formulare und AJAX-Anfragen
- Upload-Prüfung: nur Bilddateien bis 5 MB
- Rollenbasierte Autorisierung; Kunden können nur ihre eigenen Reservierungen ändern
- Schriften und Skripte werden lokal ausgeliefert – keine Verbindung zu Google Fonts oder CDNs (DSGVO)
- Keine Geheimnisse im Quellcode

---

## 🖼️ Screenshots
<img width="1401" height="1031" alt="Screenshot 2026-07-15 152847" src="https://github.com/user-attachments/assets/3c97debd-830d-48f0-ace9-2a4675272b3b" />
<img width="1396" height="1035" alt="Screenshot 2026-07-15 152854" src="https://github.com/user-attachments/assets/b0de6ef9-9484-4584-85ce-1b39be9d1cc9" />
<img width="1407" height="911" alt="Screenshot 2026-07-15 152923" src="https://github.com/user-attachments/assets/337170cf-0d37-4537-ac98-da91674a2579" />
<img width="1396" height="931" alt="Screenshot 2026-07-15 152932" src="https://github.com/user-attachments/assets/f24c6f24-a17d-4472-80f7-f58975faec22" />
<img width="1390" height="933" alt="Screenshot 2026-07-15 152938" src="https://github.com/user-attachments/assets/974eceb2-f860-4479-a1b8-a56c3ecd9ea2" />
<img width="1882" height="1025" alt="Screenshot 2026-07-15 153032" src="https://github.com/user-attachments/assets/e9a2c8da-2deb-4e40-858b-f2c627e745ea" />
<img width="1888" height="1012" alt="Screenshot 2026-07-15 153130" src="https://github.com/user-attachments/assets/369553c0-58c1-401e-a9bc-6986ef2b69ed" />
<img width="1887" height="1027" alt="Screenshot 2026-07-15 153138" src="https://github.com/user-attachments/assets/f3be522a-41b4-45e2-83de-7dc60f8f2fea" />
<img width="1887" height="1025" alt="Screenshot 2026-07-15 153207" src="https://github.com/user-attachments/assets/c6f5b0ac-2e8a-4493-9235-2206f232d5da" />
<img width="1876" height="1032" alt="Screenshot 2026-07-15 153218" src="https://github.com/user-attachments/assets/8b2b6315-f6b4-4e43-b669-d70f9de43b01" />
<img width="1892" height="1025" alt="Screenshot 2026-07-15 153313" src="https://github.com/user-attachments/assets/659858f0-ee33-4e73-b40e-c9dfd6a533a4" />
<img width="1882" height="1030" alt="Screenshot 2026-07-15 153321" src="https://github.com/user-attachments/assets/e0511d0d-d253-4ce9-8838-fa2e63b716e8" />
<img width="1877" height="1026" alt="Screenshot 2026-07-15 153335" src="https://github.com/user-attachments/assets/aac6050f-7b4f-4642-a57c-246f85d3ba02" />
<img width="1876" height="1027" alt="Screenshot 2026-07-15 153343" src="https://github.com/user-attachments/assets/04d1c651-6e82-4781-b25b-bc469e6438b7" />

---

## 👤 Entwickler
**Amir Reza Afshar** – Umschulung zum Fachinformatiker für Anwendungsentwicklung
GitHub: https://github.com/amirafshar2 · Portfolio: https://amirrezaafshar.de
