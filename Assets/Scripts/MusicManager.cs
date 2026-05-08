using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    public AudioSource mainSource;
    public AudioSource timestopSource;
    
    public void SetAngryness(float a)
    {
        mainSource.volume = a * 0.75f;
        timestopSource.volume = (1f - a) * 0.65f;
    }
}
