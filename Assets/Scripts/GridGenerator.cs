using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public static GridGenerator Instance { get; private set; }

    public GameObject gridSectionsHolder;
    public GameObject gridSectionPrefab;

    private void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        Vector2Int gridSize = TimestopManager.Instance.levelSize;
        float halfX = Constants.TimestopConstants.GridSize * (gridSize.x / 2f - 0.5f);
        for (int i = 0; i < gridSize.y + 1; i++)
        {
            GameObject o = Instantiate(gridSectionPrefab, gridSectionsHolder.transform, true);
            o.transform.localPosition = new Vector3(halfX, 0, (i - 0.5f) * Constants.TimestopConstants.GridSize);
            o.transform.localScale = new Vector3(Constants.TimestopConstants.GridSize * gridSize.x / 10f, 1, 0.25f);
        }
        float halfY = Constants.TimestopConstants.GridSize * (gridSize.y / 2f - 0.5f);
        for (int i = 0; i < gridSize.x + 1; i++)
        {
            GameObject o = Instantiate(gridSectionPrefab, gridSectionsHolder.transform, true);
            o.transform.localPosition = new Vector3((i - 0.5f) * Constants.TimestopConstants.GridSize, 0, halfY);
            o.transform.localScale = new Vector3(Constants.TimestopConstants.GridSize * gridSize.y / 10f, 1, 0.25f);
            o.transform.localRotation = Quaternion.Euler(0,90,0);
        }
    }
}
