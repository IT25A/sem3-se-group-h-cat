using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings; 

public class NotificationManager : MonoBehaviour {
    public static NotificationManager Instance;

    [Header("UI Elemente")]
    public GameObject notificationPanel; 
    public TextMeshProUGUI messageText;  
    public CanvasGroup canvasGroup;      

    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (notificationPanel != null)
            notificationPanel.SetActive(false);
    }

    public void ShowLocalizedNotification(string tableCollectionName, string entryKey, float duration = 3f) {
        StopAllCoroutines();
        StartCoroutine(DisplayLocalizedRoutine(tableCollectionName, entryKey, duration));
    }

    private IEnumerator DisplayLocalizedRoutine(string tableCollectionName, string entryKey, float duration) {
        yield return LocalizationSettings.InitializationOperation;

        var operation = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(tableCollectionName, entryKey);
        yield return operation;

        if (operation.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded) {
            if (messageText != null)
                messageText.text = operation.Result; 
        }
        else {
            if (messageText != null)
                messageText.text = "MISSING_TEXT";
        }

        notificationPanel.SetActive(true);

        yield return StartCoroutine(FadeCanvasGroup(canvasGroup, 0, 1, 0.3f));

        yield return new WaitForSeconds(duration);

        yield return StartCoroutine(FadeCanvasGroup(canvasGroup, 1, 0, 0.3f));

        notificationPanel.SetActive(false);
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float time) {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, elapsedTime / time);
            yield return null;
        }
        cg.alpha = end;
    }
}