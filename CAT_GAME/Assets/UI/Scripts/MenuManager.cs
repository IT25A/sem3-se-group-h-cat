using UnityEngine;
using UnityEngine.SceneManagement; 

public class MenuManager : MonoBehaviour {

    public static bool autoStartGame = false; 

    public enum GameState { MainMenu, Playing, Paused, GameOver }
    public static GameState currentState;

    [Header("Panels")]
    public GameObject startPanel;
    public GameObject settingsPanel;
    public GameObject inGamePanel; 
    public GameObject inGameSettings; 
    public GameObject pausePanel; 
    public GameObject endPanel; 
    public GameObject notifyPanel; 
    public GameObject gameOver; 

    public GameObject creditsPanel; 

    [Header("Settings")]
    public string gameSceneName = "Scene 1 - Phase 1"; //hier GameScene name ändern

    private void Start() {
        SetGameState(GameState.MainMenu);
        if (autoStartGame) {
            StartGame();
            autoStartGame = false; 
        }else {
            ShowStartPanel(); 
        }
    }
    public void SetGameState(GameState newState) {
        currentState = newState;

        switch (currentState) {
            case GameState.MainMenu:
                Time.timeScale = 1f;
                break;

            case GameState.Playing:
                Time.timeScale = 1f;
                break;

            case GameState.Paused:
                Time.timeScale = 0f;
                break;

            case GameState.GameOver:
                Time.timeScale = 0f;
                break;
        }
    }

    public void StartGame() {
        Debug.Log("Start Game");
        
        startPanel.SetActive(false);
        settingsPanel.SetActive(false);
        inGameSettings.SetActive(false);
        pausePanel.SetActive(false);
        endPanel.SetActive(false);
        creditsPanel.SetActive(false);
        
        inGamePanel.SetActive(true);
        Time.timeScale = 1;

        WaterManager waterManager = FindObjectOfType<WaterManager>();
        if (waterManager != null) {
            waterManager.isGameOver = false;
            waterManager.isGameStarted = false;

            waterManager.RefillWater(100f);
            waterManager.StartWaterDecrease();
        }
        SetGameState(GameState.Playing);
    }

    public void OpenSettings() {
        startPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OpenCredits() {
        startPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void ShowStartPanel() {
        startPanel.SetActive(true);
        settingsPanel.SetActive(false);
        pausePanel.SetActive(false);
        inGamePanel.SetActive(false);
        endPanel.SetActive(false); 
        creditsPanel.SetActive(false);
    }
    public void QuitToMainMenu() {
        SetGameState(GameState.MainMenu);
    
        WaterManager waterManager = FindObjectOfType<WaterManager>();
        if (waterManager != null) {
            waterManager.isGameOver = false;
            waterManager.isGameStarted = false;
        }

        if(gameOver != null) gameOver.SetActive(false);
        if(pausePanel != null) pausePanel.SetActive(false);
        ShowStartPanel();
    }

    public void PauseMenu() {
        pausePanel.SetActive(true);
        inGameSettings.SetActive(false);
        SetGameState(GameState.Paused);
    }

    public void ContinueGame() {
        pausePanel.SetActive(false);
        SetGameState(GameState.Playing);
    }

    public void OpenInGameSettings() {
        inGameSettings.SetActive(true);
        pausePanel.SetActive(false);
    }

    public void RestartGame() {
        autoStartGame = true; 
        SetGameState(GameState.Playing);
        gameOver.SetActive(false); 
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.name);
    }

    public void FinishGame() {
        SetGameState(GameState.GameOver);
        endPanel.SetActive(true);
    }

    public void CloseNotify() {
        notifyPanel.SetActive(false); 
    }
}