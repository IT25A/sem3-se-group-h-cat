 **Bei erstmaliger Verwendung**: 
- Window
- Package Management -> Package Manager
- Unter Sources Unity Registry
- Cinemachine installieren

**Ausführung:**
- Rechtsklick Hierarchy
- Cinemachine -> Targeted Cameras -> 2D Camera

**Mittige Ausrichtung:**
- Die Position von Kamera und Player gleichstellen
- einfach bei beiden X- und Y-Position auf 0 setzen
- Z-Position der Kamera auf -10, damit diese herauszoomen kann
- bei Cinemachine nicht zwingend notwendig, da Camera Distance standardmäßig bei 10 liegt

**Tracking Target für Fokus einer Figur:**
- auf CinemachineCamera gehen 
- beim Inspector ganz oben das Schloss anklicken
- das Player-Objekt in das Tracking-Target-Feld ziehen

**Kamera-Symbol verkleinern / verbergen:**
- in der grauen Leiste unter Scene auf die Sphäre ganz rechts klicken 
- entweder direkt deaktivieren oder 3D Icons verkleinern

**Cinemachine Brain:**
- Add Components bei Main Camera
- Cinemachine Brain hinzufügen

**Größe der Figur in der Kamera:**
- zum Inspektor von CinemachineCamera
- dort einen Wert für Lens festlegen (je kleiner, desto näher ist die Figur an der Kamera)
- nur im Game-Modus sichtbar