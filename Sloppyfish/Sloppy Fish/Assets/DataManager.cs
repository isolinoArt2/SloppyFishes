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
//using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class DataManager : MonoBehaviour
{
   // public Text gpgsStatustext;
   // public Text firebaseStatusText;
    //public Text _userId;
    private string _userId;

    public GameObject LoadingScreen;
    public GameObject Gamemanager;
    // public GameObject errorsText;
    string authCode;

    // public int sceneIndex;

    // public UnityEngine.UIElements.Slider slider;
    public Slider connectionSlider; // Asigna el objeto Slider en el Inspector de Unity

    //   public TextMeshProUGUI progressText;

    //  public Text leaderboardText; // El Text donde mostrarás la tabla de clasificación
    public Transform scoreElementsContainer;
    public ScoreElement scoreElementPrefab; // Asigna tu prefab de ScoreElement en el Inspector de Unity

    // data base

    string userID;

    // public InputField nameInpt, coinsInpt, inv1Inpt, inv2Inpt, inv3Inpt;

    bool isConnected;
    //public Text SaveLog, LoadLog;

     private Text nameLbl, Highscorelbl;
    private void Awake()
    {
        LoadingScreen.SetActive(true);
       // LoadData();
    }
    

    public void LoadData()
    {
        if (isConnected)
        {
            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
            DocumentReference DocRef = db.Collection("FishPlayerData").Document(userID);
            DocRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    nameLbl.text = snapshot.GetValue<string>("playerName");
                    Highscorelbl.text = snapshot.GetValue<int>("playerHighScore").ToString();

                    

                    
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
        //Debug.Log("save data entra");
        if (isConnected)
        {
            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

           // string playerName = _userId.text;
            string playerName = _userId;
            int playerScore = PlayerPrefs.GetInt("highScore");
           
            Dictionary<string, object> saveValues = new Dictionary<string, object>
            {
                {"playerName",playerName },
                {"playerHighScore",playerScore },
            };
            //esta collection guarda todos los documentos de datos de nuestros usuarios
            DocumentReference docRef = db.Collection("FishPlayerData").Document(userID);
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
        LoadData();

        // Restablece el valor del Slider al inicio de la conexión
        connectionSlider.value = 0f;
    }



    public void GPGSLogin()
    {
        PlayGamesPlatform.Instance.Authenticate((success) =>
        {
            if (success == SignInStatus.Success)
            {
                //loged in to gpgs
                //gpgsStatustext.text = "logged in";
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
                      //  firebaseStatusText.text = "Dependency error";
                    }
                });

            }
        });
    }

    void ConnectToFirebase()
    {
        
        //  firebaseStatusText.text = "try to connect";
        PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
        {
            authCode = code;
           // firebaseStatusText.text = "authcode " + authCode;

            Firebase.Auth.FirebaseAuth FBauth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            Firebase.Auth.Credential FBcred = Firebase.Auth.PlayGamesAuthProvider.GetCredential(authCode);
            FBauth.SignInWithCredentialAsync(FBcred).ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                  //  firebaseStatusText.text = "sign in canceled";

                }
                if (task.IsFaulted)
                {
                  //  firebaseStatusText.text = "error :: " + task.Result;
                }

                Firebase.Auth.FirebaseUser user = FBauth.CurrentUser;
                if (user != null)
                {
                    userID = user.UserId;
                 //   firebaseStatusText.text = "signed in as " + user.DisplayName;
                    _userId = user.DisplayName;
                    isConnected = true;

                    StartCoroutine(UpdateConnectionSlider());

                    // ffReference = FirebaseFirestore.DefaultInstance;
                   
                }
                else
                {
                    //error getting user
                    // errorsText.SetActive(true);
                }
            });
        });
    }
    private IEnumerator UpdateConnectionSlider()
    {
        float duration = 3f; // Duración total de la conexión (ajusta según tu necesidad)
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            float progress = Mathf.Clamp01((Time.time - startTime) / duration);

            // Actualiza el valor del Slider gradualmente
            connectionSlider.value = progress;
           

            yield return null;
        }

        // Asegúrate de que el Slider esté en 1 al finalizar la conexión
        connectionSlider.value = 1f;
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
       

        SaveData();
        LoadingScreen.SetActive(false);
        Gamemanager.SetActive(true);
        yield return null;
    }

    public void LoadLeaderboard()
    {
        if (isConnected)
        {
            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
            CollectionReference playersRef = db.Collection("FishPlayerData");

            // Consulta los datos de los líderes y ordénalos por puntuación en orden ascendente
            Query query = playersRef.OrderBy("playerHighScore").Limit(10);

            query.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    QuerySnapshot snapshot = task.Result;

                    // Elimina las instancias existentes de ScoreElement en el contenedor
                    foreach (Transform child in scoreElementsContainer)
                    {
                        Destroy(child.gameObject);
                    }

                    List<ScoreElement> scoreElements = new List<ScoreElement>();

                    // Variable para llevar un seguimiento del índice
                    int index = 1;

                    // Calcula la cantidad total de elementos en la consulta
                    int totalElements = 0;
                    foreach (var doc in snapshot.Documents)
                    {
                        totalElements++;
                    }

                    foreach (DocumentSnapshot doc in snapshot.Documents)
                    {
                        string playerName = doc.GetValue<string>("playerName");
                        int playerScore = doc.GetValue<int>("playerHighScore");

                        // Crea una instancia de ScoreElement y configura los datos
                        ScoreElement scoreElement = Instantiate(scoreElementPrefab, scoreElementsContainer);

                        // Agrega el índice invertido, nombre del jugador y puntuación al elemento de puntuación
                        scoreElement.NewScoreElement(totalElements - index + 1, playerName, playerScore);

                        // Incrementa el índice
                        index++;

                        // Agrega el ScoreElement a la lista
                        scoreElements.Add(scoreElement);
                    }

                    // No es necesario invertir la lista ya que los índices se generan en orden invertido
                    scoreElements.Reverse();
                    // Reorganiza las instancias en el contenedor
                    foreach (var scoreElement in scoreElements)
                    {
                        scoreElement.transform.SetAsLastSibling();
                    }
                }
            });
        }
        else
        {
            // Firebase no está conectado
        }
    }

}
