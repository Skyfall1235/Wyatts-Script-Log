using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickUpController : MonoBehaviour
{
    #region variables
    //gun slot 1 & 2
    public GameObject[] gunSlots = new GameObject[2];
    //distance to pick up gun objects
    public float pickUpRange;

   
    
    //equipped (open hand is empty or full)
    public bool itemInHand;
   
    //refernence to player holding the object
    public GameObject player;
    //public 
    Vector3 distanceToPlayer;

    //references


    #endregion
    #region Start, update, and initial Calls
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = player.transform.position - transform.position;
        DropOrPickup();
        
    }
    #endregion
    void Equipped()
    {

    }
    void DropOrPickup()
    {
        //keep keycodes for now, replace later
        if (!itemInHand && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
        }
        //Drop if equipped and "Q" is pressed
        if (itemInHand && Input.GetKeyDown(KeyCode.Q))
        {
            Drop();
        }
    }

    void PickUp()
    {
        //if picked up, turn on gun system, but check if it is equipped to see if it can shoot
    }

    void Drop()
    {
        //if dropped, turn off the gun system
        //throw it
        //turn into not equiped
        // turn off can shoot
        //
    }
    #region internal Actions
    void ThrowObject()
    {

    }

    void TurnOff()
    {

    }

    void TurnOn()
    {

    }
    //if item not equipped, turn off. if it IS equiped, turn on


    //List of actions
    //needs 2 slots, and when not equiped, the other goes into the other spot, on the back or pocket
    //when not equiped, turn off canshoot and gun system components
    //be able to swap between guns
    //pick up within certain radius
    #endregion
}
