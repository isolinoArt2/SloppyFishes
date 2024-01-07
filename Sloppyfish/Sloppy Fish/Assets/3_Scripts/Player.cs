using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
   // public Sprite[] sprites;
    private int spriteIndex;
    private Vector3 direction;
    public float gravity = -9.8f;
    public float strength = 5f;
    public float moveSpeed = 5f; // Nueva variable de velocidad

    public List<GameObject> bubblePrefabs; // Lista de prefabs de burbujas
    public Transform bubbleSpawnPoint; // Punto de spawn de las burbujas

    //[SerializeField] private AudioSource jumpSoundEffect;

    public GameManager gameManager; // Reference to the GameManager
    public DataManager dataManager;

    public float dashCooldown = 3f;
    public float dashDuration = 0.5f; public float dashSpeedMultiplier = 2f;  // Ajusta esto según sea necesario
   
    public GameObject dashFeedbackObject;

    private bool isCooldown = false;
    private float dashCooldownTimer = 0f;
    private float dashTimer = 0f;
    private bool isDashActive = false;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
     //   gameManager = FindObjectOfType<GameManager>(); // Find and store the GameManager reference
    }

    private void Start()
    {
      //  InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
    }

    public void OnPlay()
    {
        Vector3 position = transform.position;
       position.y = 0f;
        transform.position = position;
        direction = Vector3.zero;

    }


    private void Update()
    {
        // Movimiento constante hacia la derecha
        if (!isDashActive)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }

        // Manejar el cooldown del dash
        if (isCooldown)
        {
            dashCooldownTimer += Time.deltaTime;

            if (dashCooldownTimer >= dashCooldown)
            {
                isCooldown = false;
                dashCooldownTimer = 0f;

                // Activar el feedback de dash disponible
                if (dashFeedbackObject != null)
                    dashFeedbackObject.SetActive(true);
            }
        }
        // Manejar la entrada para el dash
        if (Input.GetKeyDown(KeyCode.X))
        {
            // Iniciar el dash solo si no estamos en cooldown
            if (!isCooldown)
            {
                StartCoroutine(Dash());
            }
        }

        // Manejar la entrada para el salto
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            FindObjectOfType<AudioManager>().Play("jump");
            direction = Vector3.up * strength;
            BubbleSpawn();
        }

        // Actualizar posición y gravedad
        direction.y += gravity * Time.deltaTime;

        // Solo aplicar la posición durante el dash si no estamos en dash
        if (!isDashActive)
        {
            transform.position += direction * Time.deltaTime;
        }

    }


    private IEnumerator Dash()
    {
        // Desactivar el feedback de dash disponible
        if (dashFeedbackObject != null)
            dashFeedbackObject.SetActive(false);

        // Iniciar el dash
        isDashActive = true;
        float startTime = Time.time;
        while (Time.time < startTime + dashDuration)
        {
            // Aplicar la velocidad de dash en el eje X
            transform.Translate(Vector3.right * moveSpeed * dashSpeedMultiplier * Time.deltaTime);

            // No modificar la posición en y durante el dash
            yield return null;
        }

        // Finalizar el dash
        isDashActive = false;

        // Iniciar el cooldown
        isCooldown = true;
        dashCooldownTimer = 0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
           // Transform obstacleParent = other.transform.parent; // Obtén la referencia al objeto padre
           // Destroy(obstacleParent.transform.gameObject);
            gameManager.GameOver(); // Call the GameManager's GameOver method through the reference
            OnPlay();
        }
        else if (other.gameObject.CompareTag("Scoring"))
        {
            //gameManager.IncreaseScore(); // Call the GameManager's IncreaseScore method through the reference
            // dataManager.IncreaseScore(1);
            dataManager.SendMessage("IncreaseScore", 1);
        }
        else if (other.gameObject.CompareTag("Mines"))
        {
            Transform obstacleParent = other.transform.parent; // Obtén la referencia al objeto padre
            Destroy(obstacleParent.transform.gameObject);
            gameManager.GameOver(); // Call the GameManager's GameOver method through the reference
        }
    }

    public void BubbleSpawn()
    {
        // Verificar si la lista de burbujas está vacía
        if (bubblePrefabs.Count == 0)
        {
            Debug.LogWarning("La lista de bubblePrefabs está vacía.");
            return;
        }

        // Seleccionar un prefab de burbuja aleatorio de la lista
        int indicePrefabAleatorio = Random.Range(0, bubblePrefabs.Count);
        GameObject prefabSeleccionado = bubblePrefabs[indicePrefabAleatorio];

        // Instanciar el prefab de burbuja en el punto de spawn
        if (bubbleSpawnPoint != null)
        {
            Instantiate(prefabSeleccionado, bubbleSpawnPoint.position, bubbleSpawnPoint.rotation);
        }
        else
        {
            Debug.LogWarning("El punto de spawn de burbujas (bubbleSpawnPoint) no está asignado en el Inspector.");
        }
    }
}
