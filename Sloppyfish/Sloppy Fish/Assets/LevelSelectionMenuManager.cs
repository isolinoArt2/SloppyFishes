using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectionMenuManager : MonoBehaviour
{
    public LevelObject[] levelObjects;

    public static int currentLevel;
    public static int unlockedLevels;

    public Sprite goldenStarSprite;
    public Slider loadingSlider;
    public GameObject loadingScreen;
    public GameObject levelMenu;


    // Start is called before the first frame update
    void Start()
    {
        loadingScreen.SetActive(false);
        UnlockTheLevels();
    }
    public void UnlockTheLevels()
    {
        unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 0);
        for (int i = 0; i < levelObjects.Length; i++)
        {
            if (unlockedLevels >= i)
            {
                levelObjects[i].levelButton.interactable = true;
                int stars = PlayerPrefs.GetInt("Stars" + i.ToString(), 0);
                for (int j = 0; j < stars; j++)
                {
                    levelObjects[i].stars[j].sprite = goldenStarSprite;
                }
            }
        }

    }
    public void OnClickLevel(int levelNum)
    {
        currentLevel = levelNum;
        // SceneManager.LoadScene("GameScene");
        StartCoroutine(LoadLevelAsync());
    }

    IEnumerator LoadLevelAsync()
    {
        // Muestra el slider antes de cargar la escena
        loadingScreen.SetActive(true);

        // Comienza la carga asincrónica de la escena
        AsyncOperation operation = SceneManager.LoadSceneAsync("GameScene");

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
        levelMenu.SetActive(false);


        // Desactiva el loadingScreen después de cargar la escena
        loadingScreen.SetActive(false);

      
    }


    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs reiniciados.");
    }


}
