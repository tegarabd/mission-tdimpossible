using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private float xAxis;
    private float yAxis;

    [SerializeField] private Transform followTargetPos;
    [SerializeField] private Transform player;
    [SerializeField] private Transform pivotPistol, pivotRifle;
    [SerializeField] private float mouseSense;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private bool zoom;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        xAxis += Input.GetAxisRaw("Mouse X") * mouseSense;
        yAxis -= Input.GetAxisRaw("Mouse Y") * mouseSense;
        yAxis = Mathf.Clamp(yAxis, -40, 40);
        zoom = Input.GetMouseButton(1);

        if (zoom)
        {
            virtualCamera.gameObject.SetActive(true);
        }
        else
        {
            virtualCamera.gameObject.SetActive(false);
        }
    }
    private void LateUpdate()
    {
        Rotate();
    }
    private void Rotate()
    {
        followTargetPos.localEulerAngles = new Vector3(yAxis, followTargetPos.localEulerAngles.y, followTargetPos.localEulerAngles.z);
        if (player.GetComponent<Player>().onHand)
        {
            pivotPistol.localEulerAngles = new Vector3(pivotPistol.localEulerAngles.x, pivotPistol.localEulerAngles.y, yAxis);
            pivotRifle.localEulerAngles = new Vector3(pivotRifle.localEulerAngles.x, pivotRifle.localEulerAngles.y, yAxis);
        }
        

        player.localEulerAngles = new Vector3(player.eulerAngles.x, xAxis, player.eulerAngles.z);
    }
}
