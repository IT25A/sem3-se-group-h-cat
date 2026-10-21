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

    public static bool isGameStarted = false;
    public static List<string> savedNotificationHistory = new List<string>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void ResetOnStart() {
        isGameStarted = false;
        savedWater = -1f;
        savedFood = 0;
        TargetSpawnID = null;
        LastSpawnID = null;
        savedNotificationHistory.Clear();
    }
}

