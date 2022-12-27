using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBullet : MonoBehaviour
{
    float BulletSpeed;
    public GameObject Gun;
    //damage variables
    int damagePass;
    public int damageRaw;

    int limitZ = 30; //range limits of the bullet on each axis
    int limitX = 30;

    public Collider[] destructionColliders; //Array of colliders
    public int destroyRadius = 10;
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
        if (transform.position.z > limitZ)
        {
            Destroy(gameObject);
        }
        else if (transform.position.z < -limitZ)
        {
            Destroy(gameObject);
        }
        else if (transform.position.x > limitX)
        {
            Destroy(gameObject);
        }
        else if (transform.position.x < -limitX)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other) // Destroy both objects when entering the target's collider(remember bullets MUST have rigidbodies)
    {
        destructionColliders = Physics.OverlapSphere(gameObject.transform.position, destroyRadius); // Gets the colliders of other objects inside a radius

        //Animation Goes Here!

        foreach (Collider collider in destructionColliders)
        {
            if (collider.gameObject.tag == "Enemy" || collider.gameObject.tag == "DestructableObject")
            {
                collider.gameObject.GetComponent<HealthSystem>().TakeDamage(damagePass);
                Debug.Log($"did {damagePass} damage to {collider}");
            }
        }
    }
    /*    void onCollisonEnter(Collider other) // Destroy both objects when entering the target's collider(remember bullets MUST have rigidbodies)
    {
        if (other.CompareTag("Enemy"))
        {
            //enemies may have armor, so let the health script handle that
            other.gameObject.GetComponent<HealthSystem>().TakeDamage(damagePass);
        }

        if (other.CompareTag("DestructableObject"))
        {
            //destructable objects dont get armor, but instead get a damage reduction
            other.gameObject.GetComponent<HealthSystem>().TakeDamage(damagePass / 2);
        }

        Destroy(gameObject);
        Debug.Log("collided with an entity");
    }*/
}