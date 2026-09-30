# 🌍 Traversal – Full‑Stack Entwicklung

> **Live-Demo ohne Registrierung:** `/Demo/Admin` (Admin-Panel) · `/Demo/Member` (Kundenbereich)
> Die Demo-Datenbank wird automatisch alle 6 Stunden zurückgesetzt.

## 🚀 Schnellstart

```bash
git clone https://github.com/amirafshar2/TraversalCoreProject.git
cd TraversalCoreProject/TraversalCoreProje
dotnet run
```

- Keine Datenbank-Installation nötig: **SQLite** liegt im Projekt unter `TraversalCoreProje/App_Data/traversal.db`.
- Beim Start werden fehlende Migrationen automatisch angewendet. Ist die Datenbank leer, werden Demo-Daten aus `App_Data/seed/seed-data.json` eingespielt.
- Demo-Konten: `admin@traversal.demo` / `gast@traversal.demo`, Passwort `Demo123!`.
- Einstellungen in `appsettings.json` → Abschnitt `Demo` (Demo-Modus, Reset-Intervall, Portfolio-/Impressum-Links).
- KI-Assistent (optional): `dotnet user-secrets set "Gemini:ApiKey" "<schlüssel>"` – der Schlüssel gehört **nie** in den Code.

### Deployment (Render, kostenlos)
`Dockerfile` und `render.yaml` sind vorhanden: Repository in Render als *Blueprint* verbinden → Region Frankfurt, Plan *Free*.

### Neue Migration anlegen
```bash
dotnet ef migrations add <Name> --project DataAccessLayer --startup-project TraversalCoreProje
```

---

Traversal ist ein Reiseverwaltungs‑ und Buchungssystem, das ursprünglich nur aus einem statischen UI‑Template bestand.  
Ich habe dieses Template vollständig in ein **funktionsfähiges, mehrschichtiges System** verwandelt – inklusive Datenbank, Backend‑Logik, Admin‑Dashboard, Reservierungssystem und dynamischen Reisezieleinträgen.

---

## 👨‍💻 Meine Rolle als Entwickler  
Das Projekt hatte nur ein fertiges Frontend‑Design.  
**Alle technischen Funktionen, Datenstrukturen und Verwaltungsbereiche wurden komplett von mir entwickelt.**

### ✔️ Was bereits vorhanden war
- Ein statisches HTML/CSS‑UI  
- Visuelle Layouts ohne Funktionalität  
- Keine Datenbank, keine Logik, keine Admin‑Seiten

### ✔️ Was ich selbst entwickelt habe
Ich habe das gesamte System technisch aufgebaut:

---

## 🏗️ 1. Architektur & Backend‑Struktur  
Ich habe die komplette technische Basis implementiert:

- Mehrschichtige Architektur (Entity, DAL, BLL, UI)
- Repository‑Pattern
- Dependency Injection
- Datenbankmodellierung mit EF Core Code-First (SQLite, früher SQL Server)
- Vollständige CRUD‑Funktionen
- Erweiterbare, saubere Code‑Struktur

---

## 🖥️ 2. Frontend‑Integration  
Das UI war vorhanden, aber ich habe:

- Alle Seiten dynamisch gemacht  
- Datenbindung implementiert  
- Reiseziele, Blogartikel und Kommentare aus der Datenbank geladen  
- Reservierungsformulare mit Backend verbunden  
- Validierungen hinzugefügt  
- Benutzerprofil‑Seiten und Passwort‑Update integriert

---

## 🛠️ 3. Admin‑Dashboard (komplett von mir entwickelt)  
Das Admin‑Panel existierte nicht – ich habe es vollständig selbst gebaut:

### 📊 Dashboard
- Statistiken für Besucher, Kunden, Reiseziele und Reservierungen  
- Dynamische Karten und Tabellen

### 🧭 Reiseziele‑Management
- Reiseziele hinzufügen, bearbeiten, löschen  
- Bilder hochladen  
- Status (aktiv/inaktiv)  
- Vorschau‑Funktion  
- Preis, Dauer, Beschreibung, Kategorien

### 🗂️ Reservierungssystem
- Ausstehende Reservierungen  
- Bestätigte Reservierungen  
- Stornierte Reservierungen  
- Neue Reservierungen  
- Pagination

### 💬 Kommentar‑Management
- Kommentare anzeigen  
- Aktiv/Inaktiv  
- Löschen  
- Zugehöriges Reiseziel anzeigen

### 🧭 Reiseführer, Inhalte, Nachrichten & Newsletter
- Reiseführer verwalten und Reservierungen zuweisen
- Startseiten-Inhalte (Banner, Top-Angebote, Kundenstimmen), „Über uns“ und Kontaktdaten pflegen
- Kontaktanfragen lesen, Newsletter-Abonnenten als CSV exportieren

### 👤 Benutzer‑Management
- Benutzer hinzufügen  
- Profilbild  
- Kommentare des Benutzers  
- Bearbeiten/Löschen

### 🔐 Rollen & Berechtigungen
- Rollen: User, Moderator, Admin  
- Aktiv/Inaktiv  
- Bearbeiten/Löschen

---

## 🧩 4. Technologien, die ich verwendet habe
- ASP.NET Core MVC (.NET 8)  
- Entity Framework Core  
- SQLite (EF Core Migrationen)  
- ASP.NET Core Identity (Rollen: Admin, Moderator, Member)  
- FluentValidation, X.PagedList  
- Docker / Render  
- Repository‑Pattern  
- Dependency Injection  
- HTML / CSS / JavaScript  
- Bootstrap  
- C#  

---

## 🖼️ 5. Screenshots  
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
**Amir Reza AFSHAR**  
GitHub: https://github.com/amirafshar2

