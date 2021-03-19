using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class File : MonoBehaviour
{

    void Start()
    {
        string path = @"F:\Repository\Tic-tac-toe\Assets\text.txt";
        var readText = System.IO.File.ReadAllText(path);
        var result = new Dictionary<string, string>();


    }

}
