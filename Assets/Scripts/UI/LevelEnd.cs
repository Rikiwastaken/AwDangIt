using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    public GameObject character;
    
    public void Open()
    {
        gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        character.GetComponent<MovementScript>().enabled = false;
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        character.GetComponent<GunController>().enabled = false;
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
