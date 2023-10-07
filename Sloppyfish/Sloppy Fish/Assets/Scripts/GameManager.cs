using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Player player;
    public GameObject _player;
    public Text scoreText;
    public GameObject playButton;
    public GameObject gameOver;
    public GameObject leaderButton;
    public GameObject title;
    public GameObject startui;
    private int score = 0;
    public Spawner _spawner;
    public GameObject soundMute;
    public GameObject spawnerObjt;

    public GameObject datamanager;

    private int highscore;
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

        playButton.SetActive(false);
        leaderButton.SetActive(false);
        gameOver.SetActive(false);
        title.SetActive(false);
        soundMute.SetActive(false);

        Time.timeScale = 1f;
        player.enabled = true;

       
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
    }

    [System.Obsolete]
    public void GameOver()
    {
        if (PlayerPrefs.GetInt("score") > highscore)
        {
            PlayerPrefs.SetInt("highScore", PlayerPrefs.GetInt("score"));
            highscore = PlayerPrefs.GetInt("highScore");

        }
        datamanager.SendMessage("SaveData");
        gameOver.SetActive(true);
        playButton.SetActive(true);
        leaderButton.SetActive(true);
        _spawner.GetComponent<Spawner>().ResetTimer();
        FindObjectOfType<AudioManager>().Play("gameover");
        soundMute.SetActive(true);
        Pause();
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
        PlayerPrefs.SetInt("score", score);
       
    }
}
