using Cinemachine;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        dronebaseY = Drone.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        ManageDroneMovement();
    }

    public void TestBouton()
    {
        Debug.Log("Ca marche");
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
