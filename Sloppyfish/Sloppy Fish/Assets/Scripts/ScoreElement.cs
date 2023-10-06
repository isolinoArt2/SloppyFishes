using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreElement : MonoBehaviour
{
    public TMP_Text userNameText;
    public TMP_Text userHighScoreText;

    public void NewScoreElement(string _userName, int _highScore)
    {
        userNameText.text = _userName;
        userHighScoreText.text = _highScore.ToString();

    }
}
