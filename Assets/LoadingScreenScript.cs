using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenScript : MonoBehaviour
{

    public static LoadingScreenScript instance;

    public Image LoadingImage;

    public float movepersecond;

    public float startx;

    private bool loadscene;

    private string scenetolaod;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (loadscene)
        {

            if (LoadingImage.transform.localPosition.x < 0)
            {
                LoadingImage.transform.localPosition += new Vector3(movepersecond * Time.deltaTime, 0f, 0f);
                if (LoadingImage.transform.localPosition.x >= 0)
                {
                    SceneManager.LoadScene(scenetolaod);
                }
            }
            else
            {
                if (LoadingImage.transform.localPosition.x < -startx)
                {
                    LoadingImage.transform.localPosition += new Vector3(movepersecond * Time.deltaTime, 0f, 0f);
                }
                else
                {
                    loadscene = false;
                }
            }

        }
    }

    public void LoadScene(string sceneName)
    {
        LoadingImage.transform.localPosition = new Vector3(startx, LoadingImage.transform.localPosition.y, LoadingImage.transform.localPosition.z);
        scenetolaod = sceneName;
        loadscene = true;
    }

    public void LoadScene(int sceneID)
    {
        LoadingImage.transform.localPosition = new Vector3(startx, LoadingImage.transform.localPosition.y, LoadingImage.transform.localPosition.z);
        scenetolaod = SceneManager.GetSceneByBuildIndex(sceneID).name;
        loadscene = true;
    }



}
