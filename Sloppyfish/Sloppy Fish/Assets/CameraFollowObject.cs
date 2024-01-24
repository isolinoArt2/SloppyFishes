using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Transform _playerTransform;

    [Header("Flip Rotation Stats")]
    [SerializeField] private float _flipRotationTime;

    private Coroutine _turnCoroutine;

   // private PlayerMovement _player;
   private PlayerMovement2 _player2;

    private bool _isFacingRight;


    private void Awake()
    {
        //_player = _playerTransform.gameObject.GetComponent<PlayerMovement>();
       _player2 = _playerTransform.gameObject.GetComponent<PlayerMovement2>();

       // _isFacingRight = _player.IsFacingRight;
       _isFacingRight = _player2.IsFacingRight;
    }
   
    // Update is called once per frame
    void Update()
    {
        transform.position = _playerTransform.position;
    }

    public void CallTurn()
    {
        _turnCoroutine = StartCoroutine(FlipYLerp());
    }
    private IEnumerator FlipYLerp()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = DetermineEndRotation();
        float yRotation = 0f;

        float elapsedTime = 0f;
        while(elapsedTime < _flipRotationTime)
        {
            elapsedTime += Time.deltaTime;

            yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime / _flipRotationTime));
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            yield return null;
        }

    }

    private float DetermineEndRotation()
    {
        _isFacingRight = !_isFacingRight;

        if (_isFacingRight)
        {
            return 180f;
        }
        else
        {
            return 0f;
        }
    }
}
