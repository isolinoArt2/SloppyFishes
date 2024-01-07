using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionMenuManager : MonoBehaviour
{
    public LevelObject[] levelObjects;

    public static int currentLevel;
    public static int unlockedLevels;

    public Sprite goldenStarSprite;

    public void OnClickLevel(int levelNum)
    {
        currentLevel = levelNum;
        SceneManager.LoadScene("GameScene");
    }
    // Start is called before the first frame update
    void Start()
    {
        unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 0);
        for (int i = 0; i < levelObjects.Length; i++)
        {
            if(unlockedLevels >= i)
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

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs reiniciados.");
    }


}
