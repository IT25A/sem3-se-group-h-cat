using UnityEngine;
using Unity.Cinemachine; 

// Automatically assigns the player object as the follow target for the Cinemachine camera on start
public class CameraSetup : MonoBehaviour {
    void Start() {
        CinemachineCamera vcam = GetComponent<CinemachineCamera>();
        
        if (vcam != null) {
            GameObject player = GameObject.FindWithTag("Player");
            
            if (player != null) {
                vcam.Follow = player.transform;
            }
        }
    }
}