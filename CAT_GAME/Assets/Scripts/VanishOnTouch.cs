using UnityEngine;

public class VanishOnTouch : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Destroys this object as soon as the player touches the trigger
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}