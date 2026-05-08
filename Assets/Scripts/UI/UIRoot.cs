using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIRoot : MonoBehaviour
{
    private InputAction _restartAction;

    private void Start()
    {
        _restartAction = InputSystem.actions.FindAction("Restart");
    }

    private void Update()
    {
        if (_restartAction.WasPressedThisFrame())
        {
            LoadingScreenScript.instance.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
