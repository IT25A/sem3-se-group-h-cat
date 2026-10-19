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
    public GameObject notifyHistory; 

    [Header("Settings")]
    public string gameSceneName = "Scene 1 - Phase 1"; //hier GameScene name ändern

    [Header("LocalizationKeys")]
    public string tableName = "UI_TextTable";
    public string sM = "startMessage";
    private void Start() {

        string currentSceneName = SceneManager.GetActiveScene().name;

        if (SceneData.isGameStarted) {
            SetGameState(GameState.Playing);

            if (startPanel != null) startPanel.SetActive(false);
            if (settingsPanel != null) settingsPanel.SetActive(false);
            if (inGameSettings != null) inGameSettings.SetActive(false);
            if (pausePanel != null) pausePanel.SetActive(false);
            if (endPanel != null) endPanel.SetActive(false);
            if (creditsPanel != null) creditsPanel.SetActive(false);
            if (inGamePanel != null) inGamePanel.SetActive(true);

            Time.timeScale = 1f;

            WaterManager waterManager = FindAnyObjectByType<WaterManager>();
            if (waterManager != null) {
                waterManager.isGameOver = false;
                waterManager.isGameStarted = true; 
                waterManager.StartWaterDecrease();
            }
        }
        else  {
            SetGameState(GameState.MainMenu);
            if (autoStartGame) {
                StartGame();
                autoStartGame = false; 
            } else {
                ShowStartPanel(); 
            }
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

        SceneData.isGameStarted = true; 
        
        startPanel.SetActive(false);
        settingsPanel.SetActive(false);
        inGameSettings.SetActive(false);
        pausePanel.SetActive(false);
        endPanel.SetActive(false);
        creditsPanel.SetActive(false);
        
        inGamePanel.SetActive(true);
        if (NotificationManager.Instance != null) {
                    NotificationManager.Instance.ShowLocalizedNotification(tableName, sM);
                }
        Time.timeScale = 1;

        WaterManager waterManager = FindAnyObjectByType<WaterManager>();
        if (waterManager != null) {
            waterManager.isGameOver = false;
            waterManager.isGameStarted = false;

            waterManager.RefillWater(100f);
            SceneData.savedWater = 100f; 
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
    
        WaterManager waterManager = FindAnyObjectByType<WaterManager>();
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

    public void NotificationHistory() {
        NotificationHistoryUI nhUI = FindAnyObjectByType<NotificationHistoryUI>();
        bool newState = !notifyHistory.activeSelf;
        notifyHistory.SetActive(newState);

        if (newState) {
            nhUI.UpdateHistoryDisplay();
        }
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
        SceneData.savedWater = 100f; 
        //Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(gameSceneName);
        
    }

    public void FinishGame() {
        SetGameState(GameState.GameOver);
        endPanel.SetActive(true);
    }

    public void CloseNotify() {
        notifyPanel.SetActive(false); 
    }
}