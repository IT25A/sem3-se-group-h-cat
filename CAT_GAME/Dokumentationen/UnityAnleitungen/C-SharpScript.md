- Edit
- Preferences
- External Tools
- Überprüfen, ob bei External Script Editor Visual Studio zu erkennen ist

**Skript erstellen:**
- Assets
- Create
- Scripting
- Empty C# Script
- Skript benennen
- Wichtig: Der Klassenname im Skript muss exakt wie der Skriptname sein.
- damit der Code später per Drag-and-Drop an das entsprechende Objekt übergeben wird, muss der Code folgende Struktur haben

*using UnityEngine* 

*public class Klassenname : MonoBehaviour*
{
...
}

- MonoBehaviour wird vorausgesetzt, damit das Objekt den Code erkennen kann
- beim Rigidbody-Feld für das Skript muss das Rigidbody 2D vom gleichen Charakter von oben gezogen werden