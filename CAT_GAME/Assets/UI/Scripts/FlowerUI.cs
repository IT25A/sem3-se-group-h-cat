using UnityEngine;
using TMPro;

public class FlowerUI : MonoBehaviour
{
    [Header("UI Elemente")]
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