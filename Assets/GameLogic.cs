using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Globalization;

public class GameLogic : MonoBehaviour
{
    public int whichPlayer; //1 - X, 2 = 0
    public int turnCount;
    public Text whoseTurn;
    public Text winner;
    public Button tile;
    public Transform panel;
    public Button[] titles;
    public bool gamePause = false;
    public int[] markedTiles = new int[9];

    Dictionary<int, string> playersIcons = new Dictionary<int, string> { {1,"X" }, { 2, "O" } };
    Dictionary<string, float> all = new Dictionary<string, float>();

    List<string> AISteps = new List<string>();

    private float alpfa = 0.1f;

    private void Start()
    {
        GameSetup();
    }

    void GameSetup()
    {
        whichPlayer = 1;
        turnCount = 0;
        whoseTurn.text = playersIcons[1];
        AISteps.Clear();

        WritingFile();
        for(int i = 0; i < markedTiles.Length; i++)
        {
            markedTiles[i] = 0;
        }
    }


    public void PressTheTile(int tileIndex)
    {
        turnCount++;
        ChangeStates(tileIndex);
        
        if (turnCount > 4)
        {
            CheckingWinner();
        }

        if (whichPlayer == 2 && !gamePause)
        {
            AITurn();
        }

    }

    void ChangeStates(int tileIndex)
    {
        markedTiles[tileIndex] = whichPlayer;
        var tile = titles[tileIndex];
        var localTile = tile.GetComponentInChildren<Text>();
        localTile.text = playersIcons[whichPlayer];
        tile.interactable = false;
        if (!gamePause)
        {
            whichPlayer = whichPlayer == 1 ? 2 : 1;
        }
        whoseTurn.text = playersIcons[whichPlayer];
    }

    void AITurn()
    {  
        MakePosibleMoves();       
    }

    void MakePosibleMoves()
    {
        List<string> posibleMoves = new List<string>();
        string result = string.Join("", markedTiles);
        for(int i = 0; i < result.Length; i++)
        {
            var temp = result;
            if (result[i] == '0')
            {
                posibleMoves.Add(temp.Remove(i, 1).Insert(i, "2")) ;
            }
        }

        string bestMove = "";
        float bestScore = 0f;

        foreach (KeyValuePair<string, float> keyValue in all)
        {
            foreach(var move in posibleMoves)
            {
                if (keyValue.Key == move)
                {
                    //Debug.Log($"{keyValue.Key} - {keyValue.Value}");
                    if(bestScore < keyValue.Value)
                    {
                        bestScore = keyValue.Value;
                        bestMove = keyValue.Key;
                    }
                    /*if (UnityEngine.Random.Range(0, 11) == 1)
                    {
                        bestScore = 100f;
                    }*/
                }
            }  
        }
        //Debug.Log($"{bestMove} - {bestScore}");
        AISteps.Add(bestMove);
        for (int i = 0; i < result.Length; i++)
        {
            if (bestMove[i] != '0' && markedTiles[i] == 0)
            {
                Debug.Log($"{i}");
                ChangeStates(i);
            }
        }
    }

    void WritingFile()
    {
        string path = @"F:\Repository\Tic-tac-toe\Assets\text2.txt";

        using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                all.Add(line.Split(' ')[0], Convert.ToSingle(line.Split(' ')[1]));
            }
        }
    }

    void CalculateNewWeights(int winner)
    {
        foreach (var keyValue in all)
        {
            foreach (var move in AISteps)
            {
                if(move == keyValue.Key)
                {
                    
                    float newWeight = keyValue.Value + alpfa * (winner - keyValue.Value);
                    all[keyValue.Key] = newWeight;
                }
            }
        }
        ReWriting();
    }

    void ReWriting()
    {
        Debug.Log("newWeight");
        string path = @"F:\Repository\Tic-tac-toe\Assets\text2.txt";
        FileStream file1 = new FileStream(path, FileMode.Create); //создаем файловый поток
        StreamWriter writer = new StreamWriter(file1); //создаем «потоковый писатель» и связываем его с файловым потоком
        foreach (var keyValue in all)
        {
            Debug.Log("Rewriting");
            writer.Write(string.Join(" ", keyValue.Key, keyValue.Value));
        }

        Debug.Log("Rewriting Finish");     //записываем в файл
        writer.Close(); //закрываем поток. Не закрыв поток, в файл ничего не запишется
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
        foreach(var win in winChecksList)
        {
            if (win)
            {
                winner.text = "Winner";
                Debug.Log($"{playersIcons[whichPlayer]} has won!");
                gamePause = true;
                CalculateNewWeights(whichPlayer - 1);
                foreach (Button btn in titles)
                {
                    btn.interactable = false;
                }
                
            }
        }
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