using UnityEngine;
using Unity.Cinemachine; 

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