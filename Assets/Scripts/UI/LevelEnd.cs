using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    [Header("accessors")]
    public GameObject character;
    public ModeSwitcher modeSwitcher;
    public TimestopManager timestopManager;

    [Header("text")]
    public TMP_Text timestopText;
    public TMP_Text movesText;
    
    public void Open()
    {
        gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        character.GetComponent<MovementScript>().enabled = false;
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        character.GetComponent<GunController>().enabled = false;

        timestopText.text = "Timestops used: " + modeSwitcher.switches;
        movesText.text = "Moves: " + timestopManager.moves;
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
