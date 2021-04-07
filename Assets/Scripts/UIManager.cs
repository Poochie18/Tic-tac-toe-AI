using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Globalization;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public event Action RestartGameEvent;
    public event Action ResetDataBase;
    public event Action<int> RestartAndSwitch;
    public event Action<bool> SetUpAIBattle;

    [SerializeField] private GameObject XO;
    [SerializeField] private GameObject whoseTurn;

    [SerializeField] private Text[] winsCount;

    [SerializeField] private Button restartButton;
    [SerializeField] private Button settingsButton;

    [SerializeField] private Sprite[] textSprites;
    [SerializeField] private Sprite[] XOSprites;

    [SerializeField] private GameObject bottomPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject switchBtn;
    [SerializeField] private GameObject switchBtnForPanel;
    [SerializeField] private GameObject switchBtnAIvsAI;

    private bool setMenu = true;
    private bool setAIBattle = false;

    private bool switchState;
    private bool switchStateForPanel;
    public void OnSwitchButtonClicked()
    {
        switchBtn.transform.DOLocalMoveX(-switchBtn.transform.localPosition.x, 0.2f);
        switchState = !switchState;
        if (switchState)
            RestartAndSwitch(1);
        else
            RestartAndSwitch(2);
        RestartGame();
    }

    public void OnSwitchButtonClickedForBottomPanel()
    {
        switchBtnForPanel.transform.DOLocalMoveX(-switchBtnForPanel.transform.localPosition.x, 0.2f);
        
        bottomPanel.SetActive(switchStateForPanel);
        switchStateForPanel = !switchStateForPanel;
    }

    public void OnSwitchAIvsAI()
    {
        switchBtnAIvsAI.transform.DOLocalMoveX(-switchBtnAIvsAI.transform.localPosition.x, 0.2f);

        SetUpAIBattle(setAIBattle);
        setAIBattle = !setAIBattle;
        SetPausePanel();
        RestartGame();
    }


    public void ChangePlayerIcon(int playerIndex)
    {
        whoseTurn.GetComponent<Image>().sprite = textSprites[0];
        XO.GetComponent<Image>().sprite = XOSprites[playerIndex-1];
    }

    public void SetUpWinningText(int playerIndex)
    {
        whoseTurn.GetComponent<Image>().sprite = textSprites[1];
        XO.GetComponent<Image>().sprite = XOSprites[playerIndex-1];
    }

    public void SetUpDrawText()
    {
        whoseTurn.GetComponent<Image>().sprite = textSprites[2];
        XO.GetComponent<Image>().sprite = null;
    }

    public void RestartGame()
    {
        RestartGameEvent();
    }

    public void SetIterationsCount(int Xwins, int Owins)
    {
        winsCount[0].text = Xwins.ToString();
        winsCount[1].text = Xwins.ToString();
        winsCount[2].text = Owins.ToString();
        winsCount[3].text = Owins.ToString();
    }

    public void ExitTheGame()
    {
        Application.Quit();
    }

    public void SetPausePanel()
    {
        setMenu = !setMenu;
        pausePanel.SetActive(setMenu);
    }

}

