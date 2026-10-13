Navigiere zunächst in deiner Bash oder dem Terminal direkt in den Hauptordner deines Projekts. Der generelle Ablauf, um ein lokales Projekt mit Git zu tracken und auf einen Server wie GitHub zu laden, ist unabhängig von der Programmiersprache fast immer gleich:

1. **Repository initialisieren:** Tippe `git init` ein, um den versteckten `.git`-Ordner zu erstellen, der ab jetzt alle Versionen und Änderungen aufzeichnet.
    
2. **.gitignore anlegen:** Erstelle eine Datei namens `.gitignore` im Hauptverzeichnis. Hier definierst du alle Dateien und Ordner (wie Build-Verzeichnisse, IDE-Einstellungen oder temporäre Cache-Dateien), die nicht in die Versionskontrolle gehören.
    
3. **Dateien vormerken (Staging):** Mit `git add .` fügst du alle aktuellen Projektdateien dem Staging-Bereich hinzu.
    
4. **Erster Commit:** Speichere diesen sauberen Startpunkt lokal ab, indem du `git commit -m "Initial commit"` ausführst.
    
5. **Remote-Repository verknüpfen:** Verbinde dein lokales Projekt mit dem leeren Repository auf dem Server über den Befehl `git remote add origin <DEINE_REPO_URL>`.
    
6. **Branch benennen und hochladen:** Setze den Haupt-Branch mit `git branch -M main` (da einige Git-Versionen lokal standardmäßig noch `master` nutzen) und schiebe die Daten abschließend mit `git push -u origin main` in die Cloud.
    

Falls dein Online-Repository bei der Erstellung bereits automatisch eine `README.md` oder Lizenzdatei generiert hat, musst du diese Dateien vor dem allerersten Push zwingend mit `git pull origin main --rebase` auf deinen Rechner herunterladen, da Git den Upload sonst blockiert.