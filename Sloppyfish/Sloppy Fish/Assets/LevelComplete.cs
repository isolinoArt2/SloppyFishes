using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{
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
       
        SceneManager.LoadScene("MenuScene");
    }

}
