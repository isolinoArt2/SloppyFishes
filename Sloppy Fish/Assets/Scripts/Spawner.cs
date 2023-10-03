using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public float spawnRate = 1f;
    public float minHeight = -2f;
    public float maxHeight = 2f;

    private void OnEnable()
    {
        InvokeRepeating("Spawn", 0f, spawnRate);
    }

    private void OnDisable()
    {
        CancelInvoke("Spawn");
    }

    private void Spawn()
    {
        Vector3 spawnPosition = transform.position + Vector3.up * Random.Range(minHeight, maxHeight);
        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }
}
