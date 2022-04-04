using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<Mission> missions;
    public Mission current;
    public int totalMissionDone;

    // Mission Text
    [SerializeField] private TextMeshProUGUI missionDisplay;

    private void Awake()
    {
        Instance = this;
        missions = new List<Mission>();

        missions.Add(new Mission(1, "Find 'Asuna' and Talk to her"));
        missions.Add(new Mission(2, "Pick up the pistol"));
        missions.Add(new Mission(3, "Shoot 10 Rounds at the shooting target! (0/10)"));
        missions.Add(new Mission(4, "Shoot 50 Bullets with the rifle! (0/50)"));
        missions.Add(new Mission(5, "Eliminate the soldiers that are attacking the village! (0/16)"));
        missions.Add(new Mission(6, "Head to the secret teleport room and defeat the boss"));

        StartCurrentMission();
    }
 

    public void StartCurrentMission()
    {
        current = missions[totalMissionDone];
        missionDisplay.SetText(current.description);
        missionDisplay.color = Color.yellow;
    }
    public void MissionDone()
    {
        missionDisplay.SetText(missionDisplay.text + "\n[Done]");
        missionDisplay.color = Color.green;
        current.done = true;
        totalMissionDone++;
    }

    public void ChangeMissionDisplay(int count)
    {
        string text = current.description;
        text = text.Remove(text.IndexOf("(") + 1, (count > 10) ? 2 : 1);
        text = text.Insert(text.IndexOf("(") + 1, count.ToString());
        missionDisplay.SetText(text);
    }

}
