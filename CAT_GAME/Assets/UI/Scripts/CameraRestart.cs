using UnityEngine;
using Unity.Cinemachine; 

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