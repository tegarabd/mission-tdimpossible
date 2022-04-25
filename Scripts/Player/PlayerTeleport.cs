using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerTeleport : MonoBehaviour
{
    [SerializeField] private GameObject panelUI;
    [SerializeField] private TextMeshProUGUI panelUIText;
    [SerializeField] private LayerMask teleportLayer;

    void Update()
    {
        if (Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), 2f, teleportLayer))
        {
            panelUI.SetActive(true);
            panelUIText.SetText("Press F to teleport");

            if (Input.GetKeyDown(KeyCode.F))
            {
                StartCoroutine(Teleport());
            }
        }
        else
        {
            panelUI.SetActive(false);
        }
    }

    private IEnumerator Teleport()
    {
        Debug.Log(Dungeon.initialPos);
        GetComponent<Player>().health = 100;
        GetComponent<CharacterController>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        transform.position = Dungeon.initialPos;
        yield return new WaitForSeconds(0.1f);
        GetComponent<CharacterController>().enabled = true;
        GetComponent<PlayerMovement>().enabled = true;
    }
}
