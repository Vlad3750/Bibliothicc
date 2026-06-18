# Bibliothicc

**Klasse:** 3AHIF · **Schuljahr:** 2025/26  
**Team:** Talha Zengin & Vladislav Neicovcen

**Bibliothicc** ist eine Full-Stack Medienbibliothek-Anwendung, die es Benutzern ermöglicht, ihre persönlichen Medien — wie Videos, Musik, Bilder und Dokumente — in organisierten Bibliotheken zu verwalten, zu kategorisieren und zu teilen.

---

## Github Repo

- Frontend: https://github.com/Vlad3750/Bibliothicc
- Backend: https://github.com/Vlad3750/Bibliothicc_Backend

---

## Rollen

| Rolle | Rechte |
|---|---|
| **User** | Login/Registrierung, Bibliotheken/Medien/Kategorien erstellen, ändern und löschen, Bibliotheken veröffentlichen, öffentliche Bibliotheken anderer User browsen und herunterladen |
| **Admin** | Alles was ein User kann + Übersicht aller veröffentlichten Bibliotheken mit Besitzer, Veröffentlichung von Bibliotheken aufheben |

---

## Voraussetzungen

### Entwicklung

| Software | Version |
|---|---|
| Windows | 10 / 11 (64-bit) |
| Visual Studio 2022 | ≥ 17.10 (für `.slnx`-Format) |
| .NET SDK | 10.0 |
| Python | 3.11+ |

### Laufzeit (Endnutzer)

| Komponente | Version |
|---|---|
| Windows | 10 / 11 (64-bit) |
| .NET Desktop Runtime | 10.0 |

### NuGet-Pakete

Keine NuGet-Pakete wurden in diesem Projekt verwendet.

### Python-Pakete (Backend)

| Paket | Verwendung |
|---|---|
| FastAPI | REST-API Framework |
| SQLAlchemy | ORM für SQLite |
| Uvicorn | ASGI-Server |
| fastapi-restful | CBV-Pattern für Router |
| Pydantic | Request/Response-Validierung |

---

## Architektur

- **Frontend** kommuniziert ausschließlich über `ILibraryService` mit dem Backend
- **LibraryServiceRest** implementiert alle HTTP-Aufrufe (JSON + Multipart für Datei-Upload)
- **LibraryServiceFake** ermöglicht lokales Testen ohne Backend
- **Backend** folgt dem CBV-Pattern mit FastAPI-Routern pro Ressource

### Ordnerstruktur Frontend

Bibliothicc/

├── Models/                  # User, Library, Media, Category
├── Services/
│   ├── ILibraryService.cs   # Interface für alle Service-Operationen
│   ├── LibraryServiceRest.cs  # HTTP-Implementierung
│   ├── LibraryServiceFake.cs  # Lokale Fake-Implementierung zum Testen
│   └── Logger.cs            # Einfaches Logging
├── Views/                   # MainWindow, LoginRegisterWindow, BrowseWindow, …
└── App.xaml                 # Globale Styles & Themes (Dark/Light Mode)

### Ordnerstruktur Backend

Bibliothicc_Backend/

├── routers/
│   ├── user.py
│   ├── library.py
│   ├── library_collection.py
│   ├── media.py
│   ├── category.py
│   └── category_per_media.py
├── models.py                # SQLAlchemy-Modelle
├── database.py              # DB-Verbindung & Session
└── main.py                  # FastAPI-App, Router-Registrierung, CORS

---

## Features

### User
- Login & Registrierung
- Bibliotheken erstellen (Video, Film, Musik, Bild, Text) und löschen
- Medien hinzufügen, bearbeiten und löschen
- Eigene Kategorien/Tags pro User verwalten und Medien damit markieren
- Suche und Filterung nach Kategorie in Echtzeit
- Bibliotheken veröffentlichen / unpublishen
- Öffentliche Bibliotheken anderer User browsen und Dateien herunterladen
- Medien direkt mit dem System-Standardprogramm abspielen/öffnen
- Dark Mode & Light Mode

### Admin
- Alle veröffentlichten Bibliotheken mit Besitzer-Name einsehen
- Bibliotheken bei Regelverstoß unpublishen

### Technisch
- Grid-Ansicht und Listen-Ansicht für Medien umschaltbar
- Thumbnail-Vorschau in der Grid-Ansicht
- Datei-Download vom Server in temp-Verzeichnis vor dem Öffnen
- Dark/Light Mode wechselt live alle Farben ohne Neustart
- Vollständig eigenes WPF-Design (keine externen UI-Bibliotheken)

---

## Bekannte Probleme & Lösungen

| Problem | Lösung |
|---|---|
| `SolidColorBrush` aus XAML ist eingefroren (Freeze) → Theme-Wechsel schlägt fehl | `PresentationOptions:Freeze="False"` + direkte Mutation via `brush.Color =` |
| `DynamicResource` in `BasedOn` nicht erlaubt | `BasedOn` bleibt `StaticResource`, nur Trigger-Setter auf `DynamicResource` umgestellt |
| ComboBox Dropdown behält System-Standard-Hintergrund | Vollständiges `ControlTemplate` mit eigenem `<Popup>` und `<Border>` nötig |
| `ToggleButton` im ComboBox-Template zeigt hässlichen Hover-Effekt | Eigenes leeres `ControlTemplate` für den `ToggleButton` |
| `ListViewItem` nach Umbau auf Media-Binding — Delete/Change per Index falsch | Direkt `(Media)ListViewFiles.SelectedItem` casten statt per Index zugreifen |
| `SizeToContent="Height"` startet Fenster zu groß | `MaxHeight` gesetzt um Maximalgröße zu begrenzen |
| Kategorien nach Umbenennung noch als Tag sichtbar | Tag wird beim Umbenennen automatisch aus `ListViewCategoriesToAdd` entfernt |
| Zwei ListView für Kategorien schwer zu synchronisieren: `ListViewSystemCategories` (alle Kategorien des Users) und `ListViewCategoriesToAdd` (zugewiesene Tags eines Mediums) müssen konsistent bleiben | `CategoryItem`-Klasse mit `Name` und `Symbol` (`✓`/`○`) — beim Öffnen von `CategoriesWindow` wird geprüft welche Kategorien bereits im Medium sind und das Symbol entsprechend gesetzt; Doppelklick toggelt den Zustand und fügt gleichzeitig in `ListViewCategoriesToAdd` ein bzw. entfernt daraus |
| `ListViewCategoriesToAdd` zeigt nach Änderungen veraltete Daten | `Items.Refresh()` reicht nicht ohne `INotifyPropertyChanged` — stattdessen das Item entfernen und neu einfügen um WPF zum Neurendern zu zwingen |

---

## KI-Verwendung

Folgende Teile wurden mit KI-Unterstützung (Claude) erstellt und sind im Code mit `// AI (Claude)` gekennzeichnet:

- `RefreshFileList()` — Echtzeit-Suche und Kategorie-Filterung
- `RefreshCategoryComboBox()` — dynamische Kategorie-ComboBox
- `CreateGridCard()` — Grid-Karten mit Thumbnail-Vorschau
- `CategoriesWindow` — Kategorienverwaltung mit Haken/Kreis-System
- Backend-Router (`library.py`, `media.py`, `category.py`, `category_per_media.py`, `library_collection.py`) — vollständige Implementierung mit Validierung

---

## Projekttagebuch

| Datum | Was wurde gemacht | Wer |
|---|---|---|
| 15.05.2026 | Projektidee, Anforderungen, Technologiestack gewählt | Talha & Vladislav |
| 19.05.2026 | Datenmodell, GUI-Skizzen | Talha & Vladislav |
| 20.05.2026 | WPF-Grundgerüst, Models, LoginWindow | Talha |
| 21.05.2026 | MainWindow, Library- und Media-Verwaltung | Talha |
| 22.05.2026 | FastAPI Backend Grundgerüst, Models, Datenbank | Vladislav |
| 26.05.2026 | Backend-Router: User, Library, Media | Vladislav |
| 27.05.2026 | ILibraryService Interface, LibraryServiceFake | Vladislav |
| 28.05.2026 | Eigenes WPF-Design (Dark Theme, Button/TextBox-Styles) | Talha |
| 02.06.2026 | Kategorien-System, CategoriesWindow | Talha |
| 03.06.2026 | **Zwischenpräsentation** | Talha & Vladislav |
| 04.06.2026 | LibraryServiceRest, HTTP-Anbindung ans Backend | Vladislav |
| 05.06.2026 | Suche & Kategorie-Filter | Talha |
| 09.06.2026 | Light Mode / Dark Mode Toggle | Talha |
| 11.06.2026 | BrowseWindow, öffentliche Bibliotheken | Talha & Vladislav |
| 12.06.2026 | Publish/Unpublish | Vladislav |
| 13.06.2026 | Datei-Upload, Download, DuckDNS | Vladislav |
| 13.06.2026 | Play-Button | Talha |
| 18.06.2026 | **Endpräsentation / Abgabe** | Talha & Vladislav |
| 18.06.2026 | Admin-BrowseWindow | Talha |
| 18.06.2026 | Bugfixes, Grid-Ansicht mit Thumbnail-Vorschau, Admin-Rolle | Vladislav |


