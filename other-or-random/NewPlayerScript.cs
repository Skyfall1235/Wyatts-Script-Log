using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerScript : MonoBehaviour
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
    [SerializeField] private float playerSpeed;
    [SerializeField] private float walkSpeed; 
    [SerializeField] private float sprintRampUp;
    [SerializeField] private float maxSprintSpeed;
    [SerializeField] private float sprintRampDown;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float jumpForce;
    private bool isGrounded;
    private bool isSprinting;
    [SerializeField] private bool isCrouched = false;
    // Start is called before the first frame update
    void Start()
    {
        //lock the cursor to the window and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Get the axis of motion som we can use them 
        playerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        playerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        //run lower level functions of the script
        MovePlayer();
        MovePlayerCamera();
        isGrounded = CheckGrounded();
        playerSpeed = playerBody.velocity.magnitude;

    }
    private void MovePlayer()
    {
        //if the player is grounded and presses the LShift button, toggle sprinting
        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded && playerSpeed < maxSprintSpeed && !isCrouched)
        {
            moveVector = transform.TransformDirection(playerMovementInput) * (walkSpeed * sprintRampUp);
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }

        //if the player stops moving, stop the player sprinting
        if (playerBody.velocity.x == 0 && playerBody.velocity.z == 0)
        {
            isSprinting = false;
        }

        //move the player at the srpint speed if they're sprinting and normally if they aren't
        if (!isSprinting && playerSpeed >= walkSpeed && !isCrouched)
        {
            
        }

        //movement, based on the direction of the camera, based on movementVector
        //is the most basiv movement, and must be calculated lasted
        playerBody.velocity = new Vector3(moveVector.x, playerBody.velocity.y, moveVector.z);
        #region Jumping
        //if the player presses space while grounded, add a force pushing them into the air and stop them sprinting 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                playerBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isSprinting = false;
            }
        }
        #endregion
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