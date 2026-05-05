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
    
    private InputAction _modeSwitchInput;

    [Header("level data")]
    public int switches = 0;
    public int switchLimit = 3;
    
    [Header("accessors")]
    public CinemachineVirtualCamera timestopCamera;
    public GameObject crosshair;
    
    private CinemachineVirtualCamera _movementCamera;
    private GroundDetectionScript _movementGrounded;
    private Rigidbody _movementRigid;

    [Header("debug")]
    public bool inTimestop = false;
    // Start is called before the first frame update
    void Start()
    {
        _modeSwitchInput = InputSystem.actions.FindAction("ModeSwitch");

        GameObject character = MovementScript.Instance.gameObject;
        _movementCamera = character.GetComponentInChildren<CinemachineVirtualCamera>();
        _movementGrounded = character.GetComponentInChildren<GroundDetectionScript>();
        _movementRigid = character.GetComponent<Rigidbody>();
        
        InitMode();
    }

    // Update is called once per frame
    void Update()
    {
        if (_modeSwitchInput.WasPressedThisFrame() && _movementGrounded.grounded && _movementRigid.velocity.magnitude <= 0.1f && switches < switchLimit)
        {
            inTimestop = !inTimestop;
            InitMode();
        }
    }

    void InitMode()
    {
        TimestopManager.Instance.enabled = inTimestop;
        timestopCamera.enabled = inTimestop;
        crosshair.SetActive(!inTimestop);
        MovementScript.Instance.enabled = !inTimestop;
        _movementCamera.enabled = !inTimestop;
        GunController.Instance.enabled = !inTimestop;
        if (inTimestop)
        {
            Cursor.lockState = CursorLockMode.None;
            TimestopManager.Instance.selectedBuilding = null;
            TimestopManager.Instance.buildingMoved = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            ArrowSelectIdle.Instance.gameObject.SetActive(false);
            if (TimestopManager.Instance.buildingMoved)
            {
                switches++;
            }
        }
    }
}
