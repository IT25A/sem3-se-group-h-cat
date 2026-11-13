using System.Collections.Generic;
using UnityEngine;

public static class TaskCollectibleState
{
    public static int EggCount { get; private set; } = 0;
    public static int FlowerCount { get; private set; } = 0;
    public static int MouseCount { get; private set; } = 0;
    public static int KeyCount { get; private set; } = 0;
    public static int SimbaNecklaceCount { get; private set; } = 0;
    public static int WoodCount { get; private set; } = 0;

    public static bool eggNotificationSent = false;
    public static bool flowerNotificationSent = false;
    public static bool mouseNotificationSent = false;
    public static bool simbaNecklaceNotificationSent = false;
    public static bool keyNotificationSent = false;
    public static bool woodNotificationSent = false;

    // Speichert die eindeutigen IDs aller bereits eingesammelten Gegenstände
    private static readonly HashSet<string> collectedIds = new HashSet<string>();

    // Setzt die Zähler beim Starten des Play-Modus im Unity-Editor sauber zurück
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
            if (!collectedIds.Contains(id))
            {
                collectedIds.Add(id);
                EggCount++;
            }
        }
    }

    public static void RegisterFlower(string id)
    {
        if (!string.IsNullOrEmpty(id))
        {
            collectedIds.Add(id);
        }
        FlowerCount++;
    }

    public static void RegisterMouse(string id)
    {
        if (!string.IsNullOrEmpty(id))
        {
            collectedIds.Add(id);
        }
        MouseCount++;
    }

    public static void RegisterKey(string id)
    {
        if (!string.IsNullOrEmpty(id))
        {
            collectedIds.Add(id);
        }
        KeyCount++;
    }

    public static void RegisterSimbaNecklace(string id)
    {
        if (!string.IsNullOrEmpty(id))
        {
            collectedIds.Add(id);
        }
        SimbaNecklaceCount++;
    }

    public static void RegisterWood(string id)
    {
        if (!string.IsNullOrEmpty(id))
        {
            collectedIds.Add(id);
        }
        WoodCount++;
    }

    // Setzt alle Zähler und Benachrichtigungs-Flags nach Abschluss einer Quest zurück
    public static void ResetTaskCollectibles()
    {
        EggCount = 0;
        FlowerCount = 0;
        MouseCount = 0;
        SimbaNecklaceCount = 0;
        KeyCount = 0;
        WoodCount = 0;

        eggNotificationSent = false;
        flowerNotificationSent = false;
        mouseNotificationSent = false;
        simbaNecklaceNotificationSent = false;
        keyNotificationSent = false;
        woodNotificationSent = false;

        Debug.Log("Task collectibles wurden zurückgesetzt.");
    }

    public static void ClearAllCollectedData()
    {
        collectedIds.Clear();
        Debug.Log("alle gesammelten sachen gecleared");
    }
}