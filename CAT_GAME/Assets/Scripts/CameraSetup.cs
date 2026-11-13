using UnityEngine;
using Unity.Cinemachine; 

// Automatically assigns the player as the target for the Cinemachine camera on scene start
public class CamerasSetup : MonoBehaviour {
    void Start() {
        CinemachineCamera cam = GetComponent<CinemachineCamera>();

        if (cam != null) {
            GameObject player = GameObject.FindWithTag("Player");
            
            if (player != null) {
                cam.Follow = player.transform;
            }
        }
    }
}