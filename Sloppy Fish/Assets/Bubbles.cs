using UnityEngine;

public class Bubbles : MonoBehaviour
{
    public float velocidadVertical = 1.0f; // Velocidad vertical constante
    public float amplitudHorizontal = 0.1f; // Amplitud del movimiento horizontal
    public float frecuenciaHorizontal = 1.0f; // Frecuencia del movimiento horizontal

    public float tiempoDeVida = 4.0f; // Tiempo de vida antes de destruirse

    private Vector3 initialPosition;
    private float tiempoPasado = 0f;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        // Mover la burbuja hacia arriba de forma constante
        transform.Translate(Vector3.up * velocidadVertical * Time.deltaTime);

        // Oscilar en el eje X
        float newX = initialPosition.x + Mathf.Sin(Time.time * frecuenciaHorizontal) * amplitudHorizontal;
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        // Contar el tiempo transcurrido
        tiempoPasado += Time.deltaTime;

        // Destruir el objeto después de 4 segundos
        if (tiempoPasado >= tiempoDeVida)
        {
            Destroy(gameObject);
        }
    }
}
