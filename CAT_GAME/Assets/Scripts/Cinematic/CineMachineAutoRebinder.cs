using UnityEngine;
using Unity.Cinemachine;

public class CinemachineAutoRebinder : MonoBehaviour
{
    private CinemachineCamera vcam;

    void Awake()
    {
        // Cache the CinemachineCamera component attached to this GameObject
        vcam = GetComponent<CinemachineCamera>();
    }

    void OnEnable()
    {
        // Subscribe to the scene loaded event to rebind the target on scene transitions
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unsubscribe from the scene loaded event to prevent memory leaks
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        // Automatically search and follow the player when a new scene finishes loading
        FindAndFollowPlayer();
    }

    void Start()
    {
        // Initial check when the script starts
        FindAndFollowPlayer();
    }

    void FindAndFollowPlayer()
    {
        // Search for the player/cat by its Tag (make sure the cat has the "Player" tag assigned)
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && vcam != null)
        {
            // Assign the player's transform to Cinemachine so it starts tracking immediately
            vcam.Follow = player.transform;
        }
        else
        {
            Debug.LogWarning("CinemachineAutoRebinder: Player with the 'Player' tag could not be found.");
        }
    }
}