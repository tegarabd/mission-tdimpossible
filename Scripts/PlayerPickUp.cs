using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerPickUp : MonoBehaviour
{
    [SerializeField] private Camera cam;

    // weapon
    [SerializeField] private Transform weaponPistolContainer;
    [SerializeField] private Transform weaponRiffleContainer;
    [SerializeField] private RigBuilder rigBuilder;
    [SerializeField] private TwoBoneIKConstraint rightHandIK;
    [SerializeField] private TwoBoneIKConstraint leftHandIk;

    // ammo

    private void Update()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                PickUp(hit.transform);
            }
        }
    }

    private void PickUp(Transform item)
    {
        if (item.CompareTag("Weapon"))
        {
            PickUpWeapon(item);
        }

        else if (item.CompareTag("Ammo"))
        {

        }
    }

    private void PickUpWeapon(Transform weapon)
    {
        if (weapon.name.Equals("Pistol")) weapon.SetParent(weaponPistolContainer);
        else if (weapon.name.Equals("M4_Carbine")) weapon.SetParent(weaponRiffleContainer);
        weapon.localPosition = Vector3.zero;
        weapon.localRotation = Quaternion.Euler(Vector3.zero);
        weapon.localScale = Vector3.one;

        rigBuilder.enabled = true;
        leftHandIk.data.target = weapon.Find("Ref_Left_Hand_Grip");
        rightHandIK.data.target = weapon.Find("Ref_Right_Hand_Grip");
        rigBuilder.Build();

        weapon.GetComponent<Rigidbody>().isKinematic = true;
        weapon.GetComponent<BoxCollider>().isTrigger = true;
        weapon.GetComponent<WeaponManager>().enabled = true;
    }
}
