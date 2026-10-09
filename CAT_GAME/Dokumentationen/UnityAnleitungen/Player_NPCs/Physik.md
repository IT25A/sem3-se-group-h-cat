- bei Hierarchy Rechtsklick
- 2D Object -> Sprites -> Square
- auf Quadrat klicken
- bei Inspector ganz nach unten scrollen und auf Add Components klicken
- Physics 2D -> Rigidbody 2D
- für Top-Down-Perspektive Gravity Scale auf 0 setzen
- nochmal Add Component 
- Physics 2D -> Box Collider 2D
- zusätzlich unter Constraints Haken bei Freeze Rotation Z setzen

**Für 2.5D-Optik:**
- Der Kollisionsbereich soll um die Füße bzw. im unteren Teil des Charakters liegen
- Wenn der Charakter weiter unten ist, muss er im Vordergrund sein und darf sich nicht mit der Optik der anderen Objekten vermischen
- Dies kann mittels dynamischer Y-Sortierung geregelt werden
- im Projekt-Fenster nach Renderer2D suchen
- im Inspector zu Transparency Sort Mode
- auf Custom Axis stellen 
- Y = 1 setzen, X und Y bleiben bei 0 