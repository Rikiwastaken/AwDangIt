using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTracker : MonoBehaviour
{
    
    private TargetTracker[] _allTrackers;

    public static GoalTracker Instance { get; private set; }
    
    private void Awake()
    {
        Instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _allTrackers = GetComponentsInChildren<TargetTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        bool levelFinished = true;
        foreach (var tracker in _allTrackers)
        {
            if (tracker.targets > 0)
            {
                levelFinished = false;
                break;
            }
        }

        if (levelFinished && !LevelEnd.Instance.gameObject.activeSelf)
        {
            // Cursor.lockState = CursorLockMode.None;
            // SceneManager.LoadScene("Scenes/LevelSelect", LoadSceneMode.Single);
            LevelEnd.Instance.Open();
        }
    }

    public int GetRemaining()
    {
        int remaining = 0;
        foreach (var tracker in _allTrackers)
        {
            remaining += tracker.targets;
        }

        return remaining;
    }
}
