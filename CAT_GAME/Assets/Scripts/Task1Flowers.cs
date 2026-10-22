using UnityEngine;

public class QuestFlower : MonoBehaviour
{
    private string flowerID;
    private bool isCollected = false;

    private void Awake()
    {
        // Creamos un ID único para esta flor basado en su posición en el mundo y el nombre de la escena
        // (Ejemplo: "SceneName_X12.5_Y5.1")
        flowerID = gameObject.scene.name + "_" + transform.position.x + "_" + transform.position.y;
    }

    private void Start()
    {
        // Al iniciar la escena, comprobamos si ya fue recogida en el pasado
        if (FlowerQuestManager.Instance != null && FlowerQuestManager.Instance.IsFlowerCollected(flowerID))
        {
            // Si ya la habían recogido, la destruimos inmediatamente para que no aparezca
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;

            // Le avisamos al manager pasándole su ID único
            if (FlowerQuestManager.Instance != null)
            {
                FlowerQuestManager.Instance.CollectFlower(flowerID);
            }

            // Destruimos la flor actual
            Destroy(gameObject);
        }
    }
}