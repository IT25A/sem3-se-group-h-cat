using System.Collections;
using TMPro;
using UnityEngine;
using System.Collections.Generic; 
using UnityEngine.Localization.Settings; 

public class NotificationManager : MonoBehaviour {
    public static NotificationManager Instance;

    [Header("UI Elemente")]
    public GameObject notificationPanel; 
    public TextMeshProUGUI messageText;  
    public CanvasGroup canvasGroup;

    [Header("Verlauf (History)")]
    public List<string> notificationHistory = new List<string>();
    public int maxHistoryCount = 20;      

    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (notificationPanel != null)
            notificationPanel.SetActive(false);
    }

    public void ShowLocalizedNotification(string tableCollectionName, string entryKey, float duration = 5f) {
        StopAllCoroutines();
        StartCoroutine(DisplayLocalizedRoutine(tableCollectionName, entryKey, duration));
    }

    private IEnumerator DisplayLocalizedRoutine(string tableCollectionName, string entryKey, float duration) {
        yield return LocalizationSettings.InitializationOperation;

        var operation = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(tableCollectionName, entryKey);
        yield return operation;

        string finalMessage = "MISSING_TEXT";

        if (operation.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded) {
            finalMessage = operation.Result;
        }
        if (messageText != null) {
            messageText.text = finalMessage;
        }
        
        AddToHistory(finalMessage);

        notificationPanel.SetActive(true);
        yield return StartCoroutine(FadeCanvasGroup(canvasGroup, 0, 1, 0.3f));
        yield return new WaitForSeconds(duration);
        yield return StartCoroutine(FadeCanvasGroup(canvasGroup, 1, 0, 0.3f));
        notificationPanel.SetActive(false);
    }
    private void AddToHistory(string message) {
        notificationHistory.Add(message);
        if (notificationHistory.Count > maxHistoryCount) {
            notificationHistory.RemoveAt(0);
        }
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float time) {
        float elapsedTime = 0f;
        while (elapsedTime < time) {
            elapsedTime += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, elapsedTime / time);
            yield return null;
        }
        cg.alpha = end;
    }
}