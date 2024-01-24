using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHit : MonoBehaviour
{
    public string targetTag = "Mines";
    public bool isBowl;
    public GameObject parent;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            // Destruir el objeto "Mines"
            Destroy(other.gameObject);

           

            // Destruir el proyectil o el arma (dependiendo de cómo esté organizada la jerarquía)
            Destroy(parent);
        }
    }
}
