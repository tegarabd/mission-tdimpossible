using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    private Transform player;
    private int health;

    private void Awake()
    {
        health = 100;
        player = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        healthBar.value = health;
        if (health <= 0)
        {
            Dead();
        }
        healthBar.transform.LookAt(player);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    private void Dead()
    {
        GetComponent<Rigidbody>().detectCollisions = false;
        GetComponent<EnemyMovement>().MoveToDead();
        GetComponent<EnemyMovement>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponentInChildren<PathFinding>().enabled = false;

        Invoke("Delete", 1);
    }

    private void Delete()
    {
        Destroy(gameObject);
    }
}
