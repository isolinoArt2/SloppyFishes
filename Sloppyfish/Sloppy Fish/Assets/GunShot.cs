using System.Collections;
using UnityEngine;

public class GunShot : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float shotInterval = 1.0f;
    public Transform spawnPosition;

    private void Start()
    {
        StartCoroutine(ShootRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        while (true)
        {
            Instantiate(projectilePrefab, spawnPosition.position, Quaternion.identity);
            yield return new WaitForSeconds(shotInterval);
        }
    }
}
