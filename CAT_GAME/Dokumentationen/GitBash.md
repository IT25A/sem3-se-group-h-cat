Projektordner mit GitBash öffnen

**1. Updates vom Team holen** Stell als Erstes sicher, dass du den aktuellsten Stand deiner Gruppenmitglieder hast.

- `git switch main` (wechselt auf den Haupt-Branch)
    
- `git pull` (lädt die neuesten Änderungen deiner Kollegen herunter)
    

**2. Deinen Feature-Branch erstellen** Jetzt kapselst du deine fertige Katzen-Steuerung in einen eigenen Arbeitsbereich ab.

- `git switch -c feature/simba-movement`
    

**3. Deine Änderungen einpacken (Staging & Commit)** Jetzt sicherst du alles, was du gerade in Unity gebaut hast (Skripte, Animationen, das Inspector-Setup).

- `git add .` (der Punkt nimmt alle geänderten Dateien auf, auch die wichtigen unsichtbaren Unity-Meta-Dateien)
    
- `git commit -m "feat: Simba Steuerung, Kamera-Tracking und Collider hinzugefügt"`
    

**4. Hochladen zu GitHub** Schieb deinen fertigen Branch auf den GitHub-Server der Gruppe. Da es den Branch dort noch nicht gibt, nutzt du beim ersten Mal diesen Befehl:

- `git push -u origin feature/simba-movement`
    

**5. Der Pull Request** Das passiert nicht mehr im Terminal. Du gehst im Browser auf eure GitHub-Seite. Dort wird dir jetzt direkt ein grüner Button "Compare & pull request" für deinen neuen Branch angezeigt. Klick darauf, dann können deine Teamkollegen deinen Code absegnen und gefahrlos in den `main`-Branch mergen.