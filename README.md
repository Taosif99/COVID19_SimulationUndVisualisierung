# DE: Simulation und Visualisierung
 COVID-19 Simulation Projekt

## Installation & Start

**Windows:**
- Die Anwendung als .zip von [hier](TODO) herunterladen
- Die .zip Datei entpacken
- SimulationUndVisualisierung.exe ausführen

**Andere Betriebssysteme:**

Leider stellen wir aktuell keine Versionen für andere Betriebssysteme zur Verfügung,
allerdings kann der Source Code mit Unity geöffnet werden um diese gegebenenfalls selbst zu erstellen.

## Bedienung
- Die Kamera kann mit den WASD oder Pfeiltasten bewegt werden
- Die Sicht lässt sich mit dem Mausrad vergrößern und verkleinern
- Bildschirmaufnahmen können mit der Leertaste aufgenommen werden (Speicherort: siehe Wichtige Verzeichnisse)

## Wichtige Verzeichnisse
Alle Daten der Anwendung sind unter folgendem Dateipfad zu finden:

- Windows: `%USERPROFILE%/AppData/LocalLow/DefaultCompany/SimulationUndVisualisierung` (Im Explorer oder über `Windows-Taste + R` eingeben)
- Für andere Betriebsysteme, siehe [persistantDataPath der UnityAPI](https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html)

In diesem Verzeichnis befinden sich folgende Unterverzeichnisse:
- Gespeicherte Simulationen: `SavedSimulations/`
- Bildschirmaufnahmen: `Screenshots/`
- .CSV Log Dateien: `SimulationLogs/`

## Für Interessierte (Source-Code)

### Primäre Simulations Klassen/Dateien 
- .../Assets/Scripts/Simulation/Runtime/Venue.cs
- .../Assets/Scripts/Simulation/Runtime/Person.cs
- .../Assets/Scripts/Simulation/Runtime/HealthState.cs
- .../Assets/Scripts/Simulation/Runtime/SimulationController.cs
- .../Assets/Scripts/Simulation/Runtime/DefaultInfectionParameters.cs
- .../Assets/Scripts/EpidemiologicalCalculation/EpidemiologicalCalculator.cs

---

# EN: Simulation and Visualization
COVID-19 Simulation Project

## Installation & Start

**Windows:**
- Download the application .zip file from [here](TODO)
- Unpack the .zip file
- Run SimulationUndVisualisierung.exe

**Other Operating Systems:**

Unfortunately we do not offer versions for other operating systems at the moment. However, the source code can be opened with Unity to create one yourself, if necessary.

## Controls
- The camera can be moved with the WASD or arrow keys
- The view is adjustable by using the mousewheel to zoom in or out
- Screenshots can be taken using the spacebar (File location: see Important Directories)

## Important directories
All application data can be found under the following file path:

- Windows: `%USERPROFILE%/AppData/LocalLow/DefaultCompany/SimulationUndVisualisierung` (Enter in Explorer or via `Windows-Key + R`)
- For other operating systems, see [persistantDataPath der UnityAPI](https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html)

In this directory you can find the following subdirectories:
- Saved Simulations: `SavedSimulations/`
- Screenshots: `Screenshots/`
- .CSV Log Files: `SimulationLogs/`

## For Interested People (Source-Code)

### Primary Simulation Classes/Files
- .../Assets/Scripts/Simulation/Runtime/Venue.cs
- .../Assets/Scripts/Simulation/Runtime/Person.cs
- .../Assets/Scripts/Simulation/Runtime/HealthState.cs
- .../Assets/Scripts/Simulation/Runtime/SimulationController.cs
- .../Assets/Scripts/Simulation/Runtime/DefaultInfectionParameters.cs
- .../Assets/Scripts/EpidemiologicalCalculation/EpidemiologicalCalculator.cs