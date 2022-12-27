using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    private Vector3 playerMovementInput;
    private Vector2 playerMouseInput;
    private float currentXRotation;

    private Vector3 moveVector;
    [SerializeField] private LayerMask floorMask;
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Rigidbody playerBody;
    [Space]
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float maxSprintSpeed;
    [SerializeField] private float sprintRampUp;
    [SerializeField] private float sprintRampDown;
    [SerializeField] private float slideBoost;
    [SerializeField] private float slideDecayRate;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float jumpForce;
    private bool isGrounded;
    [SerializeField] private bool isCrouched = false;
    [SerializeField] private bool isSprinting = false;
    [SerializeField] bool notSliding = true;
    bool crouchInternalBool = true;

    //ONLY TO LOOK AT, DO NOT EDIT
    [SerializeField] float refPlayerSpeed;

    void Start()
    {
        //lock the cursor to the window and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        moveVector = transform.TransformDirection(playerMovementInput) * walkSpeed;
    }
    void Update()
    {
        initialisingCommands();
    }
    void initialisingCommands()
    {
        //Get the axis of motion som we can use them 
        playerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        playerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        //run lower level functions of the script
        MovePlayer();
        MovePlayerCamera();
        isGrounded = CheckGrounded();
        refPlayerSpeed = playerBody.velocity.magnitude;
    }
    private void MovePlayer()
    {
        #region sprinting and walking
        //allows the chracter to walk
        moveVector = transform.TransformDirection(playerMovementInput) * (walkSpeed);
        if (Input.GetKey(KeyCode.LeftShift) && isGrounded && refPlayerSpeed >= walkSpeed && !isCrouched)
        {
            Debug.Log("is trying to sprint");
            //ramps the sprint speed up over time
            moveVector = transform.TransformDirection(playerMovementInput) * (refPlayerSpeed * sprintRampUp);
            isSprinting = true;

            if (refPlayerSpeed >= maxSprintSpeed)
            {
                Debug.Log("is at max speed");
                moveVector = transform.TransformDirection(playerMovementInput) * maxSprintSpeed;
            }
        }
        //is no longer sprinting and must ramp down
        else
        {
            if (isSprinting && refPlayerSpeed >= walkSpeed && !isCrouched)
            {
                moveVector = transform.TransformDirection(playerMovementInput) * (refPlayerSpeed * sprintRampDown);
            }
            else if (isSprinting && refPlayerSpeed <= walkSpeed && !isCrouched)
            {
                isSprinting = false;
                Debug.Log("no longer sprinting and speed is" + refPlayerSpeed);
            }
            else if (!isSprinting && refPlayerSpeed <= walkSpeed /*&& notSliding*/)
            {
                moveVector = transform.TransformDirection(playerMovementInput) * (walkSpeed);
            }
        }
        #endregion

        #region Crouching and Sliding
        if (Input.GetKey(KeyCode.LeftControl) && isGrounded)
        {
            if (notSliding && refPlayerSpeed <= walkSpeed)
            {
                moveVector = transform.TransformDirection(playerMovementInput) * crouchSpeed;
                Debug.Log("currently crouching");
                isCrouched = true;
            }
            else if (refPlayerSpeed > walkSpeed || notSliding == false)
            {
                Debug.Log("currently sliding");
                notSliding = false;
                isCrouched = true;

                //internal bool runs once per slide in order set the initial slide speed to itself plus the slide boost
                if (crouchInternalBool)
                {
                    moveVector = transform.TransformDirection(playerMovementInput) * (refPlayerSpeed + slideBoost);
                    crouchInternalBool = false;
                }
                moveVector = transform.TransformDirection(playerMovementInput) * (refPlayerSpeed * slideDecayRate);
                Debug.Log("slide speed is " + refPlayerSpeed);

                //if the player lets up on the crouch key OR his slide speed gets to low, he just crouches and stops sliding
                if (refPlayerSpeed <= 0.5f || Input.GetKeyUp(KeyCode.LeftControl))
                {
                    notSliding = true;
                    Debug.Log("completed sliding");
                    isCrouched = true;
                    moveVector = transform.TransformDirection(playerMovementInput) * crouchSpeed;
                    crouchInternalBool = true;
                }
            }
        }
        //if for any reason, the crouch key is let up but the player is still crouching/sliding, fix it
        else if (!Input.GetKey(KeyCode.LeftControl) && isCrouched)
        {
            notSliding = true;
            isCrouched = false;
            Debug.Log("stopped sliding");
            moveVector = transform.TransformDirection(playerMovementInput) * walkSpeed;
        }
        #endregion

        //the movement vector is just the speed in that given direction
        playerBody.velocity = new Vector3(moveVector.x, playerBody.velocity.y, moveVector.z);
    }

    private void MovePlayerCamera()
    {
        //set the value containing the players current x rotation to the current x rotation - the mouse input and clamp this rotation between 90 and -90 
        currentXRotation -= playerMouseInput.y * mouseSensitivity;
        currentXRotation = Mathf.Clamp(currentXRotation, -90f, 90f);
        //rotate the player by the amount the mouse has moved and rotate the camera locally upward or downward based to match current x rotation
        transform.Rotate(0f, playerMouseInput.x * mouseSensitivity, 0f);
        playerCamera.transform.localRotation = Quaternion.Euler(currentXRotation, 0f, 0f);
    }
    private bool CheckGrounded()
    {
        //if the ground check sphere sees an object with the ground tag, return true, else return false
        if (Physics.CheckSphere(groundCheckTransform.position, 0.1f, floorMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
