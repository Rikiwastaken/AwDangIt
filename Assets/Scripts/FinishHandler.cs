using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            Cursor.lockState = CursorLockMode.None;
            LoadingScreenScript.instance.LoadScene("Scenes/LevelSelect");
        }
    }
}
