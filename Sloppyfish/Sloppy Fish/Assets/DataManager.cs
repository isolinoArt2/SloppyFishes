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
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class DataManager : MonoBehaviour
{
   // public Text gpgsStatustext;
   // public Text firebaseStatusText;
    //public Text _userId;
    private string _userId;
    private int _yourhighscore;

    public GameObject LoadingScreen;
    public GameObject Gamemanager;
    // public GameObject errorsText;
    string authCode;

    // public int sceneIndex;

    // public UnityEngine.UIElements.Slider slider;
    public UnityEngine.UI.Slider connectionSlider; // Asigna el objeto Slider en el Inspector de Unity

    //   public TextMeshProUGUI progressText;

    //  public Text leaderboardText; // El Text donde mostrarás la tabla de clasificación
    public Transform scoreElementsContainer;
    public ScoreElement scoreElementPrefab; // Asigna tu prefab de ScoreElement en el Inspector de Unity

    // data base
    public GameObject _connectWithNoUser;

    string userID;

    public TextMeshProUGUI yourPositionText;
    public TextMeshProUGUI yourScoreText;
    public TextMeshProUGUI yourNameText;

    public Text scoreText;
    //public Text yourUserScore;

    private int _userLoadedScore;
    private string _userLoadedName;

    private int _yourIndex;
    private int highscore;
    private int _currentScore;
    // public InputField nameInpt, coinsInpt, inv1Inpt, inv2Inpt, inv3Inpt;

    bool isConnected;
    //public Text SaveLog, LoadLog;

     private Text nameLbl, Highscorelbl;
    private void Awake()
    {
        LoadingScreen.SetActive(true);
       // LoadData();
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        isConnected = false;
        PlayGamesPlatform.Activate();
        GPGSLogin();
        //LoadData();

        // Restablece el valor del Slider al inicio de la conexión
        connectionSlider.value = 0f;
    }

    public void LoadData()
    {
       // yourUserScore.text = " entre a load data";
        if (isConnected)
        {
            //yourUserScore.text = " entro a isConnected";
            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
            DocumentReference DocRef = db.Collection("FishPlayerData").Document(userID);
            DocRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
               // yourUserScore.text = " entro a docRef" + userID;
                DocumentSnapshot snapshot = task.Result;
               // yourUserScore.text = " task resultado es " + task.Result.ToString();
                if (snapshot.Exists)
                {
                    //yourUserScore.text = "entro y cargo datos";
                   // nameLbl.text = snapshot.GetValue<string>("playerName");
                  //  yourUserScore.text = "entro y cargo datos y obtengo playername";
                    int playerHighScore;
                    if (snapshot.TryGetValue("playerHighScore", out playerHighScore))
                    {
                        _userLoadedScore = playerHighScore;
                        PlayerPrefs.SetInt("highScore", _userLoadedScore);
                      //  yourUserScore.text = "entro y cargo datos" + playerHighScore.ToString();
                        //nameLbl.text = snapshot.GetValue<string>("playerName");
                        // Ahora puedes utilizar playerHighScore
                    }
                    else
                    {
                        //yourUserScore.text = "Error: No se pudo obtener playerHighScore";
                    }

                    string playerName;
                    if (snapshot.TryGetValue("playerName", out playerName))
                    {

                        _userLoadedName = playerName;
                       // nameLbl.text = snapshot.GetValue<string>("playerName");
                       // yourUserScore.text = "entro y cargo datos" + playerName;
                        // Ahora puedes utilizar playerHighScore
                    }
                    else
                    {
                        //yourUserScore.text = "Error: No se pudo obtener playername";
                    }

                }
                else
                {
                   // yourUserScore.text = "load error: no previous data";
                }
            });
        }
        else
        {
          //   yourUserScore.text = "load error: Firebase not connected";
        }

       // SaveData();
    }


    public void SaveData()
    {
        //PlayerPrefs.SetInt("highScore", _yourhighscore);
        //Debug.Log("save data entra");
        if (isConnected)
        {
            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

           // string playerName = _userId.text;
            string playerName = _userId;
            //  int playerScore = PlayerPrefs.GetInt("highScore");
            int playerScore = highscore;

            int playerCurrentScore = _currentScore;

            Dictionary<string, object> saveValues = new Dictionary<string, object>
            {
                            {"playerCurrentScore", playerCurrentScore},
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
                        //Debug.Log("log in sin usuario");
                        //error fixing firebase dependencies
                        //  firebaseStatusText.text = "Dependency error";
                    }
                });

            }
            else 
            {
                //Debug.Log("log in sin usuario");
                _connectWithNoUser.SetActive(true);
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
                 //   yourUserId.text = userID;
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
        LoadData();
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

        
        // SaveData();
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

            // Consulta los datos de todos los jugadores y ordénalos por highscore en orden descendente
            Query query = playersRef.OrderByDescending("playerHighScore");

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

                    foreach (DocumentSnapshot doc in snapshot.Documents)
                    {
                        string playerName = doc.GetValue<string>("playerName");
                        int playerScore = doc.GetValue<int>("playerHighScore");

                        // Comprueba si el jugador es el usuario actual
                        if (playerName == _userId)
                        {
                            _yourIndex = index;
                        }

                        // Crea una instancia de ScoreElement y configura los datos
                        ScoreElement scoreElement = Instantiate(scoreElementPrefab, scoreElementsContainer);

                        // Agrega el índice, nombre del jugador y puntuación al elemento de puntuación
                        scoreElement.NewScoreElement(index, playerName, playerScore);

                        // Incrementa el índice
                        index++;

                        // Agrega el ScoreElement a la lista
                        scoreElements.Add(scoreElement);

                        yourPositionText.text = _yourIndex.ToString();
                        //  yourScoreText.text = PlayerPrefs.GetInt("highScore").ToString();
                        yourScoreText.text = _userLoadedScore.ToString();
                        yourNameText.text = _userLoadedName;
                    }

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

    public void ShowYourPosition()
    {
        if (isConnected)
        {
            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
            CollectionReference playersRef = db.Collection("FishPlayerData");

            // Consulta los datos de todos los jugadores y ordénalos por highscore en orden descendente
            Query query = playersRef.OrderByDescending("playerHighScore");

            query.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    QuerySnapshot snapshot = task.Result;

                    // Busca tu userId en la lista
                    string yourUserId = _userId; // Reemplaza con tu userId real

                    int myPosition = -1;

                    foreach (DocumentSnapshot doc in snapshot.Documents)
                    {
                        myPosition++;
                        string playerId = doc.Id;

                        if (playerId == yourUserId)
                        {
                            // Has encontrado tu userId en la lista, guarda la posición
                            break;
                        }
                    }

                    // Incrementa en 1 la posición para mostrarla como índice de lista (empezando desde 1)
                    myPosition++;

                    // Ahora puedes mostrar tu posición en la UI
                    yourPositionText.text = myPosition.ToString();
                    //  yourScoreText.text = PlayerPrefs.GetInt("highScore").ToString();
                    yourScoreText.text = _userLoadedScore.ToString();
                    yourNameText.text = _userLoadedName;
                }
            });
        }
        else
        {
            // Firebase no está conectado
        }
    }
    public void IncreaseScore(int amount)
    {
        _currentScore += amount;
        scoreText.text = _currentScore.ToString();
        IncreaseScoreInFirebase(); // Llama a la nueva función para actualizar Firebase
    }

    public void IncreaseScoreInFirebase()
    {
        if (isConnected)
        {
            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
            DocumentReference playerDocRef = db.Collection("FishPlayerData").Document(userID);

            // Obtén el puntaje del jugador desde Firebase
            playerDocRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    DocumentSnapshot snapshot = task.Result;

                    if (snapshot.Exists)
                    {
                        // string playerName = _userId.text;
                        string playerName = _userId;
                        //  int playerScore = PlayerPrefs.GetInt("highScore");
                        int playerScore = _userLoadedScore;

                        int playerCurrentScore = _currentScore;
                        playerDocRef.SetAsync(new Dictionary<string, object>
                        {
                            {"playerCurrentScore", playerCurrentScore},
                            {"playerName",playerName },
                            {"playerHighScore",playerScore },

                        });
                        //  int currentScore = snapshot.GetValue<int>("playerHighScore");

                        // Incrementa el puntaje
                      //  currentScore += amount;

                        if (_currentScore > _userLoadedScore)
                        {
                            // Solo si el puntaje actual es mayor que el puntaje máximo, actualiza Firebase
                            playerDocRef.SetAsync(new Dictionary<string, object>
                        {
                            {"playerHighScore", _currentScore},
                            {"playerCurrentScore", playerCurrentScore},
                            {"playerName",playerName },


                        }); ;

                            // Actualiza el puntaje local solo si es mayor
                            highscore = _currentScore;
                        }

                       
                    }
                }
            });
        }
    }

    public void Play()
    {
        
        _currentScore = 0;
        scoreText.text = _currentScore.ToString();
        

    }


    public void ConnectWithNoUser()
    {
        StartCoroutine(LoadScene());
    }

}
