using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public float initialSpawnRate = 1f;
    public float minHeight = -2f;
    public float maxHeight = 2f;
    public float timeToIncreaseRate = 10.0f;  // Cada cuántos segundos aumentar la velocidad.
    public float increaseRateAmount = 0.1f;  // Cuánto aumentar la velocidad cada vez.

    private float spawnRate;
    private float spawnRateTimer;

    private void OnEnable()
    {
        spawnRate = initialSpawnRate;
        spawnRateTimer = spawnRate;
        InvokeRepeating("Spawn", 0f, spawnRate);
    }

    private void OnDisable()
    {
        CancelInvoke("Spawn");
    }

    private void Update()
    {
        // Incrementa el temporizador de velocidad de spawn.
        spawnRateTimer -= Time.deltaTime;

        // Si ha pasado el tiempo para aumentar la velocidad, aumenta la velocidad y reinicia el temporizador.
        if (spawnRateTimer <= 0)
        {
            spawnRate += increaseRateAmount;  // Aumenta la velocidad de spawn.
            spawnRateTimer = timeToIncreaseRate;  // Reinicia el temporizador.
            CancelInvoke("Spawn");  // Cancela la invocación anterior.
            InvokeRepeating("Spawn", 0f, spawnRate);  // Vuelve a invocar con la nueva velocidad.
        }
    }

    private void Spawn()
    {
        Vector3 spawnPosition = transform.position + Vector3.up * Random.Range(minHeight, maxHeight);
        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }
}
