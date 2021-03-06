﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script plays a designer defined cut scene when a button is clicked. This script also keeps track of how many cut scenes have played.
/// </summary>

public class MapManager : MonoBehaviour
{
    [SerializeField] private string CutScene1;
    [SerializeField] private string CutScene2;
    public static int Counter;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void ZoneOne()
    {
        SceneManager.LoadSceneAsync(CutScene1, LoadSceneMode.Single);
    }

    public void ZoneTwo()
    {
        SceneManager.LoadSceneAsync(CutScene2, LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
