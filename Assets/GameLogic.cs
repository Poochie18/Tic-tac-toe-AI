using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Globalization;

public class GameLogic : MonoBehaviour
{
    public static event Action<int[]> AITurn;
    public static event Action<int> CalculateNewWeight;

    //public static Dictionary<string, float> allStatements = new Dictionary<string, float>();

    [SerializeField] private int whichPlayer; //1 - X, 2 = 0
    
    [SerializeField] private Button[] titles;
    [SerializeField] private bool gamePause = false;
    [SerializeField] private int[] markedTiles = new int[9];

    private Dictionary<int, string> playersIcons = new Dictionary<int, string> { { 1, "X" }, { 2, "O" } };

    public static int turnCount;

    [SerializeField] private AIManager aiManager;
    [SerializeField] private UIManager uiManager;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("FIRSTTIMEOPENING", 1) == 1)
        {
            Debug.Log("First Time Opening");

            PlayerPrefs.SetInt("FIRSTTIMEOPENING", 0);

            string path = Application.persistentDataPath + "/text2.txt";
            TextAsset txtAsset = Resources.Load("text2") as TextAsset;
            string txt = txtAsset.text;
            System.IO.File.WriteAllText(path, txt);

        }
        else
        {
            Debug.Log("NOT First Time Opening");
        }
    }


    private void Start()
    {
        uiManager.RestartGameEvent += GameSetup;
        aiManager.ChangeTileIndex += ChangeStates;
        GameSetup();
    }


    void GameSetup()
    {
        gamePause = false;
        whichPlayer = 1;
        turnCount = 0;

        aiManager.ClearAITemp();
        uiManager.ChangePlayerIcon(whichPlayer);

        for (int i = 0; i < markedTiles.Length; i++)
        {
            markedTiles[i] = 0;
            titles[i].interactable = true;
            titles[i].GetComponentInChildren<Text>().text = null;
        }
    }


    public void PressTheTile(int tileIndex)
    {
        ChangeStates(tileIndex);
        CheckStatements();

        if (!gamePause)
        {
            AITurn(markedTiles);
            CheckStatements();
        }
        
    }

    void CheckStatements()
    {
        turnCount++;
        uiManager.ChangePlayerIcon(whichPlayer);
        if (turnCount > 4)
            CheckingWinner();
        whichPlayer = whichPlayer == 1 ? 2 : 1;
    }


    void ChangeStates(int tileIndex)
    {
        markedTiles[tileIndex] = whichPlayer;
        Button tile = titles[tileIndex];
        tile.GetComponentInChildren<Text>().text = playersIcons[whichPlayer];
        //Text[] txts = tile.GetComponentsInChildren<Text>();
        //txts[0].text = playersIcons[whichPlayer];
        //txts[1].text = null;
        tile.interactable = false;
        
    }


    void CheckingWinner()
    {

        List<bool> winChecksList = new List<bool>()
        {
            markedTiles[0] == whichPlayer && markedTiles[1] == whichPlayer && markedTiles[2] == whichPlayer,
            markedTiles[3] == whichPlayer && markedTiles[4] == whichPlayer && markedTiles[5] == whichPlayer,
            markedTiles[6] == whichPlayer && markedTiles[7] == whichPlayer && markedTiles[8] == whichPlayer,
            markedTiles[0] == whichPlayer && markedTiles[3] == whichPlayer && markedTiles[6] == whichPlayer,
            markedTiles[1] == whichPlayer && markedTiles[4] == whichPlayer && markedTiles[7] == whichPlayer,
            markedTiles[2] == whichPlayer && markedTiles[5] == whichPlayer && markedTiles[8] == whichPlayer,
            markedTiles[0] == whichPlayer && markedTiles[4] == whichPlayer && markedTiles[8] == whichPlayer,
            markedTiles[2] == whichPlayer && markedTiles[4] == whichPlayer && markedTiles[6] == whichPlayer,
        };
        foreach(bool win in winChecksList)
        {
            if (win)
            {
                uiManager.SetUpWinningText(whichPlayer);
                //Debug.Log($"{ChangePlayerTurnSprite(whichPlayer);} has won!");
                gamePause = true;
                CalculateNewWeight(whichPlayer - 1);
                foreach (Button btn in titles)
                {
                    btn.interactable = false;
                }
                continue;
            }
        }
        CheckDraw();
    }

    void CheckDraw()
    {
        if (turnCount == 9 && !gamePause)
        {
            uiManager.SetUpDrawText();
            gamePause = true;
        }
    }

    public void Restart()
    {
        GameSetup();
    }

}

/*int right = -5;
int left = 5;

for(float i = left; i >= right; i -= 5)
{
    for(float j = right; j <= left; j += 5)
    {
        var newTile = Instantiate(tile, new Vector3(j, i, 1), Quaternion.identity, panel);
        newTile.onClick.AddListener(delegate { gameManager.TaskOnClick(); });
        titles.Add(newTile);
    }
}*/