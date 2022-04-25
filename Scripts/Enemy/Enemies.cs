using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    public int deadCount;
    public int enemiesLeft = 7;

    public void AddDeadCountVillage()
    {

        if (GameManager.Instance.current.id == 5)
        {
            if (deadCount < 16)
            {
                GameManager.Instance.ChangeMissionDisplay(++deadCount);
            }

            if (deadCount >= 16 && !GameManager.Instance.current.done)
            {
                GameManager.Instance.MissionDone();
            }
        }
    }

    public void DownEnemiesLeft()
    {
        if (GameManager.Instance.current.id == 6)
        {
            enemiesLeft--;
            if (enemiesLeft <= 0)
            {
                CountDown.stop = true;
                GameObject.Find("Player").GetComponent<PlayerTeleport>().enabled = true;
            }
        }
    }
}
