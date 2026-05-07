using Cinemachine;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [Header("Scene objects")]
    public CinemachineVirtualCamera TitleCam;
    public CinemachineVirtualCamera MenuCam;

    public Transform Drone;
    [Header("Variables")]
    public float dronemovespeed;
    public float maxdroneY;
    private bool ismovingup;
    private float dronebaseY;

    public List<string> levelnames;
    public Transform LevelListTransform;

    private GameObject latestselected;

    public TextMeshProUGUI DetailsTMP;

    [Header("Option Variables")]
    public Slider MasterSlider;
    public Slider MusicSlider;
    public Slider SFXSlider;
    public TextMeshProUGUI MasterTxt;
    public TextMeshProUGUI MusicTxt;
    public TextMeshProUGUI SFXTxt;
    private float previousMaster;
    private float previousMusic;
    private float previousSFX;
    private bool schedulesave;

    // Start is called before the first frame update
    void Start()
    {
        dronebaseY = Drone.transform.position.y;
        InitializeSliders();
        SetLevelNames();

    }

    // Update is called once per frame
    void Update()
    {

        if (TitleCam.enabled && (InputSystem.actions.FindAction("Shoot").IsPressed() || InputSystem.actions.FindAction("Jump").IsPressed()))
        {
            TitleCam.enabled = false;
        }

        ManageDroneMovement();
        if (LevelListTransform.gameObject.activeSelf)
        {
            GameObject hoveredGO = GetHoveredButton();
            if (hoveredGO != null && hoveredGO != latestselected)
            {
                latestselected = hoveredGO;
                SetDetailsText();
            }
        }
        if (MasterSlider.transform.parent.gameObject.activeSelf)
        {
            UpdateVolume();
        }
    }

    private void UpdateVolume()
    {
        if (previousMaster != MasterSlider.value || previousMusic != MusicSlider.value || previousSFX != SFXSlider.value)
        {
            previousMaster = MasterSlider.value;
            previousMusic = MusicSlider.value;
            previousSFX = SFXSlider.value;
            MasterTxt.text = "Master: " + (int)(MasterSlider.value * 100);
            MusicTxt.text = "Music: " + (int)(MusicSlider.value * 100);
            SFXTxt.text = "SFX: " + (int)(SFXSlider.value * 100);
            schedulesave = true;
            DataScript.instance.UpdateMixer(MasterSlider.value, MusicSlider.value, SFXSlider.value);
        }
    }
    private void InitializeSliders()
    {
        DataScript.OptionData optionData = DataScript.instance.GetOptionData();
        MasterSlider.value = optionData.MasterVol;
        SFXSlider.value = optionData.SFXVol;
        MusicSlider.value = optionData.MusicVol;
        previousMaster = MasterSlider.value;
        previousMusic = MusicSlider.value;
        previousSFX = SFXSlider.value;
        MasterTxt.text = "Master: " + (int)(optionData.MasterVol * 100);
        MusicTxt.text = "Music: " + (int)(optionData.MusicVol * 100);
        SFXTxt.text = "SFX: " + (int)(optionData.SFXVol * 100);
    }

    private GameObject GetHoveredButton()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            Button button = result.gameObject.GetComponent<Button>();

            if (button != null)
                return button.gameObject;
        }

        return null;
    }

    private void SetLevelNames()
    {
        for (int i = 0; i < levelnames.Count; i++)
        {
            if (i < LevelListTransform.childCount)
            {
                LevelListTransform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = levelnames[i];
            }
        }
    }

    public void SaveOptions() // to be called  by button
    {
        if (schedulesave)
        {
            DataScript.instance.SaveData();
            schedulesave = false;
        }
    }

    private void SetDetailsText()
    {
        if (latestselected != null)
        {
            DataScript.LevelSaveData currentleveldata = GetLevelData(levelnames.IndexOf(latestselected.GetComponentInChildren<TextMeshProUGUI>().text));
            string detailtext = latestselected.GetComponentInChildren<TextMeshProUGUI>().text + "\n";
            detailtext += "Best Time: " + TimerScript.TimeToString(currentleveldata.PBTime) + "\n";
            detailtext += "Best Timestops: " + currentleveldata.PBTimeStop + "\n";
            detailtext += "Best Permutations: " + currentleveldata.PBMoves;
            DetailsTMP.text = detailtext;
        }

    }

    public void LoadLevel(TextMeshProUGUI ButtonTMP)
    {


        string levelname = ButtonTMP.text;
        DataScript.instance.currentlevelID = levelnames.IndexOf(levelname);
        Debug.Log("loading level : " + levelname);
        SceneManager.LoadScene("Scenes/Levels/" + levelname);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    private DataScript.LevelSaveData GetLevelData(int ID)
    {
        DataScript.LevelSaveData data = DataScript.instance.GetLevelData(ID);
        if (data == null)
        {
            return new DataScript.LevelSaveData() { ID = ID, PBMoves = 0, PBTime = 0f, PBTimeStop = 0 };
        }
        else
        {
            return data;
        }
    }
    private void ManageDroneMovement()
    {
        if (ismovingup)
        {
            if (Drone.transform.position.y < dronebaseY + maxdroneY)
            {
                Drone.transform.position += new Vector3(0f, dronemovespeed * Time.deltaTime, 0f);
            }
            else
            {
                ismovingup = false;
            }
        }
        else
        {
            if (Drone.transform.position.y > dronebaseY - maxdroneY)
            {
                Drone.transform.position -= new Vector3(0f, dronemovespeed * Time.deltaTime, 0f);
            }
            else
            {
                ismovingup = true;
            }
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Set title cam")]
    void TestTitleCam()
    {
        TitleCam.enabled = true;

    }

    [ContextMenu("Set Menu Cam")]
    void TestMenuCam()
    {
        TitleCam.enabled = false;

    }


#endif
}
