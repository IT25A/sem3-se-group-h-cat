using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class CameraZoomOut : MonoBehaviour
{
    public float targetOrthographicSize;
    public float zoomDuration = 3f;
    public float startDelay = 0.5f;
    public CinemachineCamera cinemachineCamera;
    public Camera normalCamera;
    public PlayerMovement playerMovement;

    private bool isZooming = false;

    void Awake()
    {
        if (cinemachineCamera == null)
            cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();

        if (normalCamera == null)
            normalCamera = Camera.main;
    }

    void Start()
    {
        if (playerMovement == null)
            playerMovement = FindFirstObjectByType<PlayerMovement>();

        // Kamera-Zoom automatisch starten, sobald der Spieler seinen Zielpunkt erreicht hat
        if (playerMovement != null)
        {
            playerMovement.onReachedTarget.AddListener(TriggerZoom);
        }
    }

    void OnDestroy()
    {
        if (playerMovement != null)
        {
            playerMovement.onReachedTarget.RemoveListener(TriggerZoom);
        }
    }

    [ContextMenu("Trigger Zoom Test")]
    public void TriggerZoom()
    {
        if (!isZooming && gameObject.activeInHierarchy)
        {
            StartCoroutine(ZoomRoutine());
        }
    }

    // Interpoliert die orthografische Kameragröße weich über die festgelegte Dauer
    private IEnumerator ZoomRoutine()
    {
        isZooming = true;

        if (startDelay > 0f)
            yield return new WaitForSeconds(startDelay);

        float startSize = GetCurrentSize();
        float elapsed = 0f;

        while (elapsed < zoomDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / zoomDuration);
            float current = Mathf.Lerp(startSize, targetOrthographicSize, t);
            SetSize(current);
            yield return null;
        }

        SetSize(targetOrthographicSize);
    }

    private float GetCurrentSize()
    {
        if (cinemachineCamera != null)
            return cinemachineCamera.Lens.OrthographicSize;
        if (normalCamera != null)
            return normalCamera.orthographicSize;
        return 5f;
    }

    private void SetSize(float size)
    {
        if (cinemachineCamera != null)
        {
            var lens = cinemachineCamera.Lens;
            lens.OrthographicSize = size;
            cinemachineCamera.Lens = lens;
        }
        else if (normalCamera != null)
        {
            normalCamera.orthographicSize = size;
        }
    }
}
