using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelManager : MonoBehaviour
{
    public GameObject[] levels;

    // Start is called before the first frame update
    void Start()
    {
        levels[LevelSelectionMenuManager.currentLevel].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
