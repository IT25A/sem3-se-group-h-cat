using UnityEngine;
using Unity.Cinemachine; 

// Weist der Cinemachine-Kamera beim Start automatisch das Spieler-Objekt als Verfolgungsziel zu
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