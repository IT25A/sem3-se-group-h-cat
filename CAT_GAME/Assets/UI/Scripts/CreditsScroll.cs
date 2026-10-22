using UnityEngine;
using TMPro;

public class CreditsScroll : MonoBehaviour {
    public RectTransform creditsTextRect; 
    public float scrollSpeed = 50f;       
    public float resetPosition = 600f; 

    private Vector2 startPosition;
    public bool isScrolling = false;

    void Awake() {
        if (creditsTextRect == null)
            creditsTextRect = GetComponent<RectTransform>();
        
        startPosition = creditsTextRect.anchoredPosition;
    }

    private void OnEnable() {
        ResetAndStart();
    }

    private void OnDisable() {
        StopScrolling();
    }

    void Update() {
        if (!isScrolling) return;

        creditsTextRect.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        if (creditsTextRect.anchoredPosition.y > resetPosition) {
            creditsTextRect.anchoredPosition = startPosition;
        }
    }

    public void ResetAndStart() {
        if (creditsTextRect != null) {
            creditsTextRect.anchoredPosition = startPosition;
        }
        isScrolling = true;
    }

    public void StopScrolling() {
        isScrolling = false;
        if (creditsTextRect != null) {
            creditsTextRect.anchoredPosition = startPosition;
        }
    }
}