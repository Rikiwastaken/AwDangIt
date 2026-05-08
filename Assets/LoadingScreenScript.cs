using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenScript : MonoBehaviour
{

    public static LoadingScreenScript instance;

    public Image LoadingImage;

    public float startx;

    private bool loadscene;

    private string scenetolaod;

    public float timefortransition;

    private float transitionendtime;

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
                float currentx = Mathf.Lerp(LoadingImage.transform.localPosition.x, 0f, 1.0f - (transitionendtime - Time.time) / timefortransition);
                LoadingImage.transform.localPosition = new Vector3(currentx, LoadingImage.transform.localPosition.y, LoadingImage.transform.localPosition.z);
                if (LoadingImage.transform.localPosition.x >= 0)
                {
                    SceneManager.LoadScene(scenetolaod);
                    transitionendtime = -1;
                }
            }
            else
            {
                if (transitionendtime == -1)
                {
                    transitionendtime = Time.time + timefortransition;
                }
                if (LoadingImage.transform.localPosition.x < -startx)
                {
                    float currentx = Mathf.Lerp(LoadingImage.transform.localPosition.x, -startx, 1.0f - (transitionendtime - Time.time) / timefortransition);
                    LoadingImage.transform.localPosition = new Vector3(currentx, LoadingImage.transform.localPosition.y, LoadingImage.transform.localPosition.z);
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
        transitionendtime = Time.time + timefortransition;
    }

    public void LoadScene(int sceneID)
    {
        LoadingImage.transform.localPosition = new Vector3(startx, LoadingImage.transform.localPosition.y, LoadingImage.transform.localPosition.z);
        scenetolaod = SceneManager.GetSceneByBuildIndex(sceneID).name;
        loadscene = true;
        transitionendtime = Time.time + timefortransition;
    }



}
