using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;

public class MovementScript : MonoBehaviour
{
    public static MovementScript Instance { get; private set; }

    private Vector2 MoveValue;
    private InputAction MoveInputaction;

    private Vector2 mouseDelta;
    private InputAction MouseInputaction;

    private float jumpvalue;
    private InputAction JumpInputaction;


    private CharacterController cc;

    private Transform CameraTransform;

    private Animator animator;


    [Header("Movement Variables")]
    public float groundSpeed;
    public float airborneSpeed;
    public float strafeRatio;
    public float backstepRatio;
    public float minspeedforpostprocessing;

    [Header("Camera Variables")]
    public float sensitivityX = 15f;
    public float sensitivityY = 15f;
    public float minVerticalAngle = -80f;
    public float maxVerticalAngle = 80f;
    public float rotationX = 0f;
    public float rotationY = 0f;

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

    [Header("Post Processing Variables")]
    public float timetosetweight;
    private float lastweight;
    private PostProcessVolume volume;

    [Header("SFX DRIVERS")]
    public SFXDriver sfxJumpDriver;
    public SFXDriver sfxStepDriver;

    [Header("extern")]
    public Vector3 droneTarget;
    
    [Header("debug")]
    public Vector3 velocity;
    public Building lastBuilding;
    public bool canControl = true;
    
    public void OnDisable()
    {
        velocity = Vector3.zero;
    }

    private void Awake()
    {
        Instance = this;
    }

    public bool IsGrounded()
    {
        return cc.isGrounded;
    }

    // Start is called before the first frame update
    void Start()
    {
        MoveInputaction = InputSystem.actions.FindAction("Movement");
        MouseInputaction = InputSystem.actions.FindAction("Mouse");
        JumpInputaction = InputSystem.actions.FindAction("Jump");
        cc = GetComponent<CharacterController>();
        CameraTransform = GetComponentInChildren<CinemachineVirtualCamera>().transform;
        volume = FindAnyObjectByType<PostProcessVolume>();
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

        if (MoveValue.magnitude != 0 && canControl)
        {
            float speed = airborneSpeed;
            if (cc.isGrounded)
            {
                speed = groundSpeed;
            }
            Vector3 movement = Vector3.zero;
            if (MoveValue.y == 0)
            {
                movement = new Vector3(MoveValue.x * speed * strafeRatio, 0.0f, MoveValue.y * speed);
            }
            else if (MoveValue.y < 0)
            {
                movement = new Vector3(MoveValue.x * speed, 0.0f, MoveValue.y * speed * backstepRatio);
            }
            else
            {
                movement = new Vector3(MoveValue.x * speed, 0.0f, MoveValue.y * speed);
            }


            movement = Quaternion.Euler(0, CameraTransform.eulerAngles.y, 0) * movement;



            movement.y = velocity.y;

            velocity = movement;
        }
        else
        {
            Vector3 targetspeed = new Vector3(0f, velocity.y, 0f);
            velocity = Vector3.Lerp(velocity, targetspeed, 0.5f);

        }
        if (cc.isGrounded)
        {
            animator.SetFloat("SpeedX", MoveValue.x * 2f);
            animator.SetFloat("SpeedZ", MoveValue.y * 2f);
        }

        // jump

        if (cc.isGrounded)
        {
            jumpavailable = true;
            doublejumpavailable = true;
            velocity = new Vector3(velocity.x, -1, velocity.z); //i don't want to know why it has to fall
        }
        else
        {
            jumpavailable = false;
            velocity += Physics.gravity * Time.deltaTime;
        }

        if (justjumpedcounter > 0)
        {
            justjumpedcounter--;
        }

        if (JumpInputaction.IsPressed() && canControl)
        {
            
            if (jumpavailable)
            {

                jumpavailable = false;
                pressedjump = true;
                justjumpedcounter = (int)(jumpduration / Time.deltaTime);
                velocity = new Vector3(velocity.x, JumpVerticalSpeed, velocity.z);
                animator.Play("Jump");
                sfxJumpDriver.PlayRandomSound();
            }

            else if (doublejumpavailable && !pressedjump)
            {
                doublejumpavailable = false;
                pressedjump = true;
                justjumpedcounter = (int)(jumpduration / Time.deltaTime);
                velocity = new Vector3(velocity.x, JumpVerticalSpeed * DoubleJumpSpeedRatio, velocity.z);
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

        Vector3 projected = velocity;
        projected.y = 0;
        projected.Normalize();
        if (projected.magnitude != 0 || Mathf.Abs(velocity.y) >= 1.5f)
        {
            droneTarget = transform.position - projected * 2.5f + transform.up * 2.5f;
        }
        
        if (!GrappleController.Instance.Isgrappling)
        {
            cc.Move(velocity * Time.deltaTime);
        }


        // animations

        if (cc.isGrounded)
        {
            if (!previousgrounded)
            {
                sfxStepDriver.PlayRandomSound();
            }
            if (!previousgrounded && velocity.y < -10)
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



        previousgrounded = cc.isGrounded;

        // bob
        
        
        // post precessing

        float magnitude = velocity.magnitude / minspeedforpostprocessing;
        lastweight = Mathf.Lerp(lastweight, magnitude, timetosetweight);
        volume.weight = lastweight;
    }

    public void Respawn()
    {
        if (lastBuilding)
        {
            GetComponent<CharacterController>().enabled = false;
            transform.position = lastBuilding.transform.position + new Vector3(0, 5, 0);
            GetComponent<CharacterController>().enabled = true;
        }

        canControl = true;
    }
}
