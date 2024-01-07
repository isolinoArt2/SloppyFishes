using UnityEngine;

public class Coins : MonoBehaviour
{
    [System.Obsolete]
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Obtener una referencia al GameManager
            GameManager gameManager = FindObjectOfType<GameManager>();
            FindObjectOfType<AudioManager>().Play("coins");

            if (gameManager != null)
            {
                // Llamar a la función CoinsInGame() del GameManager
                gameManager.CoinsInGame();
            }

            // Destruir la moneda después de ser recogida
            Destroy(gameObject);
        }
    }
}
