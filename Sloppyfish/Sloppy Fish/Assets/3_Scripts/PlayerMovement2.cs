using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerMovement2 : MonoBehaviour
{
    public float moveSpeed = 6f;
    
    public float initialMoveDirectionX = 1f;
    public float initialMoveDirectionY = 0f;

    private Vector3 direction;
    private bool isSwiping = false;
    public float dashSpeed = 20f;
    public float dashDuration = 0.25f;
    private bool isDashing = false;

  // public TextMeshProUGUI _text;

    [Header("Camera Stuff")]
    [SerializeField] private GameObject _cameraFollowGO;

    private CameraFollowObject _cameraFollowObject;
    public bool IsFacingRight { get; set; }

    public Joystick joystick;
    public bool isLookingRight = true;

    private void Start()
    {
        // Iniciar el movimiento en la dirección especificada
     //   direction = new Vector3(initialMoveDirectionX, initialMoveDirectionY, 0).normalized;

        _cameraFollowObject = _cameraFollowGO.GetComponent<CameraFollowObject>();
    }

    private void Update()
    {

        // Obtener el input del joystick
        float horizontalInput = joystick.Horizontal;
        float verticalInput = joystick.Vertical;

        // Calcular la dirección del movimiento
        Vector3 movementDirection = new Vector3(horizontalInput, verticalInput, 0).normalized;

        // Verificar si hay input desde el joystick
        if (movementDirection.magnitude >= 0.02f)
        {
            // Mover el jugador en la dirección del joystick
            transform.Translate(movementDirection * moveSpeed * Time.deltaTime, Space.World);
        }

        // Rotar instantáneamente según la dirección horizontal
        if (Mathf.Abs(horizontalInput) > 0.2f)
        {
            // Determinar la dirección correcta para la rotación
            float targetRotation = horizontalInput > 0 ? 0 : 180;


            // Verificar si la nueva dirección de rotación es diferente de la actual
            if (Mathf.Abs(transform.rotation.eulerAngles.y - targetRotation) > 0.1f)
            {
                // Aplicar la rotación
                transform.rotation = Quaternion.Euler(0, targetRotation, 0);
                _cameraFollowObject.CallTurn();
                // Actualizar la variable de dirección inicial
                isLookingRight = horizontalInput > 0;
            }
        }

        // Manejar la entrada para el hitMove
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (!isDashing)
            {
                StartCoroutine(StartDash());

            }
        }
    }

    public void DoADash()
    {
        if (!isDashing)
        {
            StartCoroutine(StartDash());

        }
    }
    // HandleTouchInput();

    //  HandleKeyboardInput();
    /*
    if (!isDashing)
    {
        // Mover en la dirección actual
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Rotar instantáneamente según la dirección horizontal
        if (Mathf.Abs(direction.x) > 0.1f)
        {
            transform.rotation = Quaternion.Euler(0, direction.x > 0 ? 0 : 180, 0);

        }

        // Verificar si estamos intentando hacer un dash
        if (isSwiping && Mathf.Abs(direction.magnitude) > 0.1f)
        {
            //StartCoroutine(StartDash());
        }
    }
    */


    private IEnumerator StartDash()
    {
        
        // Debug.Log("entro en dashh");
        isDashing = true;

        // Guardar la dirección actual antes de cambiarla para el dash
      //  Vector3 originalDirection = direction.normalized;

        // Configurar los valores del dash
        float originalMoveSpeed = moveSpeed;
        moveSpeed = dashSpeed;

        // Iniciar la cuenta regresiva para el final del dash
        float startTime = Time.time;
        while (Time.time < startTime + dashDuration)
        {
            // Aplicar la velocidad de dash en la dirección original
           // transform.position += originalDirection * moveSpeed * Time.deltaTime;

            yield return null;
        }

        // Finalizar el dash
        isDashing = false;
        moveSpeed = originalMoveSpeed;
    }

    /*
    private void HandleTouchInput()
    {
        Touch touch = Input.GetTouch(0);

       
           
            if (touch.phase == TouchPhase.Moved)
            {
                 isSwiping = true;
                Vector2 deltaPosition = touch.deltaPosition.normalized;
                _text.text = "definimos el vector 2 y " + deltaPosition.ToString();
                if (isSwiping)
                {
                    isSwiping = false;
                    // Verificar si es un swipe horizontal
                    if (Mathf.Abs(deltaPosition.x) > Mathf.Abs(deltaPosition.y))
                    {
                        // Verificar si la dirección del swipe es la misma que la dirección actual
                        if (Mathf.Sign(deltaPosition.x) == Mathf.Sign(direction.x))
                        {
                            // Llamar a StartDash solo si no estamos actualmente en un dash
                            if (!isDashing)
                            {
                                StartCoroutine(StartDash());

                            }
                        }
                        else
                        {
                            // Swipe horizontal
                            direction = new Vector3(deltaPosition.x, 0, 0).normalized;
                            _text.text = "hacemos un swipe horizontal ";

                        }


                    }
                    // Verificar si es un swipe vertical
                    else if (Mathf.Abs(deltaPosition.y) > Mathf.Abs(deltaPosition.x))
                    {
                        // Verificar si la dirección del swipe es la misma que la dirección actual
                        if (Mathf.Sign(deltaPosition.y) == Mathf.Sign(direction.y))
                        {
                            // Llamar a StartDash solo si no estamos actualmente en un dash
                            if (!isDashing)
                            {
                                StartCoroutine(StartDash());

                            }
                        }
                        else
                        {
                            // Swipe vertical
                            direction = new Vector3(0, deltaPosition.y, 0).normalized;
                            _text.text = "hacemos un swipe vertical ";

                        }

                    }
                }



        }
     


      
        
    }
    */
    private void HandleKeyboardInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Verificar si hay entrada desde el teclado
        if (Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f)
        {
            // Utilizar la entrada del teclado para la dirección
            Vector3 newDirection = new Vector3(horizontalInput, verticalInput, 0).normalized;

            // Verificar si hay un cambio en la dirección (no un cambio de input)
            if (direction== newDirection)
            {
                // Llamamos al dash si no estamos actualmente en un dash
                if (!isDashing)
                {
                    StartCoroutine(StartDash());
                    //_text.text = "llamo a dash ";
                }
            }
            else
            {
                if (direction.y == newDirection.y)
                    _cameraFollowObject.CallTurn();
                // Si no hay cambio de dirección, actualizamos la dirección
                direction = newDirection;
                
            }
        }
    }
    

}
