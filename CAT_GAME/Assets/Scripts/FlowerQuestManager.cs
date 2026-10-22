using UnityEngine;

public class FlowerQuestManager : MonoBehaviour
{
    public static FlowerQuestManager Instance { get; private set; }

    // Dieses Feld erscheint gleich im Inspector, damit du die Zahl beim Testen ändern kannst
    public int currentFlowerCount = 0; 

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
}