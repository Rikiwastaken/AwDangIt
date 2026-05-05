using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class ModeSwitcher : MonoBehaviour
{
    private InputAction _modeSwitchInput;
    private TimestopManager _timestopManager;

    public CinemachineVirtualCamera timestopCamera;
    public GameObject character;
    
    private MovementScript _movementScript;
    private CinemachineVirtualCamera _movementCamera;
    private GroundDetectionScript _movementGrounded;
    private Rigidbody _movementRigid;
    private GunController _gunController;

    public bool inTimestop = false;
    // Start is called before the first frame update
    void Start()
    {
        _modeSwitchInput = InputSystem.actions.FindAction("ModeSwitch");
        _timestopManager = GetComponent<TimestopManager>();

        _movementScript = character.GetComponent<MovementScript>();
        _movementCamera = character.GetComponentInChildren<CinemachineVirtualCamera>();
        _movementGrounded = character.GetComponentInChildren<GroundDetectionScript>();
        _movementRigid = character.GetComponent<Rigidbody>();
        _gunController = character.GetComponent<GunController>();
        
        InitMode();
    }

    // Update is called once per frame
    void Update()
    {
        if (_modeSwitchInput.WasPressedThisFrame() && _movementGrounded.grounded && _movementRigid.velocity.magnitude <= 0.1f)
        {
            inTimestop = !inTimestop;
            InitMode();
        }
    }

    void InitMode()
    {
        _timestopManager.enabled = inTimestop;
        timestopCamera.enabled = inTimestop;
        _movementScript.enabled = !inTimestop;
        _movementCamera.enabled = !inTimestop;
        _gunController.enabled = !inTimestop;
        if (inTimestop)
        {
            Cursor.lockState = CursorLockMode.None;
            _timestopManager.selectedBuilding = null;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            _timestopManager.arrowSelect.gameObject.SetActive(false);
        }
    }
}
