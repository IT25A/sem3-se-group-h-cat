using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class FadeInButtonAfterTime : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    [Tooltip("Wartezeit in Sekunden, bis der Button eingeblendet wird")]
    public float delayTime = 15f;

    [Tooltip("Dauer des Einblendens")]
    public float fadeDuration = 1f;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        // Zu Beginn komplett unsichtbar und klicksperre aktiv
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }

    void Start()
    {
        StartCoroutine(ShowButtonRoutine());
    }

    private IEnumerator ShowButtonRoutine()
    {
        // 1. Die gewünschte Zeit warten (z.B. 15 Sekunden)
        yield return new WaitForSeconds(delayTime);

        float elapsedTime = 0f;

        // 2. Sanft einblenden
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
        // 3. Klicks wieder erlauben, wenn er sichtbar ist
        canvasGroup.blocksRaycasts = true;
    }
}