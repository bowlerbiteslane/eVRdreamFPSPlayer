using UnityEngine;
using System.Collections;

public class PickupWeapon : MonoBehaviour {

    public Vector3 weaponPositionalOffset;
    public Vector3 weaponRotationalOffset;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            // find players weapon manager
            WeaponManager wm = col.gameObject.GetComponentInChildren<WeaponManager>();
            
            // parent this object beneath the weaponmanager
            this.gameObject.transform.parent = wm.transform;

            // reorient the gun to the specified positional offset relative to the weapon manager
            this.gameObject.transform.localPosition = weaponPositionalOffset;
            this.gameObject.transform.localRotation = Quaternion.Euler(weaponRotationalOffset);

            

            this.gameObject.collider.enabled = false;
            this.gameObject.SetActive(false);
            // add weapon to inventory
            wm.AddWeapon(this.GetComponent<GunBehaviour>());
            

            
        }
    }
}
