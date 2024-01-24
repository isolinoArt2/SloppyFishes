using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelComplete : MonoBehaviour
{
    public GameObject levelManager;
    public GameObject levelMenuPanel;
    public GameObject loadingScreen;
    public Slider loadingSlider;

    public void OnLevelComplete(int starsAquired)
    {
        if (LevelSelectionMenuManager.currentLevel == LevelSelectionMenuManager.unlockedLevels)
        {
            LevelSelectionMenuManager.unlockedLevels++;
            PlayerPrefs.SetInt("UnlockedLevels", LevelSelectionMenuManager.unlockedLevels);
        }
        if(starsAquired > PlayerPrefs.GetInt("Stars" + LevelSelectionMenuManager.currentLevel.ToString(), 0))
        {
            PlayerPrefs.SetInt("Stars" + LevelSelectionMenuManager.currentLevel.ToString(), starsAquired);
        }
        
       // SceneManager.LoadScene("MenuScene");
        StartCoroutine(LoadLevelAsync());
        PlayerPrefs.SetInt("Boots", 0);
        PlayerPrefs.SetInt("CoinsGetOnLevel", 0);



    }
    IEnumerator LoadLevelAsync()
    {
        // Muestra el slider antes de cargar la escena
        loadingScreen.SetActive(true);

        // Comienza la carga asincrónica de la escena
        AsyncOperation operation = SceneManager.LoadSceneAsync("MenuScene");

        // Desactiva la carga automática de la escena hasta que estés listo
        operation.allowSceneActivation = false;

        // Actualiza el slider según el progreso de carga
        while (operation.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingSlider.value = progress;
            yield return null;
        }

        // Ahora, cuando estés listo, activa la escena
        operation.allowSceneActivation = true;
        // Espera un breve momento antes de desactivar el loadingScreen
        yield return new WaitForSeconds(0.5f);
        // Desactiva el levelMenu después de cargar la escena
        levelMenuPanel.SetActive(true);
        
        levelManager.SendMessage("UnlockTheLevels");


        // Desactiva el loadingScreen después de cargar la escena
        loadingScreen.SetActive(false);


    }
}
