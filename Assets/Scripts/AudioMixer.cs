﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioMixer : MonoBehaviour
{
    public Slider MasterVolumeSlider;
    public Slider AmbientVolumeSlider;
    public Slider MusicVolumeSlider;
    public Slider SFXVolumeSlider;

    private GameObject Player;
    private GameObject[] Enemies;
    private GameObject AudioManager;

    public static AudioMixer Instance = null;

    private void Awake()
    {
        AudioManager = GameObject.Find("Audio Manager");

        if (Instance == null)
        {
            Instance = this;
        }

        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    public void OnMasterVolumeChange()
    {
        AudioManager.transform.GetChild(0).GetComponent<AudioSource>().outputAudioMixerGroup.audioMixer.SetFloat("Master", MasterVolumeSlider.value);
    }

    public void OnAmbientVolumeChange()
    {
        AudioManager.transform.GetChild(1).GetComponent<AudioSource>().outputAudioMixerGroup.audioMixer.SetFloat("Ambient", AmbientVolumeSlider.value);
    }

    public void OnMusicVolumeChange()
    {
        AudioManager.transform.GetChild(2).GetComponent<AudioSource>().outputAudioMixerGroup.audioMixer.SetFloat("Music", MusicVolumeSlider.value);
    }

    public void OnSFXVolumeChange()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            Player.GetComponent<AudioSource>().outputAudioMixerGroup.audioMixer.SetFloat("SFX", SFXVolumeSlider.value);

            foreach (GameObject Enemy in Enemies)
            {
                Enemy.GetComponent<AudioSource>().outputAudioMixerGroup.audioMixer.SetFloat("SFX", SFXVolumeSlider.value);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}