using UnityEngine;
using UnityEngine.InputSystem;

public class MovementScript : MonoBehaviour
{

    private Vector2 MoveValue;
    private InputAction MoveInputaction;

    private Vector2 mouseDelta;
    private InputAction MouseInputaction;

    private float jumpvalue;
    private InputAction JumpInputaction;


    private Rigidbody rb;
    private Transform CameraTransform;

    [Header("Movement Variables")]
    public float speed;

    [Header("Camera Variables")]
    public float sensitivityX = 15f;
    public float sensitivityY = 15f;
    public float minVerticalAngle = -80f;
    public float maxVerticalAngle = 80f;
    private float rotationX = 0f;
    private float rotationY = 0f;

    // Start is called before the first frame update
    void Start()
    {
        MoveInputaction = InputSystem.actions.FindAction("Movement");
        MouseInputaction = InputSystem.actions.FindAction("Mouse");
        JumpInputaction = InputSystem.actions.FindAction("Jump");
        rb = GetComponent<Rigidbody>();
        CameraTransform = GetComponentInChildren<Camera>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        MoveValue = MoveInputaction.ReadValue<Vector2>();
        mouseDelta = MouseInputaction.ReadValue<Vector2>();


        // Camera

        float mouseX = mouseDelta.x * sensitivityX * Time.deltaTime;
        float mouseY = mouseDelta.y * sensitivityY * Time.deltaTime;


        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);

        CameraTransform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        rotationY += mouseX;
        transform.rotation = Quaternion.Euler(0f, rotationY, 0f);

        // movement

        if (MoveValue.magnitude != 0)
        {
            Vector3 movement = Vector3.zero;
            movement = new Vector3(MoveValue.x * speed, 0.0f, MoveValue.y * speed);

            movement = Quaternion.Euler(0, CameraTransform.eulerAngles.y, 0) * movement;



            movement.y = rb.velocity.y;


            rb.velocity = movement;


        }
        else
        {
            Vector3 targetspeed = new Vector3(0f, rb.velocity.y, 0f);
            rb.velocity = Vector3.Lerp(rb.velocity, targetspeed, 0.5f);
        }
    }

}
