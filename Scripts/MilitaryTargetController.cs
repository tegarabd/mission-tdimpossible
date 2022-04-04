using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilitaryTargetController : MonoBehaviour
{
    private static int hitCount;
    public static void AddHitCount()
    {
        if (GameManager.Instance.current.id == 3)
        {
            if (hitCount < 10)
            {
                GameManager.Instance.ChangeMissionDisplay(++hitCount);
            }

            if (hitCount >= 10 && !GameManager.Instance.current.done)
            {
                GameManager.Instance.MissionDone();
            }
        }
        
    }

}
