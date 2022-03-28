using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    private int health;

    private void Awake()
    {
        health = 100;
    }

    private void Update()
    {
        healthBar.value = health;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
