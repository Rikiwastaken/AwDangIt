using TMPro;
using UnityEngine;

public class TimerScript : MonoBehaviour
{

    public TextMeshProUGUI TimerTMP;

    private float timerVal;
    private bool timerisPlaying;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (timerisPlaying)
        {
            timerVal += Time.deltaTime;
            TimerTMP.text = TimeToString(timerVal);
        }
    }

    public void StartTimer()
    {
        timerisPlaying = true;
        timerVal = 0f;
    }

    public void StopTimer()
    {
        timerVal = 0f;
        timerisPlaying = false;
    }

    public void PauseTimer()
    {
        timerisPlaying = false;
    }

    public void UnPauseTimer()
    {
        timerisPlaying = true;
    }

    private string TimeToString(float timer = 0f)
    {
        string seconds = (int)(timer % 60) + "";
        if (seconds.Length == 1)
        {
            seconds = "0" + seconds;
        }

        string rest = "" + ((float)(timer % 60) - (float)((int)(timer % 60)));

        rest = rest.Substring(1, 3);

        return ((int)timer / 60) + ":" + seconds + rest;
    }

#if UNITY_EDITOR
    [ContextMenu("Start Timer")]
    void StartTimerCtxMenu()
    {
        StartTimer();
    }

    [ContextMenu("Stop Timer")]
    void StopTimerCtxMenu()
    {
        StopTimer();
    }

    [ContextMenu("Pause Timer")]
    void PauseTimerCtxMenu()
    {
        PauseTimer();
    }

    [ContextMenu("Unpause Timer")]
    void UnpauseTimerCtxMenu()
    {
        UnPauseTimer();
    }
#endif

}
