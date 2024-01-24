using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStart : MonoBehaviour
{
    private GameManager gM;
    public Player player;
  //  public GameObject startUI;
  //  public GameObject score;
    private initializeUnityAds adsM;
    private DataManager dataM;

    public int amountOfCoins = 0;



    void Start()
    {

        Player playerComponent = player.GetComponent<Player>();

        // Obtener la instancia de DataManager
        dataM = DataManager.instance;
        adsM = initializeUnityAds.instance;
        gM = GameManager.instance;
        if (playerComponent != null)
            playerComponent.GetCoinsOnLevel(amountOfCoins);

    }

   
}
