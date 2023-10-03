using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Text scoreText;
    public GameObject playButton;
    public GameObject gameOver;
    public GameObject title;
    public GameObject startui;
    private int score = 0;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        gameOver.SetActive(false);
        startui.SetActive(true);
        title.SetActive(true);
        Pause();
    }

    public void Play()
    {
        score = 0;
        scoreText.text = score.ToString();

        playButton.SetActive(false);
        gameOver.SetActive(false);
        title.SetActive(false);


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

    public void GameOver()
    {
        gameOver.SetActive(true);
        playButton.SetActive(true);

        Pause();
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
    }
}
