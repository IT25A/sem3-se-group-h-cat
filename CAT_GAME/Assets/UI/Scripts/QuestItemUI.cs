using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestItemUI : MonoBehaviour
{
    [Header("UI Elemente")]
    public CanvasGroup canvasGroup;     
    public Image itemIconImage;       
    public TextMeshProUGUI itemText;    

    [Header("Gegenstands-Einstellungen")]
    public Sprite itemSprite;           
    public int requiredAmount = 10;     

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
        if (FlowerQuestManager.Instance == null) return;

        int currentCount = FlowerQuestManager.Instance.currentFlowerCount;
        bool isFinalDialogueFinished = FlowerQuestManager.Instance.finalDialogueFinished;

        bool shouldShow = currentCount > 0 && !isFinalDialogueFinished;

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