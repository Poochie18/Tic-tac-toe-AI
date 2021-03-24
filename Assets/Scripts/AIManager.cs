using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Globalization;

public class AIManager : MonoBehaviour
{
    public event Action<int> ChangeTileIndex;

    const float alpfa = 0.15f;

    private List<string> AISteps = new List<string>();

    private List<string> posibleMoves = new List<string>();

    private int[] markedTiles = new int[9];

    public Dictionary<string, float> allStatements = new Dictionary<string, float>();

    [SerializeField] private Text streamText;
    [SerializeField] private Text newWeights;

    string path;

    private void Start()
    {
#if UNITY_EDITOR
        path = Application.persistentDataPath + "/text2.txt";
        Debug.Log("Editor");
#elif UNITY_ANDROID
        path = Application.persistentDataPath + "/data.txt";
        Debug.Log("Android");
#endif
        GameLogic.AITurn += FindPossipleTurns;
        GameLogic.CalculateNewWeight += CalculateNewWeights;
        WritingFile();
    }

    private void WritingFile()
    {
        using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                allStatements.Add(line.Split(' ')[0], Convert.ToSingle(line.Split(' ')[1]));
            }
        }
    }


    private void FindPossipleTurns(int[] marked)
    {
        markedTiles = marked;
        posibleMoves.Clear();
        string result = string.Join("", markedTiles);
        for (int i = 0; i < result.Length; i++)
        {
            var temp = result;
            if (result[i] == '0')
            {
                posibleMoves.Add(temp.Remove(i, 1).Insert(i, "2"));
            }
        }
        ChoosePosibleMoves();
    }

    private void ChoosePosibleMoves()
    {
        string bestMove = "";
        float bestScore = 0f;

        if(UnityEngine.Random.Range(0,10) == 1)
        {
            int rnd = UnityEngine.Random.Range(0, posibleMoves.Count);
            bestMove = posibleMoves[rnd];
            bestScore = allStatements[bestMove];
            //Debug.Log("Random");
        }
        else
        {
            foreach (KeyValuePair<string, float> keyValue in allStatements)
            {
                foreach (var move in posibleMoves)
                {
                    if (keyValue.Key == move)
                    {
                        //Debug.Log($"{keyValue.Key} - {keyValue.Value}");
                        if (bestScore <= keyValue.Value)
                        {
                            bestScore = keyValue.Value;
                            bestMove = keyValue.Key;
                        }
                    }
                }
            }
        }
        
        //Debug.Log($"{bestMove} - {bestScore}");
        streamText.text += string.Join(" - ", bestMove, Math.Round(bestScore, 3)) + "\n";
        AISteps.Add(bestMove);
        MakeBestMove(bestMove);
    }

    private void MakeBestMove(string bestStep)
    {
        for (int i = 0; i < bestStep.Length; i++)
        {
            if (bestStep[i] != '0' && markedTiles[i] == 0)
            {
                ChangeTileIndex(i);
            }
        }
    }

    private void CalculateNewWeights(int winner)
    {
        List<string> keys = new List<string>(allStatements.Keys);

        foreach (string key in keys)
        {
            foreach (var move in AISteps)
            {
                if (move == key && allStatements[key] != 2)
                {
                    float newWeight = allStatements[key] + alpfa * (winner - allStatements[key]);
                    allStatements[key] = newWeight;

                    newWeights.text += string.Join("", Math.Round(newWeight, 2)) + "\n";
                }
            }
        }
        ReWriting();
    }

    private void ReWriting()
    {
        
        using (StreamWriter sr = new StreamWriter(path, false))
        {
            foreach(KeyValuePair<string, float> keyValue in allStatements)
            {
                string line = $"{keyValue.Key} {keyValue.Value}";
                sr.WriteLine(line);
            }
        }
        //Debug.Log("Check");
    }

    public void ClearAITemp()
    {
        newWeights.text = null;
        streamText.text = null;
        AISteps.Clear();
        posibleMoves.Clear();
    }

    private void DeleteAI()
    {
        GameLogic.AITurn -= FindPossipleTurns;
        GameLogic.CalculateNewWeight -= CalculateNewWeights;
    }
}
