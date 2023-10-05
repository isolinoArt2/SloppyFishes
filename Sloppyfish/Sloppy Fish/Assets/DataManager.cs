using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System.Threading;
using UnityEngine.UI;
using Firebase.Firestore;
using Firebase.Extensions;
using System;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using TMPro;


public class DataManager : MonoBehaviour
{
    public Text gpgsStatustext;
    public Text firebaseStatusText;
    public Text _userId;
    // public GameObject errorsText;
    string authCode;

    public int sceneIndex;

    public UnityEngine.UIElements.Slider slider;
 //   public TextMeshProUGUI progressText;

    // data base

    string userID;

    // public InputField nameInpt, coinsInpt, inv1Inpt, inv2Inpt, inv3Inpt;

    bool isConnected;
    //public Text SaveLog, LoadLog;

    // public Text nameLbl, coinlbl, inv1Lbl, inv2Lbl, inv3Lbl;

    public void LoadData()
    {
        if (isConnected)
        {
            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
            DocumentReference DocRef = db.Collection("DokkoPlayerData").Document(userID);
            DocRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {/*
                    nameLbl.text = snapshot.GetValue<string>("playerName");
                    coinlbl.text = snapshot.GetValue<int>("playerCoins").ToString();

                    List<string> invList = snapshot.GetValue<List < string >> ("playerInventory");
                    inv1Lbl.text = invList[0];
                    inv2Lbl.text = invList[1];
                    inv3Lbl.text = invList[2];

                    */
                }
                else
                {
                    // LoadLog.text = "load error: no previous data";
                }
            });
        }
        else
        {
            // LoadLog.text = "load error: Firebase not connected";
        }
    }


    public void SaveData()
    {
        if (isConnected)
        {
            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
           
            Dictionary<string, object> saveValues = new Dictionary<string, object>
            {
                
            };
            //esta collection guarda todos los documentos de datos de nuestros usuarios
            DocumentReference docRef = db.Collection("DokkoPlayerData").Document(userID);
            //ahora guardamos los valores del diccionario en la coleccion
            docRef.SetAsync(saveValues).ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    // SaveLog.text = "Save Completed";
                }
                else
                {
                    //   SaveLog.text = "error saving data: check connection";
                }
            });

        }
        else
        {
            //  SaveLog.text = "Save error: Firebase not connected";
        }

    }


    // data base


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        isConnected = false;
        PlayGamesPlatform.Activate();
        GPGSLogin();
    }



    public void GPGSLogin()
    {
        PlayGamesPlatform.Instance.Authenticate((success) =>
        {
            if (success == SignInStatus.Success)
            {
                //loged in to gpgs
                gpgsStatustext.text = "logged in";
                Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.Result == Firebase.DependencyStatus.Available)
                    {
                        // no dependency issu with firebase
                        ConnectToFirebase();
                    }
                    else
                    {
                        //error fixing firebase dependencies
                        firebaseStatusText.text = "Dependency error";
                    }
                });

            }
        });
    }

    void ConnectToFirebase()
    {
        firebaseStatusText.text = "try to connect";
        PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
        {
            authCode = code;
            firebaseStatusText.text = "authcode " + authCode;

            Firebase.Auth.FirebaseAuth FBauth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            Firebase.Auth.Credential FBcred = Firebase.Auth.PlayGamesAuthProvider.GetCredential(authCode);
            FBauth.SignInWithCredentialAsync(FBcred).ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    firebaseStatusText.text = "sign in canceled";

                }
                if (task.IsFaulted)
                {
                    firebaseStatusText.text = "error :: " + task.Result;
                }

                Firebase.Auth.FirebaseUser user = FBauth.CurrentUser;
                if (user != null)
                {
                    userID = user.UserId;
                    firebaseStatusText.text = "signed in as " + user.DisplayName;
                    _userId.text = user.DisplayName;
                    isConnected = true;
                   StartCoroutine(LoadScene());
                }
                else
                {
                    //error getting user
                    // errorsText.SetActive(true);
                }
            });
        });
    }

    
    IEnumerator LoadScene()
    {

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);


        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;
            //progressText.text = progress * 100f + "%";

            yield return null;
        }
    }
    
}
