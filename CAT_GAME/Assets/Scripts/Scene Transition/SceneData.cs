/// Static container class used to pass spawn point data across scene transitions.
using System.Collections.Generic;
using UnityEngine;
public static class SceneData
{
    // Stores the target spawn point ID for the destination scene
    public static string TargetSpawnID;
    // Stores the ID of the last spawn point/door the player used
    public static string LastSpawnID;
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

