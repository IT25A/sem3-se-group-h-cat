using UnityEngine;
using TMPro;

public class FlowerUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject flowerPanel;     
    public TextMeshProUGUI flowerText; 

    void Start()
    {
        if (flowerPanel != null)
        {
            flowerPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (FlowerQuestManager.Instance == null) return;

        int count = FlowerQuestManager.Instance.currentFlowerCount;
        bool isCompleted = FlowerQuestManager.Instance.finalDialogueFinished;

        // Only display the flower UI while at least one flower has been collected and the final dialogue is still pending
        if (count > 0 && !isCompleted)
        {
            if (flowerPanel != null && !flowerPanel.activeSelf)
            {
                flowerPanel.SetActive(true);
            }
        }
        else
        {
            if (flowerPanel != null && flowerPanel.activeSelf)
            {
                flowerPanel.SetActive(false);
            }
        }

        if (flowerText != null)
        {
            flowerText.text = $"{count}x";
        }
    }
}