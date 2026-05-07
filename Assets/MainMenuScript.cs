using Cinemachine;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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

    // Start is called before the first frame update
    void Start()
    {
        dronebaseY = Drone.transform.position.y;

        SetLevelNames();

    }

    // Update is called once per frame
    void Update()
    {
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
        Debug.Log("loading level : " + levelname);
        SceneManager.LoadScene(levelname);
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
