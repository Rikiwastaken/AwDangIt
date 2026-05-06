using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    public static LevelEnd Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    [Header("text")]
    public TMP_Text timeText;
    public TMP_Text timestopText;
    public TMP_Text movesText;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Open()
    {
        gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        MovementScript.Instance.enabled = false;
        GunController.Instance.enabled = false;
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        MovementScript.Instance.gameObject.GetComponent<Rigidbody>().freezeRotation = true;

        TimerScript.Instance.PauseTimer();

        timeText.text = "Time: " + TimerScript.TimeToString(TimerScript.Instance.timerVal);
        timestopText.text = "Timestops used: " + ModeSwitcher.Instance.switches;
        movesText.text = "Moves: " + TimestopManager.Instance.moves;
    }
    
    public void LevelSelect()
    {
        SceneManager.LoadScene("Scenes/LevelSelect", LoadSceneMode.Single);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}
