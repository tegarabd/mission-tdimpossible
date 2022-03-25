using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Mission Text
    [SerializeField] private TextMeshProUGUI missionDisplay;

    // Training Stage
    private int militaryTargetHitCount;
    private int rifleShootCount;

    private void Awake()
    {
        Instance = this;
    }

    // Training Stage Method
    public void addMilitaryTargetHitCount()
    {
        militaryTargetHitCount++;

        if (militaryTargetHitCount == 10)
        {
            missionDisplay.color = Color.green;
            return;
        }

        missionDisplay.color = Color.yellow;
        missionDisplay.SetText("Hit the training target\n" +
            militaryTargetHitCount + "/10");

    }


}
