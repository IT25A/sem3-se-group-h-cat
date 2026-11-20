using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MenuManager : MonoBehaviour
{

    public static bool autoStartGame = false;
    public NotificationHistoryUI notificationHistoryUI;
    public enum GameState { MainMenu, Playing, Paused, GameOver }
    public static GameState currentState;
    public AudioSource menuMusic;
    public GameObject cutsceneCanvas;
    public static bool hasShownStartNotification = false;

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

    public GameObject openButton;
    public GameObject closeButton;

    [Header("Settings")]
    public string gameSceneName = "Introscene";

    [Header("LocalizationKeys")]
    public string tableName = "UI_TextTable";
    public string sM = "startMessage";
    private void Start()
    {
        if (cutsceneCanvas != null)
        {
            cutsceneCanvas.SetActive(false);
        }
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Depending on the game state, play either the main menu music or the scene's background music
        if (!SceneData.isGameStarted)
        {
            if (menuMusic != null) menuMusic.Play();
        }
        else
        {
            GameObject sceneMusicObj = GameObject.Find("Music");
            if (sceneMusicObj != null)
            {
                AudioSource sceneAudio = sceneMusicObj.GetComponent<AudioSource>();
                if (sceneAudio != null && !sceneAudio.isPlaying)
                {
                    sceneAudio.Play();
                }
            }
            else
            {
                Debug.LogWarning("Kein GameObject namens 'Music' in dieser Szene gefunden.");
            }
        }

        // If the game is already running (e.g., after a scene transition), directly activate the in-game UI
        if (SceneData.isGameStarted)
        {
            SetGameState(GameState.Playing);

            if (startPanel != null) startPanel.SetActive(false);
            if (settingsPanel != null) settingsPanel.SetActive(false);
            if (inGameSettings != null) inGameSettings.SetActive(false);
            if (pausePanel != null) pausePanel.SetActive(false);
            if (endPanel != null) endPanel.SetActive(false);
            if (creditsPanel != null) creditsPanel.SetActive(false);
            if (inGamePanel != null) inGamePanel.SetActive(true);

            Time.timeScale = 1f;

            if (!hasShownStartNotification && NotificationManager.Instance != null)
            {
                NotificationManager.Instance.ShowLocalizedNotification("MenuText", "startMessage");
                hasShownStartNotification = true; 
            }

            WaterManager waterManager = FindAnyObjectByType<WaterManager>();
            if (waterManager != null)
            {
                waterManager.isGameOver = false;
                waterManager.isGameStarted = true;
                waterManager.StartWaterDecrease();
            }
        }
        else
        {
            SetGameState(GameState.MainMenu);
            if (autoStartGame)
            {
                StartGame();
                autoStartGame = false;
            }
            else
            {
                ShowStartPanel();
            }
        }
    }
    public void SetGameState(GameState newState)
    {
        currentState = newState;

        switch (currentState)
        {
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
    public void StartGame()
    {
        Debug.Log("Start Game");
        SceneData.ResetOnStart();

        if (menuMusic != null) menuMusic.Stop();

        GameObject sceneMusicObj = GameObject.Find("Music");
        if (sceneMusicObj != null)
        {
            AudioSource sceneAudio = sceneMusicObj.GetComponent<AudioSource>();
            if (sceneAudio != null)
            {
                sceneAudio.Play();
            }
        }

        startPanel.SetActive(false);
        settingsPanel.SetActive(false);
        inGameSettings.SetActive(false);
        pausePanel.SetActive(false);
        endPanel.SetActive(false);
        creditsPanel.SetActive(false);

        if (cutsceneCanvas != null)
        {
            cutsceneCanvas.SetActive(true);
        }

        Time.timeScale = 1;
        SetGameState(GameState.Playing);
    }

    public void OpenSettings()
    {
        startPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OpenCredits()
    {
        startPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void ShowStartPanel()
    {
        if (startPanel != null) startPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (inGamePanel != null) inGamePanel.SetActive(false);
        if (endPanel != null) endPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);
    }

    public void QuitToMainMenu()
    {
        SceneData.isGameStarted = false;
        hasShownStartNotification = false;

        SceneData.savedFood = 0;
        SceneData.savedWater = 0;

        TaskCollectibleState.ResetTaskCollectibles();
        TaskCollectibleState.ClearAllCollectedData();
        FlowerQuestManager.ResetFlowerQuest();

        // Destroy the persistent player so the default player is spawned again when reloading the starting scene
        PersistentPlayer player = FindAnyObjectByType<PersistentPlayer>();
        if (player != null)
        {
            Destroy(player.gameObject);
        }

        SceneManager.LoadScene("Introscene");

        GameObject sceneMusicObj = GameObject.Find("Music");
        if (sceneMusicObj != null)
        {
            AudioSource sceneAudio =
            sceneMusicObj.GetComponent<AudioSource>();
            if (sceneAudio != null && sceneAudio != menuMusic)
            {
                sceneAudio.Stop();
            }
        }

        if (menuMusic != null) menuMusic.Play();

        SetGameState(GameState.MainMenu);

        WaterManager waterManager = FindAnyObjectByType<WaterManager>();
        if (waterManager != null)
        {
            waterManager.isGameOver = false;
            waterManager.isGameStarted = false;
        }

        if (gameOver != null) gameOver.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        ShowStartPanel();
    }

    public void PauseMenu()
    {
        pausePanel.SetActive(true);
        inGameSettings.SetActive(false);
        SetGameState(GameState.Paused);
    }

    public void NotificationHistoryOpen()
    {
        NotificationHistoryUI nhUI = FindAnyObjectByType<NotificationHistoryUI>();

        if (notifyHistory != null)
        {
            notifyHistory.SetActive(true);
        }

        if (openButton != null)
        {
            openButton.SetActive(false);
        }
        if (closeButton != null)
        {
            closeButton.SetActive(true);
        }

        if (nhUI != null)
        {
            nhUI.UpdateHistoryDisplay();
        }
    }

    public void NotificationHistoryClose()
    {
        NotificationHistoryUI nhUI = FindAnyObjectByType<NotificationHistoryUI>();

        if (notifyHistory != null)
        {
            notifyHistory.SetActive(false);
        }

        if (openButton != null)
        {
            openButton.SetActive(true);
        }
        if (closeButton != null)
        {
            closeButton.SetActive(false);
        }
    }

    public void ContinueGame()
    {
        pausePanel.SetActive(false);
        SetGameState(GameState.Playing);
    }

    public void OpenInGameSettings()
    {
        inGameSettings.SetActive(true);
        pausePanel.SetActive(false);
    }

    public void RestartGame()
    {
        hasShownStartNotification = false;
        if (gameOver != null) gameOver.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        SetGameState(GameState.Playing);
        Time.timeScale = 1f;

        SceneData.isGameStarted = false;

        SceneData.savedWater = 100f;
        SceneData.savedFood = 0;
        TaskCollectibleState.ResetTaskCollectibles();
        TaskCollectibleState.ClearAllCollectedData();
        FlowerQuestManager flowerManager = FindAnyObjectByType<FlowerQuestManager>();
        if (flowerManager != null)
        {
            Destroy(flowerManager.gameObject);
        }

        PersistentPlayer player = FindAnyObjectByType<PersistentPlayer>();
        if (player != null)
        {
            Destroy(player.gameObject);
        }

        SceneData.isGameStarted = true;
        SceneManager.LoadScene("Introscene");

        if (NotificationManager.Instance != null)
        {
            NotificationManager.Instance.ShowLocalizedNotification(tableName, sM);
        }
    }

    public void FinishGame()
    {
        SetGameState(GameState.GameOver);
        endPanel.SetActive(true);
    }

    // Resets the game state after the end screen and loads the starting scene with the main menu active
    public void ReturnToMainMenuFromEnd()
    {
        SceneData.isGameStarted = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }

    public void CloseNotify()
    {
        notifyPanel.SetActive(false);
    }
}