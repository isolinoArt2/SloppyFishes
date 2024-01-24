using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using GooglePlayGames.BasicApi;
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

    private GameObject canvas;
    public int _StarsUnlocked = 1;

    private int _coinsOnLevel;
    private int _coins = 0;

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
        Debug.Log("cantidad de stars unlocked at start = " + _StarsUnlocked);
        //  InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
        _StarsUnlocked = 1;
        _coins = 0;
    }

    public void OnPlay()
    {
        /*
        Vector3 position = transform.position;
       position.y = 0f;
        transform.position = position;
        direction = Vector3.zero;
        */
       // gravity = -13;
    }

    public void GetStars(int _star)
    {
        _StarsUnlocked += _star;
    }
    public void GetCoinsOnLevel(int _lvlCoins)
    {
        _coinsOnLevel = _lvlCoins;
    }
    public void CoinsInGame()
    {
        // Sumar 1 a la variable _coins
        _coins++;
        if (_coins >= _coinsOnLevel)
            _StarsUnlocked ++;
    }
    public void OpenDoor()
    {
     
        if (PlayerPrefs.GetInt("Boots") >= 1)
        {
            _StarsUnlocked += 1;
        }

        canvas = GameObject.FindGameObjectWithTag("Canvas");
        if (canvas != null)
        {
            Debug.Log("cantidad de stars unlocked = " + _StarsUnlocked);
            // Acceder a los componentes del jugador si es necesario
            LevelComplete canvasComponent = canvas.GetComponent<LevelComplete>();
            canvasComponent.OnLevelComplete(_StarsUnlocked);
            
        }
        else
        {
            return;
            //  Debug.LogError("No se encontró el objeto del jugador.");
        }
    }

    public CinemachineVirtualCamera GetVirtualCamera()
    {
        // Aquí, asumimos que la cámara virtual está directamente en el objeto Player.
        // Si está en otro lugar, necesitarás ajustar esto en consecuencia.
        return GetComponentInChildren<CinemachineVirtualCamera>();
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
            //StartCoroutine(DamashBackDash());
            bowlIsActive = false;
            _StarsUnlocked -= 1;
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
