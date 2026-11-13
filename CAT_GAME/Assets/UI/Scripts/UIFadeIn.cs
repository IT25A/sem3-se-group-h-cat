using UnityEngine;
using System.Collections;

// Smoothly fades in a UI element via CanvasGroup after a configurable delay
[RequireComponent(typeof(CanvasGroup))]
public class UIFadeIn : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    
    [Header("Settings")]
    [Tooltip("Delay in seconds before fading in")]
    public float startDelay = 10.0f;
    
    [Tooltip("Fade-in duration in seconds")]
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