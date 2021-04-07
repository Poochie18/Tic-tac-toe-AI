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

    private List<string> AISteps = new List<string>();

    private List<string> posibleMoves = new List<string>();

    private int[] markedTiles = new int[9];

    public Dictionary<string, float> Ostatements = new Dictionary<string, float>();
    public Dictionary<string, float> Xstatements = new Dictionary<string, float>();

    public Dictionary<string, float> mainStatements = new Dictionary<string, float>();

    [SerializeField] private float epsilon;

    [SerializeField] private Text streamText;
    [SerializeField] private Text newWeights;

    private int winnerNum = 1;

    string pathX;
    string pathO;

    string mainPath;
    private void Start()
    {
#if UNITY_EDITOR
        pathX = Application.persistentDataPath + "/PCdictX.txt";
        WritingFile(pathX, Xstatements);
        pathO = Application.persistentDataPath + "/PCdictO.txt";
        WritingFile(pathO, Ostatements);

#elif UNITY_ANDROID
        pathX = Application.persistentDataPath + "/dictX.txt";
        WritingFile(pathX, Xstatements);
        pathO = Application.persistentDataPath + "/dictO.txt";
        WritingFile(pathO, Ostatements);
        
#endif
        epsilon = PlayerPrefs.GetFloat("Epsilon");
        //PlayerPrefs.SetFloat("Epsilon", 0.3f);
        GameLogic.ChoseDictionary += SetMainDictionary;
        GameLogic.AITurn += FindPossipleTurns;
        GameLogic.CalculateNewWeight += CalculateNewWeights;
        
    }

    private void WritingFile(string path, Dictionary<string, float> dict)
    {
        using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                dict.Add(line.Split(' ')[0], Convert.ToSingle(line.Split(' ')[1]));
            }
        }
    }


    private void SetMainDictionary(int whichDict)
    {
        if (whichDict == 1)
        {
            mainStatements = Xstatements;
            mainPath = pathX;
        }
        else
        {
            mainStatements = Ostatements;
            mainPath = pathO;
        }
    }

    private void FindPossipleTurns(int[] marked, int marker)
    {
        markedTiles = marked;
        posibleMoves.Clear();
        string result = string.Join("", markedTiles);
        for (int i = 0; i < result.Length; i++)
        {
            var temp = result;
            if (result[i] == '0')
            {
                posibleMoves.Add(temp.Remove(i, 1).Insert(i, marker.ToString()));
            }
        }

        ChoosePosibleMoves();
    }

    private void ChoosePosibleMoves()
    {
        string bestMove = "";
        float bestScore = 0f;
        if (UnityEngine.Random.Range(0f, 1f) <= epsilon)
        {
            int rnd = UnityEngine.Random.Range(0, posibleMoves.Count);
            bestMove = posibleMoves[rnd];
            bestScore = mainStatements[bestMove];
            epsilon *= 0.99f;
            Debug.Log("eps " + epsilon);
            PlayerPrefs.SetFloat("Epsilon", epsilon);
        }
        else
        {
            foreach (KeyValuePair<string, float> keyValue in mainStatements)
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
        
        
        streamText.text += string.Join(" - ", bestMove, Math.Round(bestScore, 3)) + "\n";
        //Debug.Log($"{bestMove} - {bestScore}");
        AISteps.Add(bestMove);
        
        MakeBestMove(bestMove);
    }

    private void MakeBestMove(string bestStep)
    {
        for (int i = 0; i < bestStep.Length; i++)
        {
            if (bestStep[i] != '0' && markedTiles[i] == 0)
            {
                //Debug.Log(i);
                ChangeTileIndex(i);
            }
        }
    }

    private void CalculateNewWeights(int winner, float alpfa)
    {
        List<string> keys = new List<string>(mainStatements.Keys);
        
        foreach (string key in keys)
        {
            foreach (var move in AISteps)
            {
                if (move == key)
                {
                    float newWeight = mainStatements[key] + alpfa * (winner - mainStatements[key]);
                    //Debug.Log($"{mainStatements[key]} - {newWeight}");
                    mainStatements[key] = newWeight;
                    
                    newWeights.text += string.Join("", Math.Round(newWeight, 2)) + "\n";
                }
            }
        }

        ReWriting();
    }

    private List<string> SplitSteps(int winner)
    {
        List<string> tempSteps = new List<string>();
        for (int i = Math.Abs(1 - winner); i < AISteps.Count; i += 2)
        {
            tempSteps.Add(AISteps[i]);
        }
        return tempSteps;
    }

    public void CalculateNewWeightsAI(int player, float alpfa)
    {
        SetMainDictionary(player);
        List<string> keys = new List<string>(mainStatements.Keys);
        var tempSteps = SplitSteps(player);
        foreach (string key in keys)
        {
            foreach (var move in tempSteps)
            {
                if (move == key)
                {
                    float newWeight = mainStatements[key] + alpfa * (winnerNum - mainStatements[key]);

                    
                    Debug.Log($"{mainStatements[key]} - {newWeight}");
                    mainStatements[key] = newWeight;

                    newWeights.text += string.Join("", Math.Round(newWeight, 2)) + "\n";
                }
            }
        }
        winnerNum = winnerNum == 1 ? 0 : 1;
        ReWriting();
    }

    private void ReWriting()
    {
        
        using (StreamWriter sr = new StreamWriter(mainPath, false))
        {
            foreach(KeyValuePair<string, float> keyValue in mainStatements)
            {
                string line = $"{keyValue.Key} {keyValue.Value}";
                sr.WriteLine(line);
            }
        }
        //Debug.Log("Check");
    }

    public void ClearAITemp()
    {
        winnerNum = 1;
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
