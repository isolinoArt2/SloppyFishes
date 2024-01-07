using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    public int starsEarned = 0; // Número de estrellas que quieres simular ganar


    public void TheNextLevel()
    {
        int currentLevel = PlayerPrefs.GetInt("ReachedLevel", 1);

        // Obtener las estrellas ganadas para el nivel actual
        int starsForCurrentLevel = PlayerPrefs.GetInt("StarsEarned_" + currentLevel, 0);

        // Incrementar ReachedLevel solo si hay al menos 1 estrella
        if (starsForCurrentLevel >= 1)
        {
            PlayerPrefs.SetInt("ReachedLevel", currentLevel + 1);
        }

        // Simular la obtención de estrellas para el nivel actual
        PlayerPrefs.SetInt("StarsEarned_" + currentLevel, starsEarned);

        // Cargar la escena "level"
        Application.LoadLevel("level");
    }
}
