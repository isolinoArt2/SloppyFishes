using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerDuplica : MonoBehaviour
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

    private GameManager gameManager; // Reference to the GameManager
   // public DataManager dataManager;

    public float dashCooldown = 1f;
    public float dashDuration = 0.5f; public float dashSpeedMultiplier = 2f;  // Ajusta esto según sea necesario
    public float backdashDuration = 0.5f; public float backdashSpeedMultiplier = 2f;  // Ajusta esto según sea necesario

    public GameObject dashFeedbackObject;

    private bool isCooldown = false;
    private float dashCooldownTimer = 0f;
    private float dashTimer = 0f;
    private bool isDashActive = false;

    public bool bowlIsActive;
    private bool desactiveBowl = false;
    private GameObject bowlInstance; // Variable para almacenar la instancia del bowl

    public CinemachineVirtualCamera virtualCamera;
    public float slowMotionDuration = 1.5f; // Duración de la cámara lenta en segundos
    public float slowMotionTimeScale = 0.25f; // Escala de tiempo durante la cámara lenta
    public float targetOrthoSize = 3f;
    public float returnDuration = 1f;
    public float zoomInDuration = 0.1f;
    public float zoomOutDuration = 0.1f;

    // Añadir una variable para controlar la dirección del movimiento
    private int movementDirection = 1; // 1 para derecha, -1 para izquierda

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
     //   gameManager = FindObjectOfType<GameManager>(); // Find and store the GameManager reference
    }

    private void Start()
    {
        gameManager = GameManager.instance;
      //  InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
    }

    public void OnPlay()
    {
        /*
        Vector3 position = transform.position;
       position.y = 0f;
        transform.position = position;
        direction = Vector3.zero;
        */
        gravity = -13;
    }


    private void Update()
    {
     

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
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // Iniciar el dash solo si no estamos en cooldown
            if (!isCooldown)
            {
                // Cambiar la dirección y rotación para BackDash
                StartCoroutine(BackDash());
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            // Iniciar el dash solo si no estamos en cooldown
            if (!isCooldown)
            {
                // Cambiar la dirección y rotación para Dash
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

        // Aplicar la posición durante el dash si no estamos en dash y también permitir movimiento vertical
        if (!isDashActive)
        {
            if (movementDirection == 1) // Mover a la derecha
            {
                transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            }
            else if (movementDirection == -1) // Mover a la izquierda
            {
                transform.position += Vector3.left * moveSpeed * Time.deltaTime;
            }

            transform.position += direction * Time.deltaTime;
        }

    }
    public CinemachineVirtualCamera GetVirtualCamera()
    {
        // Aquí, asumimos que la cámara virtual está directamente en el objeto Player.
        // Si está en otro lugar, necesitarás ajustar esto en consecuencia.
        return GetComponentInChildren<CinemachineVirtualCamera>();
    }

    private IEnumerator Dash()
    {
        // Cambiar la dirección y rotación para Dash
        movementDirection = 1;
        transform.rotation = Quaternion.identity;

        // Guardar y ajustar la gravedad durante el Dash
        float originalGravity = gravity;
        gravity = 0f;

        // Resto del código del Dash
        float startTime = Time.time;
        while (Time.time < startTime + dashDuration)
        {
            // Aplicar la velocidad de dash en el eje X
            transform.Translate(Vector3.right * moveSpeed * dashSpeedMultiplier * Time.deltaTime);

            // No modificar la posición en y durante el dash
            yield return null;
        }

        // Al finalizar el Dash, restaurar la dirección y rotación
        movementDirection = 1;
        transform.rotation = Quaternion.identity;

        // Restaurar la gravedad al valor original
        gravity = originalGravity;

        // Iniciar el cooldown
        isCooldown = true;
        dashCooldownTimer = 0f;
    }

    private IEnumerator BackDash()
    {
        // Cambiar la dirección y rotación para BackDash
        movementDirection = -1;
        transform.rotation = Quaternion.Euler(0, 180, 0);

        // Guardar y ajustar la gravedad durante el Dash
        float originalGravity = gravity;
        gravity = 0f;

        // Resto del código del BackDash
        float startTime = Time.time;
        while (Time.time < startTime + backdashDuration)
        {
            // Aplicar la velocidad de dash en el eje X
            transform.Translate(Vector3.right * moveSpeed * backdashSpeedMultiplier * Time.deltaTime);

            // No modificar la posición en y durante el dash
            yield return null;
        }

        // Al finalizar el BackDash, restaurar la dirección y rotación
        movementDirection = -1;
        transform.rotation = Quaternion.Euler(0, 180, 0);

        // Restaurar la gravedad al valor original
        gravity = originalGravity;

        // Iniciar el cooldown
        isCooldown = true;
        dashCooldownTimer = 0f;

    }

    private IEnumerator DamashBackDash()
    {
        // Desactivar el feedback de dash disponible
        if (dashFeedbackObject != null)
            dashFeedbackObject.SetActive(false);

        // Iniciar el dash
        isDashActive = true;
        float startTime = Time.time;
        while (Time.time < startTime + 0.1f)
        {
            // Aplicar la velocidad de dash en el eje X
            transform.Translate(Vector3.left * moveSpeed * 2 * Time.deltaTime);
            
            // No modificar la posición en y durante el dash
            yield return null;
        }

        // Finalizar el dash
        isDashActive = false;
      //  bowlIsActive = false;
        // Iniciar el cooldown
        isCooldown = true;
        dashCooldownTimer = 0f;
    }

    public void GetItem(GameObject itemPrefab)
    {
        StartCoroutine(TriggerSlowMotion(itemPrefab));
       
        // Puedes agregar más lógica aquí, como cambiar el comportamiento del objeto recolectado, actualizar puntuaciones, etc.
    }

    IEnumerator TriggerSlowMotion(GameObject itemPrefab)
    {
        if (virtualCamera == null)
        {
            Debug.LogError("Virtual camera is not assigned.");
            yield break;
        }

        // Obtén el componente CinemachineFramingTransposer
        CinemachineComponentBase componentBase = virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        if (componentBase is CinemachineFramingTransposer)
        {
            // Guarda el valor original de la distancia de la cámara
            float originalCameraDistance = (componentBase as CinemachineFramingTransposer).m_CameraDistance;

            // Establece el valor de la distancia de la cámara para el zoom in
            (componentBase as CinemachineFramingTransposer).m_CameraDistance = 0.5f;

            // Ajustar la escala de tiempo para la desaceleración general del tiempo
            Time.timeScale = slowMotionTimeScale;

            // Zoom In suave
            float elapsedTime = 0f;
            while (elapsedTime < zoomInDuration)
            {
                // Interpola suavemente entre el valor original y 1 usando SmoothStep
                float smoothStepValue = Mathf.SmoothStep(0f, 1f, elapsedTime / zoomInDuration);
                float newCameraDistance = Mathf.Lerp(originalCameraDistance, 1, smoothStepValue);

                // Establece el nuevo valor de la distancia de la cámara
                (componentBase as CinemachineFramingTransposer).m_CameraDistance = newCameraDistance;

                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            // Pausar el tiempo
            Time.timeScale = 0;
            // Pausa adicional después de recoger el objeto
            yield return new WaitForSecondsRealtime(0.5f);

            // Obtener el prefab y activar la cámara lenta si es necesario
            if (itemPrefab.CompareTag("Bowl"))
            {
                // Instanciar el objeto recolectado como hijo del jugador y almacenar la instancia en bowlInstance
                bowlInstance = Instantiate(itemPrefab, transform.position, Quaternion.identity);
                bowlInstance.transform.SetParent(transform);
                bowlIsActive = true;
            }
            else
            {
                // Instanciar el objeto recolectado como hijo del jugador
                GameObject collectedItem = Instantiate(itemPrefab, transform.position, Quaternion.identity);
                collectedItem.transform.SetParent(transform);

                // Puedes agregar más lógica aquí, como cambiar el comportamiento del objeto recolectado, actualizar puntuaciones, etc.
            }

            // Pausa adicional después de recoger el objeto
            yield return new WaitForSecondsRealtime(0.5f);

            // Restaura la distancia original de la cámara después del tiempo de espera
            (componentBase as CinemachineFramingTransposer).m_CameraDistance = originalCameraDistance;

            // Zoom Out
            elapsedTime = 0f;
            while (elapsedTime < zoomOutDuration)
            {
                // Puedes agregar más ajustes aquí según sea necesario

                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            // Espera un breve momento después del zoom out
            yield return new WaitForSecondsRealtime(0.5f);

            // Restaurar la escala de tiempo a su valor original
            Time.timeScale = 1;
        }
        else
        {
            Debug.LogError("CinemachineFramingTransposer component not found on the virtual camera.");
            yield break;
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        /*
        if (other.gameObject.CompareTag("Mines") && !bowlIsActive && desactiveBowl)
        {
            Transform obstacleParent = other.transform.parent; // Obtén la referencia al objeto padre
            Destroy(obstacleParent.transform.gameObject);
            gameManager.GameOver(); // Call the GameManager's GameOver method through the reference
        }
          
      

        if (other.gameObject.CompareTag("Obstacle"))
        {
            // Transform obstacleParent = other.transform.parent; // Obtén la referencia al objeto padre
            // Destroy(obstacleParent.transform.gameObject);
            gameManager.GameOver(); // Call the GameManager's GameOver method through the reference
            OnPlay();
        }

        if (other.gameObject.CompareTag("Scoring"))
        {
            //gameManager.IncreaseScore(); // Call the GameManager's IncreaseScore method through the reference
            // dataManager.IncreaseScore(1);
            dataManager.SendMessage("IncreaseScore", 1);
        }
        */

    }

    public void GetHit()
    {
        if (!bowlIsActive)
        {
           // Transform obstacleParent = other.transform.parent; // Obtén la referencia al objeto padre
          //  Destroy(obstacleParent.transform.gameObject);
            gameManager.GameOver(); // Call the GameManager's GameOver method through the reference
        }
        else
        {
            StartCoroutine(DamashBackDash());
            bowlIsActive = false;
        }
    }
    /*
    public void DeactivateBowl()
    {
        
        StartCoroutine(DamashBackDash());
        desactiveBowl = true;
    }

    */

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
