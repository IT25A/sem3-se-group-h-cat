using UnityEngine;

public class NextLevelManager:MonoBehaviour {

    [Header("Scenes")]
    public string Scene1 = "GameScene"; //hier GameScene name ändern
    public string Scene2 = "GameScene"; //hier GameScene name ändern
    public string Scene3 = "GameScene"; //hier GameScene name ändern

    [Header("Items")]
    public int Trockenfutter = 100;
    private void Start() {
        Trockenfutter = 0; 
    }

    public void Update() {
        if (Trockenfutter >= 10) {
            UnityEngine.SceneManagement.SceneManager.LoadScene(Scene2);
        }
    }
}
