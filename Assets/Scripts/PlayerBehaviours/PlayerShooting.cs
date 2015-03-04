using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour {
    Camera playerCamera;

    private WeaponManager weaponManager = null;

    public float shotOffsetModifer = 0.05f;

    public GameObject bulletImpactGeneric = null;
    public GameObject bulletImpactWood = null;
    public GameObject bulletImpactConcrete = null;
    public GameObject bulletImpactDirt = null;
    public GameObject bulletImpactMetal = null;
    public GameObject bulletImpactGlass = null;
    public GameObject bulletImpactEnemy = null;
    public int impactCount = 100;

    public LayerMask rayLayerMask = 1 << 2 << 8;                 // need to create a layer mask to ensure that the ray does not hit the player - specify here the 
    
    public bool SCRIPTDEBUG = true;                         //  creates an additional ray to represent the path of the shoot ray - disable to improve performance

    private float nextFire = 0.0F;

    private GameObject[] impacts;
    private int impactIndex = 0;


    void Awake()
    {
        weaponManager = GetComponentInChildren<WeaponManager>();
        rayLayerMask = ~rayLayerMask;                       // invert layer mask, otherwise the ray would only hit the player
        playerCamera = Camera.main;
        impacts = new GameObject[impactCount];
        if (bulletImpactGeneric == null)
        {
            Debug.Log("PlayerShooting script does not have a valid generic bulletImpact texture attached. Disabling script.");
            this.enabled = false;
        }
        if (weaponManager == null)
        {
            Debug.Log("PlayerShooting script does not have a valid weapon manager gameobject attached. Disabling script.");
            this.enabled = false;
        }


    }
	// Update is called once per frame
	void Update () 
    {
       
        if(SCRIPTDEBUG)
            ScriptDebug();
        nextFire += Time.deltaTime;
        WeaponBehaviour wb = weaponManager.CurrentWeaponBehaviour;
        // on player fire
        if (wb != null && Input.GetButton("Fire1") && nextFire >= wb.FireRate)                                                                                                   // check if the player pulls the trigger and the gun is within it's firing rate
        {
            if (wb.TriggerAttack()) // returns true if the player is not reloading or not out of ammo
            { 

                //calculate shot inaccuracy
                float currentWeaponAccuracy = weaponManager.CurrentWeaponBehaviour.WeaponAccuracy;
                float shotOffsetX = Random.Range(-shotOffsetModifer, shotOffsetModifer) * (1 - currentWeaponAccuracy);
                float shotOffsetY = Random.Range(-shotOffsetModifer, shotOffsetModifer) * (1 - currentWeaponAccuracy);
                //shotOffsetX = .1f;
                //shotOffsetY = .1f;
                Vector3 shotOffset = new Vector3(shotOffsetX, shotOffsetY, 0f);


                // send raycast
                RaycastHit rayHit;
                if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward + shotOffset, out rayHit, weaponManager.CurrentWeaponBehaviour.WeaponReach, rayLayerMask))                       // check if raycast was successful and get the location of the hit
                {
                    // increase impact array index
                    impactIndex++;
                    if (impactIndex > impacts.Length - 1)
                    {                                                                                                         // check if impactCnt has reached the max value of the arry
                        impactIndex = 0;
                    }
                    if (impacts[impactIndex] != null)
                    {                                                                                                         //  if there is currently an impact in this spot, destroy it
                        Destroy(impacts[impactIndex]);
                    }
                    // adding the (rayHit.normal/100) makes it so that the impact will show up over top of the object                                                                    
                    // check for impact material type
                    GameObject spawnImpact;

                    switch (rayHit.transform.gameObject.tag)
                    {
                        case "Enemy":
                            spawnImpact = bulletImpactEnemy;
                            break;
                        case "Wood":
                            spawnImpact = bulletImpactWood;
                            break;
                        case "Concrete":
                            spawnImpact = bulletImpactConcrete;
                            break;
                        case "Dirt":
                            spawnImpact = bulletImpactDirt;
                            break;
                        case "Metal":
                            spawnImpact = bulletImpactMetal;
                            break;
                        case "Glass":
                            spawnImpact = bulletImpactGlass;
                            break;
                        default:
                            spawnImpact = bulletImpactGeneric;
                            break;
                    }

                    if (spawnImpact == null)
                        spawnImpact = bulletImpactGeneric;
                    Quaternion bulletRotation = Quaternion.FromToRotation(Vector3.up, rayHit.normal);
                    Vector3 bulletPosition = rayHit.point + (rayHit.normal / 100);

                    impacts[impactIndex] = Instantiate(spawnImpact, bulletPosition, bulletRotation) as GameObject;
                    impacts[impactIndex].transform.parent = rayHit.transform;                                                                                              // parents the impact decal under the object that it has hit


                    // handle action events after creating impact
                    if (rayHit.transform.gameObject.tag == "Enemy")
                    {
                        if (rayHit.transform.gameObject.GetComponent<HealthManager>() != null)
                            rayHit.transform.gameObject.GetComponent<HealthManager>().TakeDamage(weaponManager.CurrentWeapon.GetComponent<WeaponBehaviour>().WeaponDamage);
                        else
                            Debug.Log("Game Object: " + rayHit.transform.gameObject + " is tagged enemy, but does not have a HealthManager Script");
                    }
                }
            }

            nextFire = 0f;
        }
    }

    void ScriptDebug()
    {
        RaycastHit rayHit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out rayHit, weaponManager.CurrentWeapon.GetComponent<WeaponBehaviour>().WeaponReach, rayLayerMask))                       // check if raycast was successful and get the location of the hit
        {
            Debug.DrawLine(playerCamera.transform.position, rayHit.point, Color.red);                                                                                                               // draw line in scene view to represent the ray
            Debug.Log("{Hit Object: [" + rayHit.transform.gameObject + "]} {Hit Point: [" + rayHit.point + "]} {Distance: [" + rayHit.distance + "]}");
        }
    }
}
