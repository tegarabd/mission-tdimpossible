using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountDown : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] GameObject timerPanel;
    [SerializeField] TextMeshProUGUI timerText;

    bool start;
    public static bool stop;
    int timer;

    private void Start()
    {
        start = false;
        stop = false;
        timer = 60;
    }
    private void Update()
    {
        if (Physics.CheckSphere(transform.position, 10f, playerLayer))
        {
            if (!start)
            {
                start = true;
                timerPanel.SetActive(true);
                timerText.SetText("01:00");
                StartCoroutine(CountDownTimerRoutine());
            }
        }

        if (stop)
        {
            StopTimer();
        }
    }

    public void StopTimer()
    {
        timer = 60;
        StopCoroutine(CountDownTimerRoutine());
        timerPanel.SetActive(false);
    }

    private IEnumerator CountDownTimerRoutine()
    {
        while (timer > 0)
        {
            yield return new WaitForSeconds(1);
            CountDownTimer();
        }
        
    }

    private void CountDownTimer()
    {
        timer--;
        if (timer <= 0) GameManager.Instance.PlayerDeadScreen();
        timerText.SetText("00:" + ((timer < 10) ? "0" + timer : "" + timer));
    }
}
