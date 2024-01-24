using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
 
    public void PauseTheGame()
    {
       
       // StartCoroutine(PlayAnimation());
        Time.timeScale = 0f;
       
    }

    public void Resume()
    {
       
        //StartCoroutine(_Resume());


         Time.timeScale = 1f;
    }
   
}
