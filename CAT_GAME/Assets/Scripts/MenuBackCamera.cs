using UnityEngine;

public class MenuBackCamera : MonoBehaviour {
    private RectTransform rectTransform;
    public float moveSpeed = 30f; 
    public Vector2 minPosition = new Vector2(-200f, -100f);
    public Vector2 maxPosition = new Vector2(200f, 100f);

    private Vector2 targetPosition;

    void Start() {
        rectTransform = GetComponent<RectTransform>();
        GetNewRandomTarget();
    }

    void Update() {
        rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(rectTransform.anchoredPosition, targetPosition) < 1f) {
            GetNewRandomTarget();
        }
    }

    void GetNewRandomTarget() {
        float randomX = Random.Range(minPosition.x, maxPosition.x);
        float randomY = Random.Range(minPosition.y, maxPosition.y);
        targetPosition = new Vector2(randomX, randomY);
    }
}