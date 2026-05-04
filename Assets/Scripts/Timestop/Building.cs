using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;
using UnityEngine.InputSystem;

public class Building : MonoBehaviour
{
    public List<Vector2Int> gridPosition;

    public void MoveBuilding(Vector2Int delta)
    {
        for (int i = 0; i < gridPosition.Count; i++)
        {
            gridPosition[i] += delta;
        }

        transform.localPosition = new Vector3(
            transform.localPosition.x + delta.x * Constants.TimestopConstants.GridSize,
            transform.localPosition.y,
            transform.localPosition.z + delta.y * Constants.TimestopConstants.GridSize
        );
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
