using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Player player;
    public GameObject _player;
    public Text scoreText;
    public GameObject _scoreTextObj;
    public GameObject playButton;
    public GameObject gameOver;
    public GameObject gameOverNoContinue;
    public GameObject leaderButton;
    public GameObject title;
    public GameObject startui;
    private int score = 0;
    public Spawner _spawner;
    public GameObject soundMute;
    public GameObject spawnerObjt;

    public GameObject datamanager;

    public GameObject animatedBtn;
    public AnimationClip animatedBtn_clip;

    public TextMeshProUGUI timerText;
    private float timer = 3.0f;
    private bool isPaused = false;

    private int highscore;

    private bool animPauseMenu = false;

    public bool continueUsed =false;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        gameOver.SetActive(false);
        startui.SetActive(true);
        title.SetActive(true);
        Pause();
        _player.SetActive(false);
        Time.timeScale = 1f;
        spawnerObjt.SetActive(false);
    }
    [System.Obsolete]
    private void Start()
    {
        FindObjectOfType<AudioManager>().Play("music");
        FindObjectOfType<AudioManager>().Play("title");
        highscore = PlayerPrefs.GetInt("highScore");
    }

    [System.Obsolete]
    public void Play()
    {
        spawnerObjt.SetActive(true);
        score = 0;
        scoreText.text = score.ToString();

       // playButton.SetActive(false);
      //  leaderButton.SetActive(false);
      //  gameOver.SetActive(false);
     //   title.SetActive(false);
       // soundMute.SetActive(false);

        Time.timeScale = 1f;
        player.enabled = true;

       
        Mines[] Mines = FindObjectsOfType<Mines>();

        for (int i = 0; i < Mines.Length; i++)
        {
            Destroy(Mines[i].gameObject);
        }
        
    }
    private void Update()
    {
        if (isPaused)
        {
            //Time.timeScale = 1f;
            timer -= Time.unscaledDeltaTime; // Utiliza Time.unscaledDeltaTime para el temporizador



            if (timer <= 0)
            {
                timerText.gameObject.SetActive(false);
                isPaused = false;
                //  Resume(); // Llama a la función Resume después de 3 segundos
              //  player.enabled = true;
                StartCoroutine(ResumeWithDelay()); // Llama a la corrutina ResumeWithDelay


            }

            // Actualiza el texto del temporizador en reversa
            timerText.text = Mathf.Ceil(timer).ToString();
        }
    }
    private IEnumerator ResumeWithDelay()
    {
        yield return new WaitForSecondsRealtime(0.2f); // Espera 3 segundos sin escalar el tiempo

        // Lógica adicional que quieras realizar cuando se reanuda después de 3 segundos
        timerText.gameObject.SetActive(false);
        //Time.timeScale = 1f;
     

        Time.timeScale = 1f; // Restaura el tiempo normal
    }

  //  [System.Obsolete]
    public void RestartHome()
    {
        Mines[] Mines = FindObjectsOfType<Mines>();

        for (int i = 0; i < Mines.Length; i++)
        {
            Destroy(Mines[i].gameObject);
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
      player.enabled = false;
        _scoreTextObj.SetActive(false);
       
    }

    public void ContinuePlaying()
    {
        continueUsed = true;
        isPaused = true;
        timer = 3.0f;
        timerText.gameObject.SetActive(true);
        player.enabled = true;
        _scoreTextObj.SetActive(true);
        
    }

    public void Resume()
    {
        timerText.gameObject.SetActive(false);
        Time.timeScale = 1f;
       player.enabled = true;
        _scoreTextObj.SetActive(true);
    }

    [System.Obsolete]
    public void GameOver()
    {
        Mines[] Mines = FindObjectsOfType<Mines>();

        for (int i = 0; i < Mines.Length; i++)
        {
            Destroy(Mines[i].gameObject);
        }
        if (PlayerPrefs.GetInt("score") > highscore)
        {
            PlayerPrefs.SetInt("highScore", PlayerPrefs.GetInt("score"));
            highscore = PlayerPrefs.GetInt("highScore");

        }
        datamanager.SendMessage("SaveData");

        if (!continueUsed)
        {
            gameOver.SetActive(true);
            animPauseMenu = true;
            StartCoroutine(PlayAnimation());
            Pause();
           // continueUsed = true;
        }
        else
        {
            gameOverNoContinue.SetActive(true);
            Pause();
            continueUsed = false;
        }
        
       
      //  playButton.SetActive(true);
      //  leaderButton.SetActive(true);
        _spawner.GetComponent<Spawner>().ResetTimer();
        FindObjectOfType<AudioManager>().Play("gameover");
        //soundMute.SetActive(true);
       
      

    }
    private IEnumerator PlayAnimation()
    {
        if (animPauseMenu)

        {
            if (animatedBtn == null)
            {
                Debug.LogError("animatedBtn is null.");
                yield break; // Salir de la rutina si animatedBtn es nulo.
            }

            if (animatedBtn_clip == null)
            {
                Debug.LogError("animatedBtn_clip is null.");
                yield break; // Salir de la rutina si animatedBtn_clip es nulo.
            }

            Animation animation = animatedBtn.GetComponent<Animation>();
            animation.clip = animatedBtn_clip;

            // Guarda la velocidad actual de la animación
            float originalSpeed = animation[animatedBtn_clip.name].speed;

            // Configura la velocidad de reproducción de la animación para que funcione en tiempo pausado
            animation[animatedBtn_clip.name].speed = 1f;

            animation.Play();
            float startTime = Time.realtimeSinceStartup;

            while (animation.isPlaying)
            {
                float currentTime = Time.realtimeSinceStartup - startTime;
                float normalizedTime = currentTime / animatedBtn_clip.length;
                animation[animatedBtn_clip.name].normalizedTime = normalizedTime;
                yield return new WaitForEndOfFrame();
            }

            // Restaura la velocidad original de la animación
            animation[animatedBtn_clip.name].speed = originalSpeed;

            animPauseMenu = false;
        }
    }


    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
        PlayerPrefs.SetInt("score", score);
       
    }
}
