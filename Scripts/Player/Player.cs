using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Transform onHand;
    public List<Transform> inventory;
    public List<Transform> weapons;

    public Slider healthBar;

    private PlayerPickUp PlayerPickUp;

    public int health;

    public int totalPistolAmmo;
    public int totalRifleAmmo;
    private void Awake()
    {
        weapons = new List<Transform>();
        inventory = new List<Transform>();
        PlayerPickUp = GetComponent<PlayerPickUp>();
        health = 100;
    }

    private void Update()
    {
        healthBar.value = health;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Transform pistol = inventory.Find(item => item.name.Equals("Pistol"));
            if (pistol)
            {
                PlayerPickUp.PickUpWeapon(pistol);
                inventory.Remove(pistol);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Transform rifle = inventory.Find(item => item.name.Equals("Rifle"));
            if (rifle)
            {
                PlayerPickUp.PickUpWeapon(rifle);
                inventory.Remove(rifle);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayerPickUp.PickUpWeapon(null);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            GameManager.Instance.PlayerDeadScreen();
        }
    }
}
