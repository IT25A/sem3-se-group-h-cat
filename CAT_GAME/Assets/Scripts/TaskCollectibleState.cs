using System.Collections.Generic;
using UnityEngine;

public static class TaskCollectibleState
{
    public static int EggCount { get; private set; } = 0;
    public static int FlowerCount { get; private set; } = 0;

    // Stores unique IDs of collectibles picked up during the current task
    private static readonly HashSet<string> collectedIds = new HashSet<string>();

    // Ensures clean state when entering Play Mode in the Unity Editor
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Init()
    {
        ResetTaskCollectibles();
    }

    public static bool IsCollected(string id)
    {
        return !string.IsNullOrEmpty(id) && collectedIds.Contains(id);
    }

    public static void RegisterEgg(string id)
    {
        if (!string.IsNullOrEmpty(id))
        {
            collectedIds.Add(id);
        }
        EggCount++;
    }

    public static void RegisterFlower(string id)
    {
        if (!string.IsNullOrEmpty(id))
        {
            collectedIds.Add(id);
        }
        FlowerCount++;
    }

    // Call this when your task/event is completed!
    public static void ResetTaskCollectibles()
    {
        EggCount = 0;
        FlowerCount = 0;
        collectedIds.Clear();
        Debug.Log("Task collectibles wurden zurückgesetzt.");
    }
}