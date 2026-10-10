using UnityEngine;
using UnityEngine.SceneManagement; 

public class MenuManager : MonoBehaviour {

    public static bool autoStartGame = false; 

    [Header("Panels")]
    public GameObject startPanel;
    public GameObject settingsPanel;
    public GameObject mainCanvas;
    public GameObject inGamePanel; 
    public GameObject inGameSettings; 
    public GameObject pausePanel; 
    public GameObject endPanel; 
    public GameObject notifyPanel; 
    public GameObject gameOver; 

    [Header("Settings")]
    public string gameSceneName = "GameScene"; //hier GameScene name ändern

    private void Start() {
        if (autoStartGame) {
            StartGame();
            autoStartGame = false; 
        }else {
            ShowStartPanel(); 
        }
    }

    public void StartGame() {
        if (mainCanvas != null) {
            startPanel.SetActive(false);
            settingsPanel.SetActive(false);
            inGameSettings.SetActive(false);
            pausePanel.SetActive(false);
            endPanel.SetActive(false);

        }
        inGamePanel.SetActive(true);
        Time.timeScale = 1; 

        WaterManager waterManager = FindObjectOfType<WaterManager>();
        if (waterManager != null) {
            waterManager.StartWaterDecrease();
        }
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
        endPanel.SetActive(false); 
    }

    public void QuitGame() {
        Debug.Log("exit game");
        Time.timeScale = 0; 
        gameOver.SetActive(false);
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
        autoStartGame = true; 
        Time.timeScale = 1; 
        gameOver.SetActive(false); 
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.name);
    }

    public void FinishGame() {
        Time.timeScale = 0; 
        endPanel.SetActive(true);
    }

    public void CloseNotify() {
        notifyPanel.SetActive(false); 
    }
}