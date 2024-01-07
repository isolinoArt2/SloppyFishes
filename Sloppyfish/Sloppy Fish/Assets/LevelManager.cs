using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private LevelData[] levelData; // Asigna estos datos en el Inspector

    Button[] levelButtons;

    private void Awake()
    {
        int reachedLevel = PlayerPrefs.GetInt("ReachedLevel", 1);

        levelButtons = new Button[transform.childCount];
        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i] = transform.GetChild(i).GetComponent<Button>();
            levelButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = (i + 1).ToString();

            // Obtener las estrellas ganadas para el nivel actual
            int starsForCurrentLevel = PlayerPrefs.GetInt("StarsEarned_" + (i + 1), 0);

            // Verificar si el nivel actual está desbloqueado por estrellas ganadas
            if (i + 1 <= reachedLevel || (i + 1 == reachedLevel && starsForCurrentLevel >= 1))
            {
                levelButtons[i].interactable = true;
            }
            else
            {
                levelButtons[i].interactable = false;
            }
        }
    }
    public void LoadScene(int Level)
    {
        PlayerPrefs.SetInt("Level", Level);
        Debug.Log("PlayerPrefs levelreach load schene  " + PlayerPrefs.GetInt("ReachedLevel"));
        if (PlayerPrefs.GetInt("ReachedLevel") <= 1)
            PlayerPrefs.SetInt("ReachedLevel", 1);
        Application.LoadLevel("Next");

    }

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs reiniciados.");
    }

}
