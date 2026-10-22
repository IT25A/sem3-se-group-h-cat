using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NotificationHistoryUI : MonoBehaviour {
    [Header("UI Elemente")]
    public GameObject historyPanel; 
    public Transform contentParent; 
    public GameObject notificationItemPrefab; 

    private void Start() {
        if (historyPanel != null)
            historyPanel.SetActive(false);
    }

    public void NotificationHistory() {
        
        if (historyPanel != null) {
            bool newState = !historyPanel.activeSelf;
            historyPanel.SetActive(newState);
        }
        UpdateHistoryDisplay();
    }

    public void UpdateHistoryDisplay() {
        if (contentParent == null || notificationItemPrefab == null || NotificationManager.Instance == null) return;

        foreach (Transform child in contentParent) {
            Destroy(child.gameObject);
        }

        var history = NotificationManager.Instance.notificationHistory;

        for (int i = history.Count - 1; i >= 0; i--) {
            GameObject newItem = Instantiate(notificationItemPrefab, contentParent);
            
            Transform txtTransform = newItem.transform.Find("notificationTxt");
            if (txtTransform != null) {
                TextMeshProUGUI itemText = txtTransform.GetComponent<TextMeshProUGUI>();
                if (itemText != null) {
                    itemText.text = history[i];
                }
            }
        }
    }
}