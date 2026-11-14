using System.Collections.Generic;
using UnityEngine;

// Static data store for cross-scene game states, resources, and spawn points
public static class SceneData
{
    public static string TargetSpawnID; // Target SpawnPoint ID in the next scene
    public static string LastSpawnID;   // Last used door/spawn ID for returning
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

    // Resets all stored runtime data on game start before loading the first scene
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

