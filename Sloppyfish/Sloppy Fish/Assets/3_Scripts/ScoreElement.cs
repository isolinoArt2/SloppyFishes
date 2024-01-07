using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreElement : MonoBehaviour
{
    public TMP_Text indexText; // Campo para mostrar el índice
    public TMP_Text userNameText;
    public TMP_Text userHighScoreText;

    public void NewScoreElement(int _index, string _userName, int _highScore)
    {
        indexText.text = _index.ToString(); // Configura el índice
        userNameText.text = _userName;
        userHighScoreText.text = _highScore.ToString();
    }
}
