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

    private bool isInterracting;

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
                GameManager.Instance.StartCurrentMission();
                isInterracting = false;
            }
        }
    }

    private void Interract()
    {
        dialogueUI.SetActive(true);
        isInterracting = true;

        if (!GameManager.Instance.current.done && GameManager.Instance.current.id != 1)
        {
            dialogueText.SetText("You have not finished your current mission, finish it first!");
        } 
        else
        {
            switch (GameManager.Instance.current.id)
            {
                case 1:
                    GameManager.Instance.MissionDone();
                    dialogueText.SetText("Take the Pistol!");
                    break;
                case 2:
                    dialogueText.SetText("It seems like you are not familiar with it, try to shoot the target!");
                    break;
                case 3:
                    dialogueText.SetText("Take the Rifle and shoot 50 times");
                    break;
            }
        }

        
    }
}
