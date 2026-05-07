using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    public static LevelEnd Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    [Header("text")]
    public TMP_Text timeText;
    public TMP_Text timestopText;
    public TMP_Text movesText;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Open()
    {
        gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        MovementScript.Instance.enabled = false;
        GunController.Instance.enabled = false;
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        // MovementScript.Instance.gameObject.GetComponent<Rigidbody>().freezeRotation = true;

        TimerScript.Instance.PauseTimer();

        timeText.text = "Time: " + TimerScript.TimeToString(TimerScript.Instance.timerVal);
        timestopText.text = "Timestops used: " + ModeSwitcher.Instance.switches;
        movesText.text = "Moves: " + TimestopManager.Instance.moves;
        SaveLevelData(TimerScript.Instance.timerVal, ModeSwitcher.Instance.switches, TimestopManager.Instance.moves);
    }

    private void SaveLevelData(float newtime, int newtimestop, int newmoves)
    {
        DataScript.LevelSaveData level = DataScript.instance.GetLevelData(DataScript.instance.currentlevelID);
        DataScript.LevelSaveData newleveldata = null;
        if (level != null)
        {
            newleveldata = new DataScript.LevelSaveData() { ID = level.ID, PBTime = level.PBTime, PBMoves = level.PBMoves, PBTimeStop = level.PBTimeStop };
            if (level.PBTime > newtime)
            {
                newleveldata.PBTime = newtime;
            }
            if (level.PBMoves > newmoves)
            {
                newleveldata.PBMoves = newmoves;
            }
            if (level.PBTimeStop > newtimestop)
            {
                newleveldata.PBTimeStop = newtimestop;
            }
        }
        else
        {
            newleveldata = new DataScript.LevelSaveData() { ID = DataScript.instance.currentlevelID, PBTime = newtime, PBMoves = newmoves, PBTimeStop = newtimestop };
        }

        DataScript.instance.SaveLevelData(newleveldata);
    }

    public void LevelSelect()
    {
        SceneManager.LoadScene("Scenes/LevelSelect", LoadSceneMode.Single);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}
