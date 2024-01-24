using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertaTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Obtener una referencia al objeto "Player" y enviar el mensaje "GetHit"
            GameObject playerObject = other.gameObject;
            if (playerObject != null)
            {
                playerObject.SendMessage("OpenDoor");
            }


        }

        
    }
}
