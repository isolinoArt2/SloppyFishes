using UnityEngine;

public class RotatingWeapon : MonoBehaviour
{
    public float rotationSpeed = 30f; // Velocidad de rotación en grados por segundo

    void Update()
    {
        // Rotar el objeto en el eje Z
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
