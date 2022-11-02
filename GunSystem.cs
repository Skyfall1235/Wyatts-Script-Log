using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunSystem : MonoBehaviour
{
    #region Variables
    [Header("Gun fire Rate info")]
    public bool isFullAuto;
    private bool isSemiAuto;

    [Header("Bullet Forces")]
    //bullet force
    public float shootForce;
    public float recoilForce;
    //Recoil target
    public Rigidbody playerRb;


    [Header("Bullet projectile Varibles")]
    //projectile data ( how far it goes)
    public float projectileRange;
    public float projectileVelocity;
    public int rawDamage;
    int damagePass;
    public GameObject[] projectilePrefabs; //0 is normal bullet; 1 is Shotgun Bullet; 2 is Exploding Bullet
    [Tooltip("0 is a normal bullet; 1 is Shotgun Bullet; 2 is Exploding Bullet")]
    public int bulletMode;


    [Header("Gun Information")]
    //work in progress variables that will be used to determine rate of fire
    public float fireRatePerMinute;
    private float fireRatePerSecond;

    [SerializeField]
    private float fireRateTimer;
    //ammo  and reload information
    public int ammoInMagazine, totalAmmoLeft, ammoFull, magazineSize;
    private int reloadAmount;
    

    //Gun stats
    public float spread, reloadTime;

    //bools for allowing the gun to be used
    public bool canShoot, isRaycast, allowShooting;
    bool reloading;
    bool justFired = false;

    [Space(1)]

    [Header("Object References")]
    //Reference
    public Transform attackPoint;

    public GameObject muzzleFlash;

    //ammo display
    public TextMeshProUGUI ammunitionDisplay;
    public TextMeshProUGUI magazineDisplay;

    //From Seperate GameObject

    //bug fixing :D
    public bool allowInvoke = true;
    #endregion
    #region Start, Update, and Initial Calls

    // Start is called before the first frame update
    void Start()
    {
        //calls the setup function once to begin theability to shoot
        InitalStartCalls();
    }
    // Update is called once per frame
    void Update()
    {
        //you could probably only do these after the reload function is complete to not run the damn command every frame

        //shoot exists to tell us that the shoot function does in fact work

        //sets the ammo display to be whatever it currently is. Could possibly only run this function after every time Shoot() is called
        //AmmoDisplay();

        ShootCall();
    }
    void InitalStartCalls()
    {
        totalAmmoLeft = ammoFull;
        fireRatePerSecond = 1 / (fireRatePerMinute / 60);
        fireRateTimer = 0;

        //if ()
        //{
        //    projectileCanBurst = false;
        //}
    }
    #endregion
    #region Gun Functions
    void ShootCall()
    {
        if (isFullAuto == false)
        {
            isSemiAuto = true;
            SingleShot();
        }
        else
        {
            isSemiAuto = false;
            FullAuto();
        }
    }
    //move ammo display to a UI script
    void AmmoDisplay()
    {
        ammunitionDisplay.SetText($"{ammoInMagazine} / {magazineSize}");
        magazineDisplay.SetText($"{totalAmmoLeft}");
    }
    void Reload()
    {
        //SHOULD ONLY BE ABLE TO BE CALLED IF THE PLAYER HAS ammoInMagazine < magazineSize
        //play the animation
        canShoot = false;
        //display "0" + "/" + totalAmmoLeft

        //setting the reload amount to whatever you CAN add to the gun magazine
        reloadAmount = magazineSize - ammoInMagazine;
        //subtracting the ammo removed from the magazine and placed into the gun
        totalAmmoLeft -= reloadAmount;

        if (reloadAmount < totalAmmoLeft)
        {
            //subtracts the reload amount from the totalAmmoLeft
            totalAmmoLeft -= reloadAmount;
        }
        else if (reloadAmount >= totalAmmoLeft)
        {
            reloadAmount = totalAmmoLeft;
        }
        //somehow make it display that the magazine has 0 ammo in it

        //refills the ammo in magazines to as much as it can
        ammoInMagazine += reloadAmount;
        ReloadFinished();
    }
    void ReloadFinished()
    {
        //display the proper amount of ammo left
        //displayMagazine and displayAmmo or whatever

        //animation to send the bolt of the gun to go forward

        //allow the gun to shoot again
        canShoot = true;
    }
    void SingleShot()
    {
        fireRateTimer -= Time.deltaTime;
        

        if (fireRateTimer < -1)
        {
            fireRateTimer = -1;
        }
        if (Input.GetButtonDown("Fire1") && isSemiAuto)
        {
            Debug.Log("Gun is supposed to be shooting!");
            if (fireRateTimer <= 0f)
            {
                Debug.Log("fire rate cycled");
                Shoot();
                justFired = true;
            }
        }
        if (Input.GetButtonUp("Fire1") && justFired)
        {
            fireRateTimer = fireRatePerSecond;
            justFired = false;
        }
    }
    //this method should fire 1 bullet every x amount of seconds
    void FullAuto()
    {
        fireRateTimer -= Time.deltaTime;
        if (fireRateTimer < -1)
        {
            fireRateTimer = 0;
        }
        if (Input.GetButton("Fire1") && isFullAuto)
        {
            if (fireRateTimer <= 0f)
            {
                Debug.Log("fire rate cycled");
                Shoot();
                fireRateTimer = fireRatePerSecond;
            }
        }
    }
    void ThreeRoundBurst()
    {
        
    }
    void Shoot()
    {
        Debug.Log("Shoot Function Called");
        if (ammoInMagazine >= 1 && reloading != true && allowShooting == true)
        {
            canShoot = true;
            Debug.Log("there is ammo, Gun is not reloading, and is allowed to shoot");
        }
        else
        {
            canShoot = false;
            Debug.Log("Ammo is out, or the gun is not allowed to shoot");
        }

        //shoot ray to detect wtf its hitting. if it hits player or friendly it shouldnt give them damage
        //if the player can shoot, and the gun is a raycast gun, do a hitscan shot
        if (isRaycast == true && canShoot == true)
        {
            //RaycastHit hit;
            Vector3 fwd = transform.TransformDirection(Vector3.forward);

            if (Physics.Raycast(attackPoint.transform.position, fwd, projectileRange))
            {
                //just for test, should apply the takedamage method
                print("There is something in front of the object!");
            }
            ammoInMagazine--;
        }
        //if the gun can shoot and is NOT a raycast, fire a projectile
        else if (isRaycast == false && canShoot == true)
        {
            //create bullet at the target point
            switch (bulletMode)
            {
                case (0):
                    NormalBullet();
                    break;
                case (1):
                    ShotgunBullet();
                    break;
                case(2):
                    ExplosiveBullet();
                    break;
                default:
                    break;
            }
            ammoInMagazine--;;
        }
        else if (isRaycast == false && canShoot == false)
        {
            Debug.Log("CanShoot is false");
        }
    }
    #endregion
    #region bullet types
    void NormalBullet() //Spawns normal bullet
    {
        Instantiate(projectilePrefabs[0], attackPoint.position, attackPoint.rotation);
    }

    void ShotgunBullet() //Spawns 3 bullets at diferrent rotations (can add more if desired)
    {
        int Offset = 10;
        Instantiate(projectilePrefabs[1], attackPoint.position, attackPoint.rotation * Quaternion.Euler(0, Offset, 0));
        Instantiate(projectilePrefabs[1], attackPoint.position, attackPoint.rotation * Quaternion.Euler(0, -Offset, 0));
        Instantiate(projectilePrefabs[1], attackPoint.position, attackPoint.rotation * Quaternion.Euler(0, 0, 0));
    }

    void ExplosiveBullet() //Spawns Explosive Bullet
    {
        Instantiate(projectilePrefabs[2], attackPoint.position, attackPoint.rotation);
    }
    #endregion
}