using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    //health of object
    public int healthValue, minHealthValue, maxHealthValue;
    public bool hasArmor;
    public int armorValue;
    //healing ability of others & self
    public bool ableToHealSelf, canHealOthers = false;
    public int healOtherValue, healOtherRate;
    //conditions of object
    public bool invulnerable, canHaveStatusEffect, isStunned, isBlinded, isPoisoned, isBleeding, isForcedProne, isDown, isRooted, canDie, isDead;
    //internal conditions
    private bool takeDamageInternalBool = false;

    [Header("Object References")]
    //the object this script is attached to
    GameObject ObjectSelf;
    //colliders of the object
    //Collider[]


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (takeDamageInternalBool == true)
        {
            HealthCheck();
            takeDamageInternalBool = false;
        }
        
    }
    void SetHealth()
    {
        //at the beggining, set the health to its maximum
        healthValue = maxHealthValue;
    }
    void HealthCheck()
    {
        if (healthValue > minHealthValue && healthValue <= maxHealthValue)
        {
            isDown = false;
            canHaveStatusEffect = true;
        }
        else if (healthValue <= minHealthValue && canDie == true)
        {
            //is dead can also be used as a kill switch for th whole team if
            isDead = true;
            healthValue = 0;
        }
        else if (healthValue <= minHealthValue)
        {
            isDown = true;
            canHaveStatusEffect = false;
        }
    }
    public void TakeDamage(int damageValue)
    {
        Debug.Log($"taken {damageValue} damage");
        healthValue -= damageValue;
        takeDamageInternalBool = true;
    }

    public void HealSelf(int HealValue)
    {
        Debug.Log($"healed {HealValue} health");
        //could spend resources?
        if (healthValue == maxHealthValue)
        {
            Debug.Log("player is at max HP");
        }
        else if (healthValue < maxHealthValue && ableToHealSelf)
        {
            healthValue += HealValue;
        }
        else
        {
            //display the user cannot be healed
            Debug.Log("player cant heal ");
        }
        if (healthValue > maxHealthValue)
        {
            healthValue = maxHealthValue;
            //tell the player they cant heal
            Debug.Log("player cant heal anymore");
        }
        takeDamageInternalBool = true;
    }
    public void HealOther()
    {

    }
    //heal self
        //will use the heal self function
    //heal others
        //will CALL the heal self function of others
    
    //status effects
}