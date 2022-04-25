using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<Mission> missions;
    public Mission current;
    public int totalMissionDone;
    public Transform player;
    public Transform cam;

    public GameObject winScreen;
    public GameObject deadScreen;

    public RawImage winScreenBackground;
    public RawImage deadScreenBackground;

    public bool isReceiveInput;

    // Mission Text
    [SerializeField] private TMPro.TextMeshProUGUI missionDisplay;

    private void Awake()
    {
        Instance = this;
        missions = new List<Mission>();
        isReceiveInput = true;
        deadScreenBackground.CrossFadeAlpha(0f, 0f, true);
        winScreenBackground.CrossFadeAlpha(0f, 0f, true);

        /*totalMissionDone = 5;*/

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
        text = text.Remove(text.IndexOf("(") + 1, 1);
        text = text.Insert(text.IndexOf("(") + 1, count.ToString());
        missionDisplay.SetText(text);
    }

    public void PlayerDeadScreen()
    {
        deadScreen.SetActive(true);
        deadScreenBackground.CrossFadeAlpha(1f, 2f, true);
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void PlayerWinScreen()
    {
        winScreen.SetActive(true);
        winScreenBackground.CrossFadeAlpha(1f, 2f, true);
        Cursor.lockState = CursorLockMode.Confined;
    }
}
