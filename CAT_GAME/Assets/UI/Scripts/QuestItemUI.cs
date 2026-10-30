using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using Unity.VectorGraphics;

public class QuestItemUI : MonoBehaviour
{
    [Header("UI Elemente")]
    public CanvasGroup canvasGroup;
    public Image itemIconImage;
    public TextMeshProUGUI itemText;

    [Header("Gegenstands-Einstellungen")]
    public Sprite itemSprite;
    public int requiredAmount = 10;

    public enum QuestType { Task1, Task2, Task3 }
    public QuestType questType;

    [Header("Localization Keys")]
    public string tableName = "MenuText";
    public string warningKey = "received_flowers";

    void Start()
    {
        // Bild zuweisen
        if (itemIconImage != null && itemSprite != null)
        {
            itemIconImage.sprite = itemSprite;
        }

        // CanvasGroup automatisch holen
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
        bool f = false;
        switch (questType)
        {
            case QuestType.Task1:
                if (FlowerQuestManager.Instance == null) return;

                currentCount = FlowerQuestManager.Instance.currentFlowerCount;
                f = FlowerQuestManager.Instance.finalDialogueFinished;
                break;
            case QuestType.Task2:
                currentCount = TaskCollectibleState.EggCount;
                if (SceneData.TargetSpawnID == "Scene2") f = true;
                break;

            case QuestType.Task3:

                break;
        }

        if (currentCount == requiredAmount)
        {
            if (NotificationManager.Instance != null)
            {
                NotificationManager.Instance.ShowLocalizedNotification(tableName, warningKey);
                Debug.Log("Notification für" + questType + " gesendet");
            }
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