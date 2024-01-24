using Cinemachine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemCollect : MonoBehaviour
{
    public GameObject itemPrefab;
    public string _tag;



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player playerScript = other.GetComponent<Player>();
            if(_tag != null)
            {
                if (_tag == "Boot")
                {
                    PlayerPrefs.SetInt("Boots", PlayerPrefs.GetInt("Boots") + 1);
                    Destroy(gameObject);
                    
                }

                else if (_tag == "OtraCosa")
                {
                    Debug.LogWarning("Player get otra cosa.");
                    Destroy(gameObject);
                }
            }
            else if (playerScript != null)
            {
                //  StartCoroutine(TriggerSlowMotion());
                playerScript.GetItem(itemPrefab);
                FindObjectOfType<AudioManager>().Play("coins");
                Destroy(gameObject);
            }


        }
       

    }


   

}