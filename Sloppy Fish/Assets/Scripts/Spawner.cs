using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> prefabs; // Lista de prefabs inicial
    public List<GameObject> prefabs1; // Lista de prefabs inicial
    public List<GameObject> prefabs2; // Lista de prefabs para la segunda etapa
    public List<GameObject> prefabs3; // Lista de prefabs para la tercera etapa
    public List<Transform> spawnpoints; // Lista de puntos de spawn
    public float tiempoEntreInstancias = 2f; // Tiempo en segundos entre cada instancia

    private float playingTime = 0f; // Tiempo transcurrido desde el inicio del juego

    private void Start()
    {
        // Comenzar a invocar la función Spawn() cada tiempoEntreInstancias segundos
        InvokeRepeating("Spawn", 0f, tiempoEntreInstancias);
        prefabs = prefabs1;
    }

    private void Update()
    {
        // Actualizar el tiempo de juego
        playingTime += Time.deltaTime;

        // Verificar la dificultad y cambiar las listas de prefabs
        if (playingTime > 30f && playingTime <= 60f)
        {
            // Cambiar a la lista de prefabs2
            prefabs = prefabs2;
           // Debug.Log("segundo nivel");
        }
        else if (playingTime > 60f)
        {
            // Cambiar a la lista de prefabs3
            prefabs = prefabs3;
           // Debug.Log("segundo nivel");
        }
    }
    public void ResetTimer()
    {
        playingTime = 0;
      //  Debug.Log("reset timer");
    }

    private void Spawn()
    {
        // Verificar si las listas están vacías
        if (prefabs.Count == 0 || spawnpoints.Count == 0)
        {
            Debug.LogWarning("La lista de prefabs o la lista de spawnpoints está vacía.");
            return;
        }

        // Seleccionar un prefab aleatorio de la lista actual
        int indicePrefabAleatorio = Random.Range(0, prefabs.Count);
        GameObject prefabSeleccionado = prefabs[indicePrefabAleatorio];

        // Seleccionar un punto de spawn aleatorio de la lista
        int indiceSpawnAleatorio = Random.Range(0, spawnpoints.Count);
        Transform puntoSpawnSeleccionado = spawnpoints[indiceSpawnAleatorio];

        // Instanciar el prefab en la posición del punto de spawn seleccionado
        Instantiate(prefabSeleccionado, puntoSpawnSeleccionado.position, puntoSpawnSeleccionado.rotation);
    }
}
