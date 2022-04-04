using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using TMPro;
using UnityEngine.UI;

public class PlayerPickUp : MonoBehaviour
{
    [SerializeField] private Camera cam;

    //ui
    [SerializeField] private GameObject pickUpUI;
    [SerializeField] private TextMeshProUGUI pickUpUIText;
    [SerializeField] private Image pistolImage;
    [SerializeField] private Image rifleImage;

    //player
    private Player player;

    // weapon
    [SerializeField] private Transform weaponPistolContainer;
    [SerializeField] private Transform weaponRiffleContainer;
    [SerializeField] private Transform weaponPistolInventory;
    [SerializeField] private Transform weaponRiffleInventory;
    [SerializeField] private RigBuilder rigBuilder;
    [SerializeField] private TwoBoneIKConstraint rightHandIK;
    [SerializeField] private TwoBoneIKConstraint leftHandIk;

    // ammo

    private void Awake()
    {
        player = GetComponent<Player>();
        pistolImage.CrossFadeAlpha(0.5f, 0f, true);
        rifleImage.CrossFadeAlpha(0.5f, 0f, true);
    }

    private void Update()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Weapon") || hit.collider.CompareTag("Ammo"))
            {
                pickUpUI.SetActive(true);
                pickUpUIText.SetText("Press F to pick up " + hit.transform.name);
            }
            else
            {
                pickUpUI.SetActive(false);
            }

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
            if (player.weapons.Contains(item)) return;
            player.weapons.Add(item);
            PickUpWeapon(item);
        }

        else if (item.CompareTag("Ammo"))
        {

        }
    }

    public void PickUpWeapon(Transform weapon)
    {

        if (player.onHand)
        {
            if (player.onHand.name.Equals("Pistol"))
            {
                player.onHand.SetParent(weaponPistolInventory);

            }
            else if (player.onHand.name.Equals("M4_Carbine"))
            {
                player.onHand.SetParent(weaponRiffleInventory);
            }

            player.onHand.localPosition = Vector3.zero;
            player.onHand.localRotation = Quaternion.Euler(Vector3.zero);
            player.onHand.localScale = Vector3.one;

            player.onHand.GetComponent<WeaponController>().enabled = false;
            rigBuilder.enabled = false;
            player.inventory.Add(player.onHand);
        }

        if (weapon)
        {
            if (weapon.name.Equals("Pistol"))
            {
                weapon.SetParent(weaponPistolContainer);
                pistolImage.CrossFadeAlpha(1f, 0f, true);
                rifleImage.CrossFadeAlpha(0.5f, 0f, true);
            }
            else if (weapon.name.Equals("M4_Carbine"))
            {
                weapon.SetParent(weaponRiffleContainer);
                pistolImage.CrossFadeAlpha(0.5f, 0f, true);
                rifleImage.CrossFadeAlpha(1f, 0f, true);
            }

            weapon.localPosition = Vector3.zero;
            weapon.localRotation = Quaternion.Euler(Vector3.zero);
            weapon.localScale = Vector3.one;

            rigBuilder.enabled = true;
            leftHandIk.data.target = weapon.Find("Ref_Left_Hand_Grip");
            rightHandIK.data.target = weapon.Find("Ref_Right_Hand_Grip");
            rigBuilder.Build();

            weapon.GetComponent<Rigidbody>().isKinematic = true;
            weapon.GetComponent<BoxCollider>().isTrigger = true;
            weapon.GetComponent<WeaponController>().enabled = true;

            player.onHand = weapon;
        }
        else
        {
            rigBuilder.enabled = false;
            player.onHand = null;
            pistolImage.CrossFadeAlpha(0.5f, 0f, true);
            rifleImage.CrossFadeAlpha(0.5f, 0f, true);
        }

        if (GameManager.Instance.current.id == 2 && player.onHand.name.Equals("Pistol"))
        {
            GameManager.Instance.MissionDone();
        }

    }


}
