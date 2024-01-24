using UnityEngine;

public class Mines : MonoBehaviour
{
    public GameObject parent;
    public bool isLimit;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Obtener una referencia al objeto "Player" y enviar el mensaje "GetHit"
            GameObject playerObject = other.gameObject;
            if (playerObject != null)
            {
                playerObject.SendMessage("GetHit");
            }

            if (!isLimit)
            {
                Destroy(parent);
            }
           
        }

        if (other.CompareTag("Weapon"))
        {
            // Destruir el objeto "Mines"
            Destroy(other.gameObject);

            // Destruir el proyectil o el arma (dependiendo de cómo esté organizada la jerarquía)
            if (!isLimit)
            {
                Destroy(parent);
            }
        }

        if (other.CompareTag("Bowl"))
        {
            GameObject playerObject = GameObject.Find("Player");
            if (playerObject != null)
            {
                playerObject.SendMessage("GetHit");
            }
            // Destruir el objeto "Mines"
            Destroy(other.gameObject);

            // Destruir el proyectil o el arma (dependiendo de cómo esté organizada la jerarquía)
            if (!isLimit)
            {
                Destroy(parent);
            }
        }
    }
}