using UnityEngine;
using TMPro;

public class CreditsScroll : MonoBehaviour {
    public RectTransform creditsTextRect; 
    public float scrollSpeed = 50f;       
    public float resetPosition = 600f; 

    private Vector2 startPosition;
    private bool isScrolling = false;

    void Start() {
        if (creditsTextRect == null)
            creditsTextRect = GetComponent<RectTransform>();
        
        startPosition = creditsTextRect.anchoredPosition;
    }

    void Update() {
        creditsTextRect.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        if (creditsTextRect.anchoredPosition.y > resetPosition) {
            creditsTextRect.anchoredPosition = startPosition;
        }
    }

    public void StartScrolling() {
        creditsTextRect.anchoredPosition = startPosition;
        isScrolling = true;
    }

    public void StopScrolling() {
        isScrolling = false;
        creditsTextRect.anchoredPosition = startPosition;
    }
}