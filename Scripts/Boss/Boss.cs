using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public int health;
    public int bulletDamage;
    public int fireRate;
    public int bulletSpeed;
    public int bulletDrop;

    [SerializeField] public GameObject healthBarPanel;
    [SerializeField] public Slider healthBar;


    public Transform player;
    public Transform area;

    private BossPathFinding pathFinding;

    private bool firstTimePlayerInRange = true;

    private void Start()
    {
        pathFinding = transform.parent.GetComponentInChildren<BossPathFinding>();
    }

    private bool PlayerInRange()
    {
        return Vector3.Distance(area.position, player.position) <= 20;
    }

    private void Update()
    {

        healthBar.value = health / 2000f * 100f;

        if (PlayerInRange())
        {
            healthBarPanel.SetActive(true);
            if (firstTimePlayerInRange)
            {
                GetComponent<BossMovement>().enabled = true;
                GetComponent<BossShooting>().enabled = true;
                GameManager.Instance.GetComponent<AudioSource>().Stop();
                GetComponentInParent<AudioSource>().Play();
                pathFinding.SetRandomNode();
                pathFinding.PathFind();
                StartCoroutine(SwitchMoveShootRoutine());
                firstTimePlayerInRange = false;
            }
        }
        else
        {
            healthBarPanel.SetActive(false);
        }
    }

    IEnumerator SwitchMoveShootRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            SwitchMoveShoot();
        }
    }

    private void SwitchMoveShoot()
    {
        if (!BossMovement.moving)
        {
            pathFinding.SetRandomNode();
            pathFinding.PathFind();
        }

        BossMovement.moving = !BossMovement.moving;
        BossShooting.timeToShooting = !BossShooting.timeToShooting;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            GameManager.Instance.PlayerWinScreen();
        }
    }
}
