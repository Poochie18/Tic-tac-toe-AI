using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Globalization;

public class UIManager : MonoBehaviour
{
    public event Action RestartGameEvent;

    [SerializeField] private Text XO;
    [SerializeField] private Text whoseTurn;
    [SerializeField] private Button restartButton;

    private Dictionary<int, string> playersIcons = new Dictionary<int, string> { { 1, "X" }, { 2, "O" } };

    public void ChangePlayerIcon(int playerIndex)
    {
        whoseTurn.text = "Whose turn";
        XO.text = playersIcons[playerIndex];
    }

    public void SetUpWinningText(int playerIndex)
    {
        whoseTurn.text = "Winner";
        XO.text = playersIcons[playerIndex];
    }

    public void SetUpDrawText()
    {
        whoseTurn.text = "Draw";
        XO.text = null;
    }

    public void RestartGame()
    {
        RestartGameEvent();
    }

    public void ExitTheGame()
    {
        Application.Quit();
    }
}
