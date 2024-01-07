using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> prefabs; // Lista de prefabs inicial
    public List<GameObject> prefabs2; // Lista de prefabs inicial
    public List<GameObject> prefabs3; // Lista de prefabs inicial
    public List<Transform> spawnpoints; // Lista de puntos de spawn
    public float tiempoEntreInstancias = 2f; // Tiempo en segundos entre cada instancia

    public int playingLevel = 1;
    public float timerInterval = 10f; // Intervalo de tiempo en segundos

    private float playingTime = 0f; // Tiempo transcurrido desde el inicio del juego
    private int ultimoPrefabInstanciadoIndex = -1; // Índice del último prefab instanciadopr
    private int _nuevoIndice;

    private void Start()
    {
        // Comenzar a invocar la función Spawn() cada tiempoEntreInstancias segundos
       // InvokeRepeating("Spawn", 0f, tiempoEntreInstancias);
      //  StartCoroutine(UpdatePlayingLevel());
    }
    private IEnumerator UpdatePlayingLevel()
    {
        while (true) // Ejecutar indefinidamente
        {
            yield return new WaitForSeconds(timerInterval); // Esperar 10 segundos
            playingLevel++; // Aumentar el nivel
           
        }
    }
    private void Update()
    {
        // Actualizar el tiempo de juego
        playingTime += Time.deltaTime;

        // Verificar si hay al menos dos prefabs en la lista
        if (spawnpoints.Count >= 2)
        {
            // Seleccionar un nuevo índice aleatorio para el próximo prefab
            int nuevoIndice = Random.Range(0, spawnpoints.Count);

            // Verificar si hay un último prefab instanciado
            if (ultimoPrefabInstanciadoIndex != -1)
            {
                // Limitar el rango del índice aleatorio en función del último prefab instanciado
                int minIndex = Mathf.Max(0, ultimoPrefabInstanciadoIndex - 1);
                int maxIndex = Mathf.Min(spawnpoints.Count - 1, ultimoPrefabInstanciadoIndex + 1);

                // Seleccionar un nuevo índice aleatorio dentro del rango permitido
                nuevoIndice = Random.Range(minIndex, maxIndex + 1);
                _nuevoIndice = nuevoIndice;
            }



        }
        else
        {
            Debug.LogWarning("La lista de prefabs debe contener al menos dos elementos para funcionar correctamente.");
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
        if (playingLevel == 1)
        {
            // Seleccionar un prefab aleatorio de la lista actual
            int indicePrefabAleatorio = Random.Range(0, prefabs.Count);
            GameObject prefabSeleccionado = prefabs[indicePrefabAleatorio];

            // Instanciar el prefab en la posición del punto de spawn seleccionado
            int indiceSpawnAleatorio = Random.Range(0, spawnpoints.Count);
            Transform puntoSpawnSeleccionado = spawnpoints[_nuevoIndice];
            Instantiate(prefabs[indiceSpawnAleatorio], puntoSpawnSeleccionado.position, puntoSpawnSeleccionado.rotation);

        }
        if (playingLevel == 2)
        {
            // Seleccionar un prefab aleatorio de la lista actual
            int indicePrefabAleatorio = Random.Range(0, prefabs2.Count);
            GameObject prefabSeleccionado = prefabs2[indicePrefabAleatorio];

            // Instanciar el prefab en la posición del punto de spawn seleccionado
            int indiceSpawnAleatorio = Random.Range(0, spawnpoints.Count);
            Transform puntoSpawnSeleccionado = spawnpoints[_nuevoIndice];
            Instantiate(prefabs2[indiceSpawnAleatorio], puntoSpawnSeleccionado.position, puntoSpawnSeleccionado.rotation);

        }
        if (playingLevel >= 3)
        {
            // Seleccionar un prefab aleatorio de la lista actual
            int indicePrefabAleatorio = Random.Range(0, prefabs3.Count);
            GameObject prefabSeleccionado = prefabs3[indicePrefabAleatorio];

            // Instanciar el prefab en la posición del punto de spawn seleccionado
            int indiceSpawnAleatorio = Random.Range(0, spawnpoints.Count);
            Transform puntoSpawnSeleccionado = spawnpoints[_nuevoIndice];
            Instantiate(prefabs3[indiceSpawnAleatorio], puntoSpawnSeleccionado.position, puntoSpawnSeleccionado.rotation);

        }

        // Actualizar el índice del último prefab instanciado
        ultimoPrefabInstanciadoIndex = _nuevoIndice;
    }
}

/*
private void Update()
{
    // Actualizar el tiempo de juego
    playingTime += Time.deltaTime;

    // Verificar si hay al menos dos prefabs en la lista
    if (prefabs.Count >= 2)
    {
        // Seleccionar un nuevo índice aleatorio para el próximo prefab
        int nuevoIndice = Random.Range(0, prefabs.Count);

        // Verificar si hay un último prefab instanciado
        if (ultimoPrefabInstanciadoIndex != -1)
        {
            // Limitar el rango del índice aleatorio en función del último prefab instanciado
            int minIndex = Mathf.Max(0, ultimoPrefabInstanciadoIndex - 1);
            int maxIndex = Mathf.Min(prefabs.Count - 1, ultimoPrefabInstanciadoIndex + 1);

            // Seleccionar un nuevo índice aleatorio dentro del rango permitido
            nuevoIndice = Random.Range(minIndex, maxIndex + 1);
           _nuevoIndice = nuevoIndice ;
        }



    }
    else
    {
        Debug.LogWarning("La lista de prefabs debe contener al menos dos elementos para funcionar correctamente.");
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

    // Instanciar el prefab en la posición del punto de spawn seleccionado
    int indiceSpawnAleatorio = Random.Range(0, spawnpoints.Count);
    Transform puntoSpawnSeleccionado = spawnpoints[indiceSpawnAleatorio];
    Instantiate(prefabs[_nuevoIndice], puntoSpawnSeleccionado.position, puntoSpawnSeleccionado.rotation);

    // Actualizar el índice del último prefab instanciado
    ultimoPrefabInstanciadoIndex = _nuevoIndice;
}
}
*/