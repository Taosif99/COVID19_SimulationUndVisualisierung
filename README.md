# SimulationUndVisualisierung / SimulationAndVisualization
 COVID-19 Simulation Projekt / COVID-19 Simulation Project

# DE
## Programm Ausführen
### TODO
## Bedienung
* In der Welt kann man sich mit den w/a/s/d oder Pfeiltasten bewegen
* Die Kamera lässt sich mit dem Scrollrad der Maus bewegen (Rein-und Rauszoomen möglich)
* Screenshots werden mit der Space-Taste aufgenommen
## Wichtige Verzeichnisse (Windows, einfach)
* Dateien sind gespeichert in: C:\Users\YourUserName\AppData\LocalLow\DefaultCompany\SimulationUndVisualisierung
* Die Simulationskonfigurationen werden als .covidSim-Binärdateien in C:\Users\YourUserName\AppData\LocalLow\DefaultCompany\SimulationUndVisualisierung\SavedSimulations gespeichert
* Die Screenshots werden in C:\Users\YourUserName\AppData\LocalLow\DefaultCompany\SimulationUndVisualisierung\Screenshots gespeichert
* Simulations csv-log-Files werden in C:\Users\YourUserName\AppData\LocalLow\DefaultCompany\SimulationUndVisualisierung\SimulationLogs gespeichert
## Wichtige Verzeichnisse (Allgemein, für alle Betriebssysteme)
Schaue im persistantDataPath der [UnityAPI](https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html) deines Betriebssystemes nach, dort sind folgende Ordner vorhanden:
* SavedSimualtions in der Simulationskonfigurationen als .covidSim-Binärdateien gespeichert wird
* Screenshots in der die screenshots gespeichert werden
* SimulationLogs in der die csv-Log-Files gespeichert werden
## Wo die Simualtionslogik hauptsächlich implementiert ist (Für Interessierte)
* ...\Assets\Scripts\Simulation\Runtime\Venue.cs
* ...\Assets\Scripts\Simulation\Runtime\Person.cs
* ...\Assets\Scripts\Simulation\Runtime\HealthState.cs
* ...\Assets\Scripts\Simulation\Runtime\SimulationController.cs
* ...\Assets\Scripts\Simulation\Runtime\DefaultInfectionParameters.cs
* ...\Assets\Scripts\EpidemiologicalCalculation\EpidemiologicalCalculator.cs

# EN
## Run Program
### TODO
## Operation
* You can move in the world with the w/a/s/d or arrow keys.
* The camera can be moved with the scroll wheel of the mouse (zooming in and out possible)
* Screenshots are taken with the space key
## Important directories (Windows,simple)
* Files are stored in: C:\Users\YourUserName\AppData\LocalLow\DefaultCompany\SimulationUndVisualisierung
* The simulation configurations are stored as .covidSim binaries in C:\Users\YourUserName\AppData\LocalLow\DefaultCompany\SimulationUndVisualisierung\SavedSimulations.
* The screenshots are stored in C:\Users\YourUserName\AppData\LocalLow\DefaultCompany\SimulationUndVisualisierung\Screenshots.
* Simulation csv-log files are stored in C:\Users\YourUserName\AppData\LocalLow\DefaultCompany\SimulationUndVisualisierung\SimulationLogs
## Important directories (General, for all operating systems)
Look in the persistantDataPath of the [UnityAPI](https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html) for your operating system, there it must contain the folders:
* SavedSimualtions where simulation configurations are stored as .covidSim binaries
* Screenshots where the screenshots are stored
* SimulationLogs where the csv log files are stored.
## Where mainly the simulation logic is implemented/most important files (For interested people)
* ...\Assets\Scripts\Simulation\Runtime\Venue.cs
* ...\Assets\Scripts\Simulation\Runtime\Person.cs
* ...\Assets\Scripts\Simulation\Runtime\HealthState.cs
* ...\Assets\Scripts\Simulation\Runtime\SimulationController.cs
* ...\Assets\Scripts\Simulation\Runtime\DefaultInfectionParameters.cs
* ...\Assets\Scripts\EpidemiologicalCalculation\EpidemiologicalCalculator.cs





