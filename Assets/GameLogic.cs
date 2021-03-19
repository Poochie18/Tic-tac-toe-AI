using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private void Start()
    {
        GameSetup();
    }

    void GameSetup()
    {
        whichPlayer = 1;
        turnCount = 0;
        whoseTurn.text = "X";

        for(int i = 0; i < markedTiles.Length; i++)
        {
            markedTiles[i] = 0;
        }
    }


    public void PressTheTile(int tileIndex)
    {
        
        var chosenTitle = titles[tileIndex];
        
        turnCount++;
        markedTiles[tileIndex] = whichPlayer;
        if (turnCount > 4)
        {
            CheckingWinner();
        }
        //string result = string.Join("", markedTiles);
        ChangeStates(chosenTitle);
    }

    void ChangeStates(Button tile)
    {
        var localTile = tile.GetComponentInChildren<Text>();
        localTile.text = playersIcons[whichPlayer];
        tile.interactable = false;
        if (!gamePause)
        {
            whichPlayer = whichPlayer == 1 ? 2 : 1;
        }
        whoseTurn.text = playersIcons[whichPlayer];
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
            markedTiles[2] == whichPlayer && markedTiles[3] == whichPlayer && markedTiles[4] == whichPlayer,
        };
        foreach(var win in winChecksList)
        {
            if (win)
            {
                winner.text = "Winner";
                Debug.Log($"{playersIcons[whichPlayer]} has won!");
                gamePause = true;
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