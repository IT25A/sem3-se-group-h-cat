using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using Unity.VectorGraphics;

public class QuestItemUI : MonoBehaviour
{
    [Header("UI Elements")]
    public CanvasGroup canvasGroup;
    public Image itemIconImage;
    public TextMeshProUGUI itemText;

    [Header("Item Settings")]
    public Sprite itemSprite;
    public int requiredAmount = 10;

    // Zuordnung des UI-Elements zur jeweiligen Sammel-Quest
    public enum QuestType { Task1, Task2, Task3, Task4, Task5, Task6}
    public QuestType questType;

    [Header("Localization Keys")]
    public string tableName = "MenuText";
    public string warningKey = "received_flowers";

    public bool f = false;
    void Start()
    {
        if (itemIconImage != null && itemSprite != null)
        {
            itemIconImage.sprite = itemSprite;
        }

        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }

        HideUI();

    }

    void Update()
    {
        int currentCount = 0;
        f = false;
        // Aktuellen Sammelstand der gewählten Quest abfragen und bei Erreichen des Ziels einmalig eine Benachrichtigung senden
        switch (questType)
        {
            case QuestType.Task1:
                if (FlowerQuestManager.Instance != null)
                {
                    currentCount = FlowerQuestManager.Instance.currentFlowerCount;
                    f = FlowerQuestManager.Instance.finalDialogueFinished;
                }
                else
                {
                    currentCount = TaskCollectibleState.FlowerCount;
                    f = false;
                }
                if (currentCount >= requiredAmount && !TaskCollectibleState.flowerNotificationSent)
                {
                    if (NotificationManager.Instance != null)
                    {
                        NotificationManager.Instance.ShowLocalizedNotification(tableName, warningKey);
                        Debug.Log("Notification für" + questType + " gesendet");
                        TaskCollectibleState.flowerNotificationSent = true;
                    }
                }
                break;
            case QuestType.Task2:
                currentCount = TaskCollectibleState.EggCount;
                if (currentCount >= requiredAmount && !TaskCollectibleState.eggNotificationSent)
                {
                    if (NotificationManager.Instance != null)
                    {
                        NotificationManager.Instance.ShowLocalizedNotification(tableName, warningKey);
                        Debug.Log("Notification für" + questType + " gesendet");
                        TaskCollectibleState.eggNotificationSent = true;
                    }
                }
                break;

            case QuestType.Task3:
                currentCount = TaskCollectibleState.MouseCount;
                if (currentCount >= requiredAmount && !TaskCollectibleState.mouseNotificationSent)
                {
                    if (NotificationManager.Instance != null)
                    {
                        NotificationManager.Instance.ShowLocalizedNotification(tableName, warningKey);
                        Debug.Log("Notification für" + questType + " gesendet");
                        TaskCollectibleState.mouseNotificationSent = true;
                    }
                }
                break;

            case QuestType.Task4:
                currentCount = TaskCollectibleState.KeyCount;
                if (currentCount >= requiredAmount && !TaskCollectibleState.keyNotificationSent)
                {
                    if (NotificationManager.Instance != null)
                    {
                        NotificationManager.Instance.ShowLocalizedNotification(tableName, warningKey);
                        Debug.Log("Notification für" + questType + " gesendet");
                        TaskCollectibleState.keyNotificationSent = true;
                    }
                }
                break;

            case QuestType.Task5:
                currentCount = TaskCollectibleState.SimbaNecklaceCount;
                if (currentCount >= requiredAmount && !TaskCollectibleState.simbaNecklaceNotificationSent)
                {
                    if (NotificationManager.Instance != null)
                    {
                        NotificationManager.Instance.ShowLocalizedNotification(tableName, warningKey);
                        Debug.Log("Notification für" + questType + " gesendet");
                        TaskCollectibleState.simbaNecklaceNotificationSent = true;
                    }
                }
                break;

            case QuestType.Task6:
                currentCount = TaskCollectibleState.WoodCount;
                if (currentCount >= requiredAmount && !TaskCollectibleState.woodNotificationSent)
                {
                    if (NotificationManager.Instance != null)
                    {
                        NotificationManager.Instance.ShowLocalizedNotification(tableName, warningKey);
                        Debug.Log("Notification für" + questType + " gesendet");
                        TaskCollectibleState.woodNotificationSent = true;
                    }
                }
                break;
        }

        bool shouldShow = currentCount > 0 && !f;

        if (shouldShow)
        {
            ShowUI();

            if (itemText != null)
            {
                itemText.text = $"{currentCount}";
            }
        }
        else
        {
            HideUI();
        }
    }

    private void ShowUI()
    {
        if (canvasGroup == null) return;
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private void HideUI()
    {
        if (canvasGroup == null) return;
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}