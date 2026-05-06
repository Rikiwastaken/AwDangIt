using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class ModeSwitcher : MonoBehaviour
{
    public static ModeSwitcher Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private List<Building> _childBuildings = new List<Building>();
    private InputAction _modeSwitchInput;

    [Header("level data")] public int switches = 0;
    public int switchLimit = 3;

    [Header("accessors")] public CinemachineVirtualCamera timestopCamera;
    public GameObject crosshair;

    private CinemachineVirtualCamera _movementCamera;

    [Header("debug")] public bool inTimestop = false;
    public bool playerHasControl = false;

    private float _regainControlAt = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _modeSwitchInput = InputSystem.actions.FindAction("ModeSwitch");

        GameObject character = MovementScript.Instance.gameObject;
        _movementCamera = character.GetComponentInChildren<CinemachineVirtualCamera>();

        foreach (Transform child in transform)
        {
            Building building = child.gameObject.GetComponent<Building>();
            _childBuildings.Add(building);
        }
        
        ExitTimestop();
        EnterMovement();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHasControl)
        {
            if (!inTimestop)
            {
                Vector3 horiz = MovementScript.Instance.velocity;
                horiz.y = 0;
                if (_modeSwitchInput.WasPressedThisFrame() && MovementScript.Instance.IsGrounded() &&
                    horiz.magnitude <= 0.1f && switches < switchLimit)
                {
                    ExitMovement();
                    
                    _regainControlAt = Time.time + 0.5f;
                }
            }
            else
            {
                if (_modeSwitchInput.WasPressedThisFrame())
                {
                    ExitTimestop();
                    
                    _regainControlAt = Time.time + 0.5f;
                }
            }
        }
        else
        {
            if (Time.time > _regainControlAt)
            {
                playerHasControl = true;

                if (inTimestop)
                {
                    EnterTimestop();
                }
                else
                {
                    EnterMovement();
                }
            }
        }
    }

    private void EnterMovement()
    {
        crosshair.SetActive(true);
        MovementScript.Instance.enabled = true;
        GunController.Instance.enabled = true;
        TimerScript.Instance.UnPauseTimer();
    }

    private void ExitMovement()
    {
        inTimestop = true;
        playerHasControl = false;
        timestopCamera.enabled = true;
        _movementCamera.enabled = false;
        crosshair.SetActive(false);
        MovementScript.Instance.enabled = false;
        GunController.Instance.enabled = false;
        TimerScript.Instance.PauseTimer();
    }

    private void EnterTimestop()
    {
        TimestopManager.Instance.control = true;
        TimestopManager.Instance.selectedBuilding = null;
        TimestopManager.Instance.buildingMoved = false;
        GridGenerator.Instance.gameObject.SetActive(true);
        foreach (var b in _childBuildings)
        {
            b.ShowSelector();
        }

        Cursor.lockState = CursorLockMode.None;
    }

    private void ExitTimestop()
    {
        inTimestop = false;
        TimestopManager.Instance.control = false;
        playerHasControl = false;
        timestopCamera.enabled = false;
        _movementCamera.enabled = true;
        GridGenerator.Instance.gameObject.SetActive(false);
        foreach (var b in _childBuildings)
        {
            b.HideSelector();
        }

        Cursor.lockState = CursorLockMode.Locked;
        if (TimestopManager.Instance.buildingMoved)
        {
            switches++;
        }
    }
}
