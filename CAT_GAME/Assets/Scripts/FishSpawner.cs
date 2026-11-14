using System.Collections;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public GameObject fishPrefab;       // Dein Fisch-Prefab
    public Transform[] spawnPoints;     // Die 3 Spawn-Punkte
    public float minSpawnDelay = 2f;    // Kürzeste Pause zwischen Sprüngen
    public float maxSpawnDelay = 5f;    // Längste Pause zwischen Sprüngen

    private bool[] pointOccupied;       // Merkt sich, welcher Punkt belegt ist

    void Start()
    {
        pointOccupied = new bool[spawnPoints.Length];
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // Warte die zufällige Zeit bis zum nächsten Sprung ab
            float waitTime = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(waitTime);

            // Wähle einen zufälligen Index
            int randomIndex = Random.Range(0, spawnPoints.Length);

            // Prüfe, ob dieser Punkt gerade frei ist, sonst suche einen freien
            int attempts = 0;
            while (pointOccupied[randomIndex] && attempts < spawnPoints.Length)
            {
                randomIndex = (randomIndex + 1) % spawnPoints.Length;
                attempts++;
            }

            // Wenn der Punkt frei ist, lass den Fisch springen
            if (!pointOccupied[randomIndex])
            {
                StartCoroutine(SpawnFishAtPoint(randomIndex));
            }
        }
    }

    IEnumerator SpawnFishAtPoint(int index)
    {
        pointOccupied[index] = true;
        Transform targetPoint = spawnPoints[index];

        // Fisch erzeugen
        GameObject spawnedFish = Instantiate(fishPrefab, targetPoint.position, targetPoint.rotation);

        // Finde die Länge der Animation heraus oder setze einen festen Wert (z.B. 0.7 Sekunden bei 7 Frames und 10 FPS)
        float animLength = 0.7f; 
        Animator animator = spawnedFish.GetComponent<Animator>();
        if (animator != null)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.length > 0)
                animLength = stateInfo.length;
        }

        // Warte, bis die Animation durchgelaufen ist
        yield return new WaitForSeconds(animLength);

        // Fisch zerstören
        Destroy(spawnedFish);

        // Punkt wieder als frei markieren
        pointOccupied[index] = false;
    }
}