using UnityEngine;
using System.Collections;

// Smoothly fades in a UI element via CanvasGroup after a configurable delay
[RequireComponent(typeof(CanvasGroup))]
public class UIFadeIn : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private CameraZoomOut cameraZoomOut;
    private bool hasStartedFade = false;
    
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
        cameraZoomOut = FindFirstObjectByType<CameraZoomOut>();

        // If a CameraZoomOut exists in the scene, automatically wait for the zoom to finish
        if (cameraZoomOut != null)
        {
            cameraZoomOut.onZoomCompleted.AddListener(StartFadeIn);
        }
        else
        {
            StartFadeIn();
        }
    }

    void OnDestroy()
    {
        if (cameraZoomOut != null)
        {
            cameraZoomOut.onZoomCompleted.RemoveListener(StartFadeIn);
        }
    }

    public void StartFadeIn()
    {
        if (hasStartedFade) return;
        hasStartedFade = true;

        float delay = (cameraZoomOut != null) ? 0f : startDelay;
        StartCoroutine(FadeRoutine(delay));
    }

    private IEnumerator FadeRoutine(float delay)
    {
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

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