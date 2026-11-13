using UnityEngine;
using System.Collections;

// Blendet ein UI-Element nach einer einstellbaren Verzögerung sanft über die CanvasGroup ein
[RequireComponent(typeof(CanvasGroup))]
public class UIFadeIn : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    
    [Header("Einstellungen")]
    [Tooltip("Wartezeit in Sekunden vor dem Einblenden")]
    public float startDelay = 10.0f;
    
    [Tooltip("Dauer des Einblendens in Sekunden")]
    public float fadeDuration = 3.0f;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
    }

    void Start()
    {
        StartFadeIn();
    }

    public void StartFadeIn()
    {
        StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        yield return new WaitForSeconds(startDelay);

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null; 
        }

        canvasGroup.alpha = 1f;
    }
}