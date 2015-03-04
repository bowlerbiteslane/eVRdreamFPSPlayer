using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour {

    public enum WeaponIndex
    {
        Melee = 0,
        Pistol = 1,
        SMG = 2,
        Machinegun = 3
    }

    private WeaponBehaviour[] inventory;
    private int inventorySize = 4;
    private GameObject currentWeapon;
    private WeaponBehaviour currentWeaponBehaviour;

    public GameObject CurrentWeapon
    {
        get
        {
            return currentWeapon;
        }
    }

    public WeaponBehaviour CurrentWeaponBehaviour
    {
        get
        {
            return currentWeaponBehaviour;
        }
    }



    void Awake()
    {

        // populate player inventory with start weapons
        inventory = new WeaponBehaviour[inventorySize];
        WeaponBehaviour[] weaponsToPopulate = GetComponentsInChildren<WeaponBehaviour>();
        for (int i = 0; i < weaponsToPopulate.Length; i++)
        {
            AddWeapon(weaponsToPopulate[i]);
            weaponsToPopulate[i].gameObject.SetActive(false);
        }

        //set current weapon to first populated index in inventory
        for (int i = 0; i < inventorySize; i++)
        {
            Debug.Log("Inventory Iteration: " + i);
            if (inventory[i] != null)
            {
                currentWeapon = inventory[i].gameObject;
                currentWeaponBehaviour = inventory[i];
                currentWeapon.SetActive(true);
                break;
            }
        }

        if (currentWeapon == null)
        {
            Debug.Log("Weapon Manager inventory is empty.");
        }
        
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Alpha1))
            ChangeWeapon((int)WeaponIndex.Pistol);
        else if (Input.GetKey(KeyCode.Alpha2))
            ChangeWeapon((int)WeaponIndex.SMG);
        else if (Input.GetKey(KeyCode.Alpha3))
            ChangeWeapon((int)WeaponIndex.Machinegun);
        else if (Input.GetKey(KeyCode.Alpha4))
            ChangeWeapon((int)WeaponIndex.Melee);
    }

    void ChangeWeapon(int index)
    {
        if (!(index > inventory.Length) && inventory[index] != null && inventory.Length >= 1)
        {
            currentWeapon.SetActive(false);
            currentWeapon = inventory[index].gameObject;
            currentWeapon.SetActive(true);
            currentWeaponBehaviour = currentWeapon.GetComponent<WeaponBehaviour>();
            //if (currentWeaponBehaviour is GunBehaviour)
                //Debug.Log("Ammo In Clip: " + ((GunBehaviour)currentWeaponBehaviour).AmmoInClip + " Clips: " + (((GunBehaviour)currentWeaponBehaviour).CurrentClips-1) + " Total Ammo: " + (((GunBehaviour)currentWeaponBehaviour).AmmoInClip + (((GunBehaviour)currentWeaponBehaviour).ClipSize * (((GunBehaviour)currentWeaponBehaviour).CurrentClips-1))));
        }
        else
        {
            Debug.Log("No weapon in that weapon slot.");
        }
        
    }

    public void AddWeapon(WeaponBehaviour weapon)
    {
        int wIndex = weapon.WeaponIndex;
        // Check if the weapons index is within the inventory bounds
        if (wIndex < inventorySize)
        {
            // check if inventory is empty so that we can later set the current weapon to this one
            bool inventoryEmpty = true;
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] != null || inventory.Length == 0)
                {
                    inventoryEmpty = false;
                }
                Debug.Log("Inventory is Empty.");
            }

            // add the current weapon
            if (inventory[wIndex] == null)
            {
                inventory[wIndex] = weapon;
            }
            else
            {
                DropWeapon(inventory[wIndex].GetComponent<WeaponBehaviour>());
                inventory[weapon.WeaponIndex] = weapon;
            }
            Debug.Log(weapon.gameObject + " added to weaponmanager at index = " + weapon.WeaponIndex);

            
            // if there is only one weapon in inventory then make it the current weapon
            if (inventoryEmpty)
            {
                currentWeapon = inventory[wIndex].gameObject;
                currentWeapon.SetActive(true);
                currentWeaponBehaviour = currentWeapon.GetComponent<WeaponBehaviour>();
                Debug.Log("Set the new current weapon to first weapon picked up.");
            }
        }
        else
        {
            Debug.Log("Weapon index exceeds inventory size. Please change weapon index to a value below " + inventorySize + ".");
        }
        
    }

    void DropWeapon(WeaponBehaviour weapon)
    {
        Debug.Log(inventory[weapon.WeaponIndex].gameObject + " being dropped by weaponmanager at index = " + inventory[weapon.WeaponIndex]);
        inventory[weapon.WeaponIndex] = null;
        //currently this script just empties a place for any added weapons with the same index as one already in inventory
    }


}
