using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Asuna : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameDisplay;
    [SerializeField] private LayerMask layerPlayer;
    [SerializeField] private GameObject pickUpUI;
    [SerializeField] private TextMeshProUGUI pickUpUIText;

    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private TextMeshProUGUI dialogueText;
    private Transform player;

    [SerializeField] Animator doorLeft;
    [SerializeField] Animator doorRight;

    [SerializeField] Animator doorLeftTeleport;
    [SerializeField] Animator doorRightTeleport;

    [SerializeField] List<GameObject> scenes;

    private bool isInterracting;
    private bool firstTime;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
    }
    private void Update()
    {
        nameDisplay.transform.LookAt(player);
        nameDisplay.transform.Rotate(0f, 180f, 0f);

        if (Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), 2f, layerPlayer))
        {
            pickUpUI.SetActive(true);
            pickUpUIText.SetText("Press F to Interract");
            player.GetComponent<PlayerPickUp>().PickUpWeapon(null);

            if (Input.GetKeyDown(KeyCode.F))
            {
                Interract();
            }
        }
        else
        {
            pickUpUI.SetActive(false);
        }

        if (isInterracting)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                dialogueUI.SetActive(false);
                if (GameManager.Instance.current.id == 4)
                {
                    OpenGate();
                }
                if (GameManager.Instance.current.id == 5)
                {
                    OpenGateTeleport();
                }
                if (firstTime)
                {
                    scenes[GameManager.Instance.current.id].SetActive(true);
                    firstTime = false;
                }
                GameManager.Instance.StartCurrentMission();
                isInterracting = false;
                GameManager.Instance.isReceiveInput = true;
            }
        }
        
    }

    private void OpenGateTeleport()
    {
        doorLeftTeleport.Play("door_left_open");
        doorRightTeleport.Play("door_right_open");
    }
    private void OpenGate()
    {
        doorLeft.Play("door_left_open");
        doorRight.Play("door_right_open");
    }

    private void Interract()
    {
        dialogueUI.SetActive(true);
        isInterracting = true;
        GameManager.Instance.isReceiveInput = false;

        if (!GameManager.Instance.current.done && GameManager.Instance.current.id != 1)
        {
            dialogueText.SetText("You have not finished your current mission, finish it first!");
        } 
        else
        {
            player.GetComponent<Player>().totalPistolAmmo += 7;
            player.GetComponent<Player>().totalRifleAmmo += 30;

            switch (GameManager.Instance.current.id)
            {
                case 1:
                    if (!GameManager.Instance.current.done) GameManager.Instance.MissionDone();
                    dialogueText.SetText("Take the Pistol!");
                    break;
                case 2:
                    dialogueText.SetText("It seems like you are not familiar with it, try to shoot the target!");
                    break;
                case 3:
                    dialogueText.SetText("Take the Rifle and shoot 50 times");
                    break;
                case 4:
                    dialogueText.SetText("Now go Through passage and kill enemies");
                    break;
                case 5:
                    dialogueText.SetText("Head to the teleport room and kill the boss");
                    break;
            }
            firstTime = true;
        }

    }
}
