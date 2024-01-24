using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevel : MonoBehaviour
{
    public GameObject level1;
    public GameObject gm;
    public GameObject tutorialPanel;
    private bool tutorial = false;
    // Start is called before the first frame update
   
    void Start()
    {

        if (level1 !=null && level1.activeSelf )
        {
            tutorialPanel.SetActive(true);
        }
    }
    private void Update()
    {
        if (tutorial)
        {
            if (gm != null && gm.activeSelf)
            {
                tutorialPanel.SetActive(true);
            }
        }
       
    }


}
