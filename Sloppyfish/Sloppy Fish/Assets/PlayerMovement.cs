using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gravity = -13f;
    public float jumpStrength = 4.3f;
    public float dashSpeed = 10f;
    public float dashDuration = 0.25f;
    public float hitMoveDuration = 0.22f;
    public float dashSpeedMultiplier = 2f;
    public float maxFallSpeed = -10f;

    private int movementDirection = 1;
    private Vector3 direction;

    private bool isDashing = false;
    private float originalMoveSpeed;
    private float originalGravity;

    [Header("Camera Stuff")]
    [SerializeField] private GameObject _cameraFollowGO;

    private CameraFollowObject _cameraFollowObject;
    public bool IsFacingRight { get; set; }

    private void Start()
    {
        _cameraFollowObject = _cameraFollowGO.GetComponent<CameraFollowObject>();
    }
    private void Update()
    {
        // Verificar si estamos en un dash
        if (isDashing)
        {
            DashUpdate();
        }
        else
        {
            // Mover en la dirección actual
            transform.position += Vector3.right * moveSpeed * movementDirection * Time.deltaTime;

            // Aplicar la gravedad
            direction.y += gravity * Time.deltaTime;

            // Limitar la velocidad de caída
            direction.y = Mathf.Max(direction.y, maxFallSpeed);

            // Aplicar la dirección actual
            transform.position += direction * Time.deltaTime;
        }

        // Rotar según la dirección
        transform.rotation = Quaternion.Euler(0, movementDirection == 1 ? 0 : 180, 0);

        // Manejar la entrada para el salto
        if (Input.GetKey(KeyCode.Space) || IsTap())
        {
           // Jump();
        }

        // Manejar la entrada para el dash
        if ((Input.GetKeyDown(KeyCode.X) && !isDashing) || (IsTouchOnRight() && !isDashing))
        {
            //StartDash(1);
            Jump(1);
            
        }
        if ((Input.GetKeyDown(KeyCode.Z) && !isDashing) || (IsTouchOnLeft() && !isDashing))
        {
           // StartDash(-1);
            Jump(-1);
            
        }


        // Manejar la entrada para el hitMove
        if (Input.GetKeyDown(KeyCode.D))
        {
            HitMove();
        }
    }
    private bool IsTouchOnRight()
    {
        if (Input.touchCount > 0)
        {
            float screenWidth = Screen.width;
            float touchX = Input.GetTouch(0).position.x;

            return touchX > screenWidth / 2;
        }
        return false;
    }

    private bool IsTouchOnLeft()
    {
        if (Input.touchCount > 0)
        {
            return !IsTouchOnRight();
        }
        return false;
    }
    private bool IsTap()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;

    }

    private bool IsSwipeRight()
    {
        if (Input.touchCount > 0)
        {
            Vector2 deltaPosition = Input.GetTouch(0).deltaPosition;
            return Mathf.Abs(deltaPosition.x) > Mathf.Abs(deltaPosition.y) && deltaPosition.x > 0;
        }
        return false;
    }

    private bool IsSwipeLeft()
    {
        if (Input.touchCount > 0)
        {
            Vector2 deltaPosition = Input.GetTouch(0).deltaPosition;
            return Mathf.Abs(deltaPosition.x) > Mathf.Abs(deltaPosition.y) && deltaPosition.x < 0;
        }
        return false;
    }

    private void StartDash(int direction)
    {
        // Iniciar el dash
        isDashing = true;
        originalMoveSpeed = moveSpeed;
        originalGravity = gravity;

        // Configurar los valores del dash
        moveSpeed = dashSpeed;
        gravity = 0;

        // Restaurar la dirección al finalizar el Dash
        movementDirection = direction;

        // Iniciar la cuenta regresiva para el final del dash
        Invoke("EndDash", dashDuration);
    }

    public void HitMove()
    {
        // Obtener la dirección actual
        int hitMoveDirection = movementDirection * -1; // Dash en la dirección contraria

        // Cambiar la velocidad de movimiento y gravedad durante el hitMove
        StartCoroutine(PerformHitMove(hitMoveDirection));
    }

    private IEnumerator PerformHitMove(int direction)
    {
        // Guardar los valores originales de movimiento y gravedad
        float originalMoveSpeed = moveSpeed;
        float originalGravity = gravity;

        // Iniciar el dash
        isDashing = true;

        // Configurar los valores del hitMove
        moveSpeed = dashSpeed;
        gravity = 0;

        // No rotar al jugador durante el hitMove
        transform.rotation = Quaternion.identity;

        float startTime = Time.time;

        // Duración del hitMove
        while (Time.time < startTime + hitMoveDuration)
        {
            // Aplicar la velocidad de dash en el eje X
            transform.Translate(Vector3.right * moveSpeed * (dashSpeedMultiplier * -direction) * Time.deltaTime * direction);

            // No modificar la posición en y durante el hitMove
            yield return null;
        }

        // Finalizar el dash
        isDashing = false;

        // Restaurar los valores originales de movimiento y gravedad
        moveSpeed = originalMoveSpeed;
        gravity = originalGravity;

        // Restaurar la dirección al finalizar el hitMove
        movementDirection = direction * -1;
    }

    private void DashUpdate()
    {
        // Movimiento durante el dash
        transform.position += Vector3.right * moveSpeed * movementDirection * Time.deltaTime;
    }

    private void EndDash()
    {
        // Restaurar los valores al finalizar el dash
        isDashing = false;
        moveSpeed = originalMoveSpeed;
        gravity = originalGravity;
    }

    private void Jump(int _direction)
    {
        direction.y = jumpStrength;
        // Restaurar la dirección al finalizar el Dash
        if(movementDirection != _direction)
            _cameraFollowObject.CallTurn();
        movementDirection = _direction;
    }
}
