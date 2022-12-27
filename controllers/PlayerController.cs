using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //inspector display for info on the player model
    [Header("Player Data")]
    public float jumpHeight;
    public float sprintSpeed;
    public float maxSprintSpeed;
    public float playerSpeed;
    public float crouchSpeed;
    public float slideBoost;
    public float slideDecayRate;
    [SerializeField]
    private float sprintRampUp;
    [SerializeField]
    private float sprintRampDown;
    public float walkSpeed;

    [Header("Stats")]
    public float gravity = -9.81f;
    public float maxFallSpeed = 54.0f;
    float xAxis;
    float zAxis;

    [Header("Components")]
    public Rigidbody playerRB;
    [SerializeField]
    private CharacterController controller;

    [Header("States")]
    public bool canMove = true;
    public bool canControlPlayer = true;
    public bool isMoving = false;
    public bool isSprinting;
    public bool isGrounded;
    public bool canShootGun;
    public bool isCrouched = false;
    bool crouchInternalBool = true;
    [SerializeField]
    bool notSliding = true;

    [Header("Ground")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Extras")]
    [SerializeField]
    Vector3 velocity;
    
    // Start is called before the first frame update
    void Start()
    {
        //components
        controller = GetComponent<CharacterController>();
        initialisingCommands();
    }
    // Update is called once per frame
    void Update()
    {
        PlayerControls();
        xAxis = Input.GetAxis("Horizontal");
        zAxis = Input.GetAxis("Vertical");
    }
    void initialisingCommands()
    {
        playerSpeed = walkSpeed;
    }
    public void PlayerControls()
    {
        #region gravity
        //gravity
        //velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        if (velocity.y > -maxFallSpeed)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else if (velocity.y < -maxFallSpeed)
        {
            velocity.y = -maxFallSpeed;
        }
        #endregion
        //Movement
        if (canMove)
        {
            float xAxis = Input.GetAxis("Horizontal");
            float zAxis = Input.GetAxis("Vertical");

            #region Basic Move
            Vector3 move = transform.right * xAxis + transform.forward * zAxis;
            controller.Move(move * playerSpeed * Time.deltaTime);

            if (velocity.y > -maxFallSpeed)
            {
                velocity.y += gravity * Time.deltaTime;
            }
            else if (velocity.y < -maxFallSpeed)
            {
                velocity.y = -maxFallSpeed;
            }
            #endregion
            //Movement
            if (canMove)
            {
                #region can i control the player?
                if (canControlPlayer)
                {
                    move = transform.right * xAxis + transform.forward * zAxis;
                    //Debug.Log(xAxis);
                    //Debug.Log(zAxis);
                    controller.Move(move * playerSpeed * Time.deltaTime);
                    //Debug.Log(move);
                }
                else
                {
                    move = new Vector3(0, 0, 0);
                    //Debug.Log(move);
                }
                if (xAxis == 0 && zAxis == 0)
                {
                    isMoving = false;
                    if (isGrounded == false)
                    {
                        isMoving = true;
                    }
                }
                else
                {
                    isMoving = true;
                }
                #endregion
                #region Jumping
                //ground check
                isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
                if (isGrounded && velocity.y < 0)
                {
                    velocity.y = -2f;
                }

                //Jumping mechnics
                if (Input.GetButton("Jump") && isGrounded == true)
                {
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                    isGrounded = false;
                }
                #endregion
                #region Sprinting
                if (Input.GetKey(KeyCode.LeftShift) && isGrounded && playerSpeed >= walkSpeed && !isCrouched)
                {
                    Debug.Log("is trying to sprint");
                    //rampup must be greater than 1
                    playerSpeed = playerSpeed * sprintRampUp;
                    isSprinting = true;
                    Debug.Log(playerSpeed = playerSpeed * sprintRampUp);
                    if (playerSpeed >= maxSprintSpeed)
                    {
                        //Debug.Log("is trying to sprint");
                        playerSpeed = maxSprintSpeed;
                    }
                }
                else
                {
                    //rampdown must be less than 1
                    if (isSprinting && playerSpeed >= walkSpeed && !isCrouched)
                    {
                        playerSpeed = playerSpeed * sprintRampDown;
                        //slowing down now
                        //Debug.Log("slowing down");
                    }
                    else if (isSprinting && playerSpeed <= walkSpeed && !isCrouched)
                    {
                        //no longer sprinting
                        isSprinting = false;
                        Debug.Log("no longer sprinting and speed is" + playerSpeed);
                    }
                    else if (!isSprinting && playerSpeed <= walkSpeed && notSliding)
                    {
                        playerSpeed = walkSpeed;
                    }
                }
                #endregion
                #region Crouching and Sliding
                //crouch movement
                //Debug.Log("is it grounded? =" + isGrounded + ", is slide completed? = " + notSliding + ", is playerSpeed <= walkSpeed completed? =" + (playerSpeed <= walkSpeed));
                if (Input.GetKey(KeyCode.LeftControl) && isGrounded)
                {
                    if (notSliding && playerSpeed <= walkSpeed)
                    {
                        playerSpeed = crouchSpeed;
                        Debug.Log("currently crouching");
                        isCrouched = true;
                    }
                    else if (playerSpeed > walkSpeed || !notSliding)
                    {
                        Debug.Log("currently sliding");
                        notSliding = false;
                        isCrouched = true;
                        //internal bool runs once per slide in order set the initial slide speed to itself plus the slide boost
                        if (crouchInternalBool)
                        {
                            playerSpeed = playerSpeed + slideBoost;
                            crouchInternalBool = false;
                        }
                        //determines slide speed based on the slide boost and decay rate
                        playerSpeed = playerSpeed * slideDecayRate;
                        //
                        Debug.Log("slide speed is " + playerSpeed);
                        controller.Move(transform.forward * playerSpeed * Time.deltaTime);

                        //if the speed is pretty much zero ot if the player lets go of the key, stop sliding
                        if (playerSpeed <= 0.5f || Input.GetKeyUp(KeyCode.LeftControl))
                        {
                            notSliding = true;
                            Debug.Log("completed sliding");
                            isCrouched = true;
                            playerSpeed = crouchSpeed;
                            crouchInternalBool = true;
                        }
                    }
                }
                else if (!Input.GetKey(KeyCode.LeftControl) && isCrouched)
                {
                    notSliding = true;
                    isCrouched = false;
                    Debug.Log("stopped sliding");
                    playerSpeed = walkSpeed;
                }
#endregion
            }
        }
    }
}
