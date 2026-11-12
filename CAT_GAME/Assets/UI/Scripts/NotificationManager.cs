using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Localization.Settings;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;

    [Header("UI Elemente")]
    public GameObject notificationPanel;
    public TextMeshProUGUI messageText;
    public CanvasGroup canvasGroup;
    public Image notificationIcon;

    [Header("Images")]
    public Sprite waterIcon;
    public Sprite startIcon;
    public Sprite foodIcon;
    public Sprite lowWaterIcon;
    public Sprite daisyFlowerIcon;
    public Sprite eggs;
    public Sprite mouseIcon;
    public Sprite keyIcon;
    public Sprite woodIcon;
    public Sprite simbaNecklaceIcon;
    public Sprite defaultIcon;

    [Header("Verlauf (History)")]
    public List<string> notificationHistory = new List<string>();

    public List<Sprite> notificationSpriteHistory = new List<Sprite>();
    public int maxHistoryCount = 20;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (notificationPanel != null)
            notificationPanel.SetActive(false);
    }

    private void Start()
    {
        if (SceneData.savedNotificationHistoryText.Count > 0)
        {
            notificationHistory = new List<string>(SceneData.savedNotificationHistoryText);
        }
        if (SceneData.savedNotificationHistorySprites.Count > 0)
        {
            notificationSpriteHistory = new List<Sprite>(SceneData.savedNotificationHistorySprites);
        }
    }

    public Sprite GetSpriteForKey(string entryKey)
    {
        if (entryKey.Contains("water") || entryKey.Contains("waterRefill")) return waterIcon;
        if (entryKey.Contains("food") || entryKey.Contains("foodCount")) return foodIcon;
        if (entryKey.Contains("waterLow") || entryKey.Contains("NotificationWaterEnding")) return lowWaterIcon;
        if (entryKey.Contains("start") || entryKey.Contains("startMessage")) return startIcon;
        if (entryKey.Contains("received_flowers")) return daisyFlowerIcon;
        if (entryKey.Contains("received_eggs")) return eggs;
        if (entryKey.Contains("received_mouse")) return mouseIcon;
        if (entryKey.Contains("received_key")) return keyIcon;
        if (entryKey.Contains("received_wood")) return woodIcon;
        if (entryKey.Contains("received_collar")) return simbaNecklaceIcon;
        return defaultIcon;
    }
    public void ShowLocalizedNotification(string tableCollectionName, string entryKey, float duration = 5f)
    {
        StopAllCoroutines();
        StartCoroutine(DisplayLocalizedRoutine(tableCollectionName, entryKey, duration));
    }

    private IEnumerator DisplayLocalizedRoutine(string tableCollectionName, string entryKey, float duration)
    {
        yield return LocalizationSettings.InitializationOperation;

        var operation = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(tableCollectionName, entryKey);
        yield return operation;

        string finalMessage = "MISSING_TEXT";

        if (operation.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
        {
            finalMessage = operation.Result;
        }
        if (messageText != null)
        {
            messageText.text = finalMessage;
        }

        if (notificationIcon != null)
        {
            if (entryKey.Contains("water") || entryKey.Contains("waterRefill"))
            {
                notificationIcon.sprite = waterIcon;
            }
            else if (entryKey.Contains("food") || entryKey.Contains("foodCount"))
            {
                notificationIcon.sprite = foodIcon;
            }
            else if (entryKey.Contains("waterLow") || entryKey.Contains("NotificationWaterEnding"))
            {
                notificationIcon.sprite = lowWaterIcon;
            }
            else if (entryKey.Contains("start") || entryKey.Contains("startMessage"))
            {
                notificationIcon.sprite = startIcon;
            }
            else if (entryKey.Contains("received_flowers"))
            {
                notificationIcon.sprite = daisyFlowerIcon;
            }
            else if (entryKey.Contains("received_eggs"))
            {
                notificationIcon.sprite = eggs;
            }
            else if (entryKey.Contains("received_mouse"))
            {
                notificationIcon.sprite = mouseIcon;
            }
            else if (entryKey.Contains("received_key"))
            {
                notificationIcon.sprite = keyIcon;
            }
            else if (entryKey.Contains("received_collar"))
            {
                notificationIcon.sprite = simbaNecklaceIcon;
            }
            else if (entryKey.Contains("received_wood"))
            {
                notificationIcon.sprite = woodIcon;
            }
            else
            {
                notificationIcon.sprite = defaultIcon;
            }
        }
        Sprite currentIcon = notificationIcon != null ? notificationIcon.sprite : defaultIcon;
        AddToHistory(finalMessage, currentIcon);

        notificationPanel.SetActive(true);
        yield return StartCoroutine(FadeCanvasGroup(canvasGroup, 0, 1, 0.3f));
        yield return new WaitForSeconds(duration);
        yield return StartCoroutine(FadeCanvasGroup(canvasGroup, 1, 0, 0.3f));
        notificationPanel.SetActive(false);
    }
    private void AddToHistory(string message, Sprite icon)
    {
        notificationHistory.Add(message);
        notificationSpriteHistory.Add(icon);

        if (notificationHistory.Count > maxHistoryCount)
        {
            notificationHistory.RemoveAt(0);
            notificationSpriteHistory.RemoveAt(0);
        }

        SceneData.savedNotificationHistoryText = new List<string>(notificationHistory);
        SceneData.savedNotificationHistorySprites = new List<Sprite>(notificationSpriteHistory);
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float time)
    {
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