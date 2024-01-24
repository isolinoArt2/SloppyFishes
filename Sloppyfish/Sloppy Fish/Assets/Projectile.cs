using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f; // Velocidad del proyectil
    public float lifetime = 2f; // Tiempo de vida del proyectil en segundos

    void Start()
    {
        // Iniciar el temporizador de vida del proyectil
        Invoke("DestroyProjectile", lifetime);
    }

    void Update()
    {
        // Mover el proyectil hacia la derecha
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    void DestroyProjectile()
    {
        // Destruir el proyectil cuando el temporizador de vida expire
        Destroy(gameObject);
    }
}
