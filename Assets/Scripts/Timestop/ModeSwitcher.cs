using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ModeSwitcher : MonoBehaviour
{
    private InputAction _modeSwitchInput;
    private TimestopManager _timestopManager;

    public Camera timestopCamera;
    public MovementScript movementScript;
    public Camera movementCamera;
    public GroundDetectionScript movementGrounded;
    public Rigidbody movementRigid;
    
    public bool inTimestop = false;
    // Start is called before the first frame update
    void Start()
    {
        _modeSwitchInput = InputSystem.actions.FindAction("ModeSwitch");
        _timestopManager = GetComponent<TimestopManager>();
        InitMode();
    }

    // Update is called once per frame
    void Update()
    {
        if (_modeSwitchInput.WasPressedThisFrame() && movementGrounded.grounded && movementRigid.velocity.magnitude <= 0.1f)
        {
            inTimestop = !inTimestop;
            InitMode();
        }
    }

    void InitMode()
    {
        _timestopManager.enabled = inTimestop;
        timestopCamera.enabled = inTimestop;
        movementScript.enabled = !inTimestop;
        movementCamera.enabled = !inTimestop;
    }
}
