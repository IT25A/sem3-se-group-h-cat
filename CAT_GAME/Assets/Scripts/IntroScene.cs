using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Wichtig für das neue Input System!

public class CutsceneSlideshow : MonoBehaviour
{
    [Header("Bilder & UI")]
    [Tooltip("Das UI-Image auf dem Canvas, das die Bilder anzeigt (SlideDisplay)")]
    public Image displayImage;
    
    [Tooltip("Ziehe hier alle gezeichneten Skizzen in der richtigen Reihenfolge rein")]
    public Sprite[] slides;

    [Header("Ablauf & Zeiten")]
    [Tooltip("Wie viele Sekunden soll jedes Bild angezeigt werden?")]
    public float timePerSlide = 5.0f;
    
    [Tooltip("Darf der Spieler mit Leertaste / E / Mausklick schneller weiterklicken?")]
    public bool allowManualSkip = true;

    [Header("Was passiert nach dem letzten Bild?")]
    [Tooltip("Name der nächsten Szene (z.B. 'StartMenu'). Leer lassen, wenn das letzte Bild stehen bleiben soll!")]
    public string nextSceneName = "Scene 1";

    private int currentIndex = 0;
    private float timer = 0f;

    void Start()
    {
        if (slides != null && slides.Length > 0 && displayImage != null)
        {
            displayImage.sprite = slides[0];
        }
    }

    void Update()
    {
        if (slides == null || slides.Length == 0) return;

        timer += Time.deltaTime;

        // Abfrage über das neue Unity Input System (Leertaste, E-Taste oder linke Maustaste)
        bool skipPressed = false;
        if (allowManualSkip)
        {
            bool spaceOrE = Keyboard.current != null && 
                            (Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.eKey.wasPressedThisFrame);
            bool mouseClick = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
            
            skipPressed = spaceOrE || mouseClick;
        }

        if (timer >= timePerSlide || skipPressed)
        {
            NextSlide();
        }
    }

    public void NextSlide()
    {
        timer = 0f;
        currentIndex++;

        if (currentIndex < slides.Length)
        {
            displayImage.sprite = slides[currentIndex];
        }
        else
        {
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}