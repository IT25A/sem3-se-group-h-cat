# SIMBA
Ein in Unity entwickeltes 2D-Adventure. Das Spiel fokussiert sich auf Exploration, Ressourcenmanagement und eine questbasierte Progression, um eine verirrte Katze durch verschiedene Etappen zurück nach Hause zu navigieren.

## Spielmechanik und Systemlogik

* **Narratives Intro:** Eine animierte Eingangssequenz etabliert die Prämisse: Die Spielfigur (Simba) hat sich beim Mäusefangen im Wald verirrt.
* **Ressourcenmanagement (Survival-Mechanik):** Das Überleben der Spielfigur ist an zwei primäre Variablen gekoppelt: Wasser und Trockenfutter (Punkte). Die Mechanik ist zeitkritisch: Der Wasserstand sinkt kontinuierlich. Fällt der Wasserwert auf null, greift die Loss-Condition (Game Over). Die Wasserressource kann an spezifischen Interaktionspunkten (Wasserquellen) regeneriert werden.
* **Progressionssystem:** Trockenfutter fungiert als Fortschrittswährung. Das globale Spielziel (Win-Condition) ist die Akkumulation von 100 Punkten. Punkte werden als Belohnung für erfolgreich abgeschlossene Quests vergeben. Die Progression ist in Etappen strukturiert, die bei Erreichen definierter Punkteschwellen neue Story-Elemente, Zielgebiete oder NPC-Interaktionen triggern.
* **Initialisierung (Startbedingungen):** Die Spielfigur spawnt mit 0 Punkten (kein Futter) und einem maximalen Wasserstand (100 %).

## Game Flow und Etappen

**Makro-Ziel:** Akkumulation von 100 Futterstücken, um die für die Heimkehr benötigte Energie zu erreichen.  
**Meilensteine:** Der Spielfortschritt verläuft in vordefinierten Etappen. Das Erreichen der jeweiligen Punkteschwelle fungiert als Trigger für den nächsten narrativen Hinweis und schaltet die Folgeaufgaben frei.

---

### Etappe 1 (Ziel: 10 Punkte)
* **Ausgangslage:** Die Spielfigur startet orientierungslos mit 0 Punkten.
* **Quest:** Ein NPC fordert den Spieler auf, 10 Blumen im Startgebiet zu sammeln und als Strauß zu übergeben *(+10P)*.
* **Narrativer Trigger (Tutorial):** *"Wenn du Durst hast, findest du an Wasserquellen frisches Wasser. Das ist sehr wichtig für dein Überleben."*

### Etappe 2 (Ziel: 25 Punkte)
* **Ausgangslage:** Interaktion mit dem nächsten NPC (Bauer).
* **Quest:** Eier müssen unbeschadet über ein Gewässer auf die gegenüberliegende Straßenseite transportiert werden *(+15P)*.
* **Narrativer Trigger:** Der Bauer gibt einen direktionalen Hinweis: *"Ich habe deine Maus gesehen. Sie ist in diese Richtung gelaufen, geh mal zu XY."*

### Etappe 3 (Ziel: 50 Punkte)
* **Quest 1:** Die entlaufene Maus lokalisieren und einfangen *(+15P)*.
* **Ausgangslage (Verzweigte Quest):** Ein NPC hat seinen Schlüssel verloren. Das System bietet dem Spieler drei Lösungsansätze:
  * **Pfad A:** Holz sammeln und ein Feuer entfachen, um dem NPC Wärme zu spenden *(+10P)*.
  * **Pfad B:** Den verlorenen Schlüssel in der Umgebung finden *(+15P)*.
  * **Pfad C:** Eine Quizfrage des NPCs beantworten  *(+20P)*.
* **Narrativer Trigger:** *"Finde dein Halsband, damit man dir weiterhelfen kann."*

### Etappe 4 (Ziel: 70 Punkte)
* **Quest 1:** Das verlorene Halsband in der Spielwelt finden *(+10P)*.
* **Quest 2 (Eskorte):** Einen verletzten NPC sicher zu einem Tierarzt eskortieren *(+15P)*.
* **Narrativer Trigger:** Der Tierarzt liest das gefundene Halsband aus und identifiziert so den Besitzer.

### Etappe 5 (Ziel: 100 Punkte)
* **Ausgangslage:** Der Tierarzt verlangt eine finale Gegenleistung für die Kontaktaufnahme mit dem Besitzer.
* **Quest (Finale):** Durchquerung eines Labyrinths, um eine seltene Pflanze als Heilmittel zu extrahieren *(+20P)*.

