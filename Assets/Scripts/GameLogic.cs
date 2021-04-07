using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Globalization;

public class GameLogic : MonoBehaviour
{
    public static event Action<int[], int> AITurn;
    public static event Action<int, float> CalculateNewWeight;
    public static event Action<int> ChoseDictionary;

    [SerializeField] const float alpfa = 0.1f;

    [SerializeField] private bool plVSbot;

    [SerializeField] private int botTurn; //1 - X, 2 = 0
    [SerializeField] private int whichPlayer; //1 - X, 2 = 0
    [SerializeField] private Button[] titles;
    [SerializeField] private bool gamePause = false;
    [SerializeField] private int[] markedTiles = new int[9];

    [SerializeField] private Sprite[] playersIcons;

    private float alpfaDraw;

    private int Xwins;
    private int Owins;

    private int turnCount;

    [SerializeField] private AIManager aiManager;
    [SerializeField] private UIManager uiManager;

    private void Awake()
    {
#if UNITY_EDITOR
        MakeFileForEditor("PCdictO", "PCdictX");
        //PlayerPrefs.SetInt("FIRSTTIMEOPENING", 1);
        //MakeFileForEditor("NewdictO", "NewdictX");
#elif UNITY_ANDROID
        MakeFileForEditor("dictO", "dictX");
        //PlayerPrefs.SetInt("FIRSTTIMEOPENING", 1);
        //MakeFileForEditor("NewdictO", "NewdictX");
#endif

    }


    private void Start()
    {
        Xwins = PlayerPrefs.GetInt("Xwins");
        Owins = PlayerPrefs.GetInt("Owins");

        alpfaDraw = PlayerPrefs.GetFloat("AlpfaDraw");

        uiManager.SetUpAIBattle += SetAIvsAIBattle;
        uiManager.RestartGameEvent += GameSetup;
        uiManager.RestartAndSwitch += SetBotTurn;
        aiManager.ChangeTileIndex += ChangeStates;

        GameSetup();
    }


    void GameSetup()
    {
        whichPlayer = 1;
        //botTurn = 2;
        turnCount = 0;

        uiManager.SetIterationsCount(Xwins, Owins);
        aiManager.ClearAITemp();
        uiManager.ChangePlayerIcon(whichPlayer);
        
        for (int i = 0; i < markedTiles.Length; i++)
        {
            markedTiles[i] = 0;
            titles[i].interactable = true;
            titles[i].GetComponentInChildren<Image>().sprite = playersIcons[0];
        }
        gamePause = false;
        WhoseTurn();
    }

    private void SetBotTurn(int playerNumber)
    {
        botTurn = playerNumber;
    }

    private void SetAIvsAIBattle(bool set)
    {
        plVSbot = set;
        botTurn = 1;
    }

    private void WhoseTurn()
    {
        ChoseDictionary(botTurn);
        
        if (!plVSbot || whichPlayer == botTurn)
        {
            BotStep();
        }
    }
    private void PressTheTile(int tileIndex)
    {

        ChangeStates(tileIndex);
        CheckStatements();

        BotStep();        
    }

    private void BotStep()
    {
        if (!gamePause)
        {
            StartCoroutine(AITurnCouroutine());
        }
    }

    IEnumerator AITurnCouroutine()
    {
        yield return new WaitForSeconds(0.2f);
        AITurn(markedTiles, botTurn);
        CheckStatements();
    }

    private void CheckStatements()
    {
        if (turnCount > 4)
        {
            CheckingWinner();
        }
        if (!gamePause)
        {
            whichPlayer = whichPlayer == 1 ? 2 : 1;
            uiManager.ChangePlayerIcon(whichPlayer);
        }

        if (!plVSbot)
        {
            botTurn = whichPlayer;
            WhoseTurn();
        }
    }


    private void ChangeStates(int tileIndex)
    {
        markedTiles[tileIndex] = whichPlayer;
        Button tile = titles[tileIndex];
        tile.GetComponentInChildren<Image>().sprite = playersIcons[whichPlayer];
        tile.interactable = false;
        turnCount++;
    }


    private void CheckingWinner()
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
                gamePause = true;
                //Debug.Log($"player {whichPlayer} bot = {botTurn}");
                ReCalrulateWeughts();
                foreach (Button btn in titles)
                {
                    btn.interactable = false;
                }
                SetWinsToUI(whichPlayer);
                uiManager.SetUpWinningText(whichPlayer);
            }
        }
        CheckDraw();
    }

    private void ReCalrulateWeughts()
    {
        if (plVSbot)
        {
            CalculateNewWeight(whichPlayer == botTurn ? 1 : 0, alpfa);
        }
        else
        {
            aiManager.CalculateNewWeightsAI(whichPlayer, alpfa);
            aiManager.CalculateNewWeightsAI(whichPlayer == 1 ? 2 : 1, alpfa);
        }
    }

    private void SetWinsToUI(int winner)
    {
        if(winner == 1)
        {
            Xwins += 1;
            PlayerPrefs.SetInt("Xwins", Xwins);
        }
        else
        {
            Owins += 1;
            PlayerPrefs.SetInt("Owins", Owins);
        }
        uiManager.SetIterationsCount(Xwins, Owins);
    }

    private void CheckDraw()
    {
        if (turnCount == 9 && !gamePause)
        {
            uiManager.SetUpDrawText();
            gamePause = true;
            alpfaDraw *= 0.99f;
            PlayerPrefs.SetFloat("AlpfaDraw", alpfaDraw);
            CalculateNewWeight(whichPlayer, alpfaDraw);
        }

        
    }

    private void MakeFileForEditor(string fileNameO, string fileNameX)
    {
        if (PlayerPrefs.GetInt("FIRSTTIMEOPENING", 1) == 1)
        {
            Debug.Log("First Time Opening");

            PlayerPrefs.SetInt("FIRSTTIMEOPENING", 0);

            WriteFileInDevise(fileNameX);
            WriteFileInDevise(fileNameO);

            PlayerPrefs.SetFloat("AlpfaDraw", -0.03f);
            PlayerPrefs.SetInt("Xwins", 0);
            PlayerPrefs.SetInt("Owins", 0);
        }
        else
        {
            //Debug.Log("NOT First Time Opening");
        }
    }

    private void WriteFileInDevise(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".txt";
        TextAsset txtAsset = Resources.Load(fileName) as TextAsset;
        string txt = txtAsset.text;
        System.IO.File.WriteAllText(path, txt);
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