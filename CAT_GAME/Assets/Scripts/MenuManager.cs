using UnityEngine;
using UnityEngine.SceneManagement; 

public class MenuManager : MonoBehaviour {
    [Header("Panels")]
    public GameObject startPanel;
    public GameObject settingsPanel;
    public GameObject mainCanvas;
    public GameObject inGamePanel; 
    public GameObject inGameSettings; 
    public GameObject pausePanel; 

    [Header("Settings")]
    public string gameSceneName = "GameScene"; //hier GameScene name ändern

    private void Start() {
        ShowStartPanel();
    }

    public void StartGame() {
        if (mainCanvas != null) {
            startPanel.SetActive(false);
            settingsPanel.SetActive(false);
            inGameSettings.SetActive(false);
            pausePanel.SetActive(false);

        }
        inGamePanel.SetActive(true);
        Time.timeScale = 1; 
    }

    public void OpenSettings() {
        startPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void ShowStartPanel() {
        mainCanvas.SetActive(true);
        startPanel.SetActive(true);
        settingsPanel.SetActive(false);
        pausePanel.SetActive(false);
        inGamePanel.SetActive(false);
    }

    public void QuitGame() {
        Debug.Log("exit game");
        Time.timeScale = 0; 
        ShowStartPanel();
    }

    public void PauseMenu() {
        pausePanel.SetActive(true);
        inGameSettings.SetActive(false);
        Time.timeScale = 0; 
    }

    public void OpenInGameSettings() {
        inGameSettings.SetActive(true);
        pausePanel.SetActive(false);
    }

    public void RestartGame() {
        Time.timeScale = 1;
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.name);
        StartGame();
    }
}