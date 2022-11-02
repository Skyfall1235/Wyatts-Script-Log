using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBullet : MonoBehaviour
{
    public int damagePass;
    float BulletSpeed;
    GameObject Gun;

    int limitZ = 20; //range limits of the bullet on each axis
    int limitX = 20;
     public float projectileLifeSpan;
    public Rigidbody bulletRB;


    private void Start()
    {
        Gun = GameObject.Find("Gun"); // gets the player
        //change this to the enemy when making enemy scripts
        BulletSpeed = Gun.GetComponent<GunSystem>().projectileVelocity; // get speed from player
        bulletRB.AddForce(transform.forward * BulletSpeed, ForceMode.Impulse); // bullet goes forward
    }
    void Update()
    {
        transform.forward = bulletRB.velocity;
        BulletLimits();
    }

    void BulletLimits()  // if bullet goes past the limits, it destroys itself
    {
        if (transform.position.z > limitZ || transform.position.z < -limitZ)
        {
            Destroy(gameObject);
        }
        else if (transform.position.x > limitX || transform.position.x < -limitX)
        {
            Destroy(gameObject);
        }

    }

    void OnTriggerEnter(Collider other) // Destroy both objects when entering the target's collider(remember bullets MUST have rigidbodies)
    {
        Debug.Log("collided with an entity");
        if (other.tag == "Enemy")
        {
            //enemies may have armor, so let the health script handle that
            other.gameObject.GetComponent<HealthSystem>().TakeDamage(damagePass);
        }

        if (other.tag == "DestructableObject")
        {
            //destructable objects dont get armor, but instead get a damage reduction
            other.gameObject.GetComponent<HealthSystem>().TakeDamage(damagePass / 2);
        }
        Destroy(gameObject);
    }

}