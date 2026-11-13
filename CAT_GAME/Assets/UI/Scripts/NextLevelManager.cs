using UnityEngine;

// Automatically switches to the next scene once the required amount of dry food is reached
public class NextLevelManager:MonoBehaviour {

    [Header("Scenes")]
    public string Scene1 = "GameScene";
    public string Scene2 = "GameScene";
    public string Scene3 = "GameScene";

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
