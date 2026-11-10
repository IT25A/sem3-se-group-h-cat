using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NotificationHistoryUI : MonoBehaviour
{
    [Header("UI Elemente")]
    public GameObject historyPanel;
    public Transform contentParent;
    public GameObject notificationItemPrefab;

    private void Start()
    {
        if (historyPanel != null)
            historyPanel.SetActive(false);
    }

    public void UpdateHistoryDisplay()
    {
        if (contentParent == null || notificationItemPrefab == null || NotificationManager.Instance == null) return;

        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        var history = NotificationManager.Instance.notificationHistory;
        var spriteHistory = NotificationManager.Instance.notificationSpriteHistory;

        for (int i = history.Count - 1; i >= 0; i--)
        {
            GameObject newItem = Instantiate(notificationItemPrefab, contentParent);

            Transform txtTransform = newItem.transform.Find("notificationTxt");
            if (txtTransform != null)
            {
                TextMeshProUGUI itemText = txtTransform.GetComponent<TextMeshProUGUI>();
                if (itemText != null)
                {
                    itemText.text = history[i];
                }
            }
            Image[] images = newItem.GetComponentsInChildren<Image>();
            Image targetImage = null;

            foreach (var img in images)
            {
                if (img.gameObject != newItem)
                {
                    targetImage = img;
                    break;
                }
            }

            if (targetImage != null && i < spriteHistory.Count)
            {
                targetImage.sprite = spriteHistory[i];
                targetImage.color = Color.white;
                targetImage.gameObject.SetActive(true);
            }
        }
    }
}