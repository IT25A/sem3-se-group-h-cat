using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class FadeInButtonAfterTime : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    [Tooltip("Delay in seconds before the button fades in")]
    public float delayTime = 15f;

    [Tooltip("Fade-in duration")]
    public float fadeDuration = 1f;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        // Button zu Beginn unsichtbar machen und Klicks blockieren, bis das Einblenden abgeschlossen ist
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }

    void Start()
    {
        StartCoroutine(ShowButtonRoutine());
    }

    private IEnumerator ShowButtonRoutine()
    {
        yield return new WaitForSeconds(delayTime);

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
}