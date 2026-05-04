using Cinemachine;
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
    private GroundDetectionScript groundDetectionScript;

    private Animator animator;

    [Header("Movement Variables")]
    public float groundSpeed;
    public float airborneSpeed;

    [Header("Camera Variables")]
    public float sensitivityX = 15f;
    public float sensitivityY = 15f;
    public float minVerticalAngle = -80f;
    public float maxVerticalAngle = 80f;
    private float rotationX = 0f;
    private float rotationY = 0f;

    [Header("Jump variables")]
    public float JumpVerticalSpeed;
    public float DoubleJumpSpeedRatio;
    private bool pressedjump;
    public bool jumpavailable;
    public bool doublejumpavailable;
    private int justjumpedcounter;
    public float jumpduration;
    public float downacceleration;
    private bool previousgrounded;

    // Start is called before the first frame update
    void Start()
    {
        MoveInputaction = InputSystem.actions.FindAction("Movement");
        MouseInputaction = InputSystem.actions.FindAction("Mouse");
        JumpInputaction = InputSystem.actions.FindAction("Jump");
        rb = GetComponent<Rigidbody>();
        CameraTransform = GetComponentInChildren<CinemachineVirtualCamera>().transform;
        groundDetectionScript = GetComponentInChildren<GroundDetectionScript>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveValue = MoveInputaction.ReadValue<Vector2>();
        mouseDelta = MouseInputaction.ReadValue<Vector2>();


        // Camera

        float mouseX = mouseDelta.x * sensitivityX;
        float mouseY = mouseDelta.y * sensitivityY;


        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);

        CameraTransform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        rotationY += mouseX;
        transform.rotation = Quaternion.Euler(0f, rotationY, 0f);

        // movement

        if (MoveValue.magnitude != 0)
        {
            float speed = airborneSpeed;
            if (groundDetectionScript.grounded)
            {
                speed = groundSpeed;
            }
            Vector3 movement = Vector3.zero;
            movement = new Vector3(MoveValue.x * speed, 0.0f, MoveValue.y * speed);

            movement = Quaternion.Euler(0, CameraTransform.eulerAngles.y, 0) * movement;



            movement.y = rb.velocity.y;


            rb.velocity = movement;

            if (groundDetectionScript.grounded)
            {
                animator.SetFloat("SpeedX", MoveValue.x * 2f);
                animator.SetFloat("SpeedZ", MoveValue.y * 2f);
            }
        }
        else
        {
            Vector3 targetspeed = new Vector3(0f, rb.velocity.y, 0f);
            rb.velocity = Vector3.Lerp(rb.velocity, targetspeed, 0.5f);
        }

        // jump

        if (groundDetectionScript.grounded)
        {
            jumpavailable = true;
            doublejumpavailable = true;
        }

        if (justjumpedcounter > 0)
        {
            justjumpedcounter--;
        }

        if (JumpInputaction.IsPressed())
        {
            if (jumpavailable)
            {

                jumpavailable = false;
                pressedjump = true;
                justjumpedcounter = (int)(jumpduration / Time.deltaTime);
                rb.velocity = new Vector3(rb.velocity.x, JumpVerticalSpeed, rb.velocity.z);
                animator.Play("Jump");

            }

            else if (doublejumpavailable && !pressedjump)
            {
                doublejumpavailable = false;
                pressedjump = true;
                justjumpedcounter = (int)(jumpduration / Time.deltaTime);
                rb.velocity = new Vector3(rb.velocity.x, JumpVerticalSpeed * DoubleJumpSpeedRatio, rb.velocity.z);
                animator.Play("DoubleJump");
            }
        }
        else
        {
            // if (justjumpedcounter > 0)
            // {
            //     if (doublejumpavailable)
            //     {
            //         rb.velocity = new Vector3(rb.velocity.x, JumpVerticalSpeed, rb.velocity.z);
            //     }
            //     else
            //     {
            //         rb.velocity = new Vector3(rb.velocity.x, JumpVerticalSpeed * DoubleJumpSpeedRatio, rb.velocity.z);
            //     }
            //
            // }

            if (pressedjump)
            {
                pressedjump = false;
            }

        }

        // animations

        if (groundDetectionScript.grounded)
        {
            if (!previousgrounded)
            {
                animator.Play("Fall To Roll");
            }
            animator.SetBool("Falling", false);
        }
        else
        {
            animator.SetBool("Falling", true);
            animator.SetFloat("SpeedX", 0f);
            animator.SetFloat("SpeedZ", 0f);
        }



        previousgrounded = groundDetectionScript.grounded;
    }

}
