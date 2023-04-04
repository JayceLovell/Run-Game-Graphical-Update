using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMaze : MonoBehaviour
{
    public int Width;
    public int Height;


    // The prefabs for the different maze pieces
    public GameObject CenterMaze;
    public GameObject CornerMaze;
    public GameObject DeadEndMaze;
    public GameObject TMaze;
    public GameObject Beginning;
    public GameObject End;

    void Start()
    {
        CreateMaze();
    }
    private void CreateMaze()
    {
       
    }
}