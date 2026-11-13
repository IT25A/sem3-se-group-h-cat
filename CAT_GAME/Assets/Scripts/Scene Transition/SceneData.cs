using System.Collections.Generic;
using UnityEngine;

// Statischer Datenspeicher für szenenübergreifende Spielzustände, Ressourcen und Spawn-Punkte
public static class SceneData
{
    public static string TargetSpawnID; // Ziel-SpawnPoint-ID in der nächsten Szene
    public static string LastSpawnID;   // Zuletzt genutzte Tür-/Spawn-ID für den Rückweg
    public static float savedWater = -1f;
    public static int savedFood = 0; 
    public static int savedMouseCount = 0;
    public static bool mouseCatched = false;
    public static int savedKeyCount = 0;
    public static int savedWoodCount = 0;
    public static bool flowerQuestCompleted = false;
    public static bool isGameStarted = false;

    public static bool isQuizFireWoodKeyCompleted = false;
    public static List<string> savedNotificationHistoryText = new List<string>();    public static List<string> savedNotificationHistoryKeys = new List<string>();
    public static List<Sprite> savedNotificationHistorySprites = new List<Sprite>();

    // Setzt beim Spielstart vor dem Laden der ersten Szene alle gespeicherten Laufzeitdaten zurück
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void ResetOnStart() {
        isGameStarted = false;
        savedWater = -1f;
        savedFood = 0;
        savedMouseCount = 0;
        mouseCatched = false;
        savedKeyCount = 0;
        isQuizFireWoodKeyCompleted = false;
        TargetSpawnID = null;
        LastSpawnID = null;
        savedNotificationHistoryText.Clear();
        savedNotificationHistorySprites.Clear();
        TaskCollectibleState.ClearAllCollectedData();
        flowerQuestCompleted = false;
    }
}

