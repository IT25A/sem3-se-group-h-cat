using UnityEngine;

public class MenuBackCamera : MonoBehaviour {
    private RectTransform rectTransform;
    public float moveSpeed = 30f; 
    public Vector2 minPosition = new Vector2(-400f, -200f);
    public Vector2 maxPosition = new Vector2(400f, 200f);

    private Vector2 targetPosition;

    void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    void OnEnable() {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();
            
        GetNewRandomTarget();
        rectTransform.anchoredPosition = targetPosition; 
    }

    void Update() {
        rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(rectTransform.anchoredPosition, targetPosition) < 1f)  {
            GetNewRandomTarget();
        }
    }

    void GetNewRandomTarget() {
        float randomX = Random.Range(minPosition.x, maxPosition.x);
        float randomY = Random.Range(minPosition.y, maxPosition.y);
        targetPosition = new Vector2(randomX, randomY);
    }
}