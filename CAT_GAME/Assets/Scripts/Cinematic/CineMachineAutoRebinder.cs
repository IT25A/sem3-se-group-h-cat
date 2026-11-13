using UnityEngine;
using Unity.Cinemachine;

// Verbindet die Cinemachine-Kamera beim Start und nach jedem Szenenwechsel automatisch mit dem Spieler
public class CinemachineAutoRebinder : MonoBehaviour
{
    private CinemachineCamera vcam;

    void Awake()
    {
        vcam = GetComponent<CinemachineCamera>();
    }

    void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        FindAndFollowPlayer();
    }

    void Start()
    {
        FindAndFollowPlayer();
    }

    void FindAndFollowPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && vcam != null)
        {
            vcam.Follow = player.transform;
        }
        else
        {
            Debug.LogWarning("CinemachineAutoRebinder: Player with the 'Player' tag could not be found.");
        }
    }
}