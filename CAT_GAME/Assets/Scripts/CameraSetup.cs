using UnityEngine;
using Unity.Cinemachine; 

// Weist der Cinemachine-Kamera beim Szenenstart automatisch den Spieler als Ziel zu
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