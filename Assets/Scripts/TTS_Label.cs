﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class TTS_Label : MonoBehaviour, IPointerEnterHandler
{
    private GameManager GameManager;
    private void Start()
    {
        GameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (this.enabled == true)
        {
            GameManager.Speak(transform.GetComponent<Text>().text);
        }
    }
}