- Git Bash installieren
- mit git --version überprüfen
- folgende Daten angeben:
	git config --global user.name "Dein Vor- und Nachname"
	git config --global user.email "deine.email@adresse.de"
- Git im Projektordner initialisieren (bei Leerzeichen in Ordnernamen Anführungszeichen verwenden):
	cd "pfad/zu/MeinUnitySpiel"
	git init

**Für Unity-Projekte mit GitHub:**
- .gitignore-Vorlage in GitHub-Server integrieren:
	curl -o .gitignore https://raw.githubusercontent.com/github/gitignore/main/Unity.gitignore
- mit git status überprüfen