using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private List<GameObject> ammos;
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
        healthBar.transform.LookAt(player);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            StartCoroutine(Dead());
        }
    }

    private IEnumerator Dead()
    {
        if (Random.value > 0.5f)
        {
            Instantiate(ammos[Mathf.RoundToInt(Random.Range(0, 2))], transform.position, Quaternion.identity);
        }

        transform.parent.GetComponentInChildren<PathFinding>().enabled = false;
        transform.parent.GetComponentInChildren<Grid>().enabled = false;
        GetComponent<Rigidbody>().detectCollisions = false;
        GetComponent<EnemyMovement>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;

        transform.parent.parent.GetComponent<Enemies>().AddDeadCountVillage();
        transform.parent.parent.GetComponent<Enemies>().DownEnemiesLeft();
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
