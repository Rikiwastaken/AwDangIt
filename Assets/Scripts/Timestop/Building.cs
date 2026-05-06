using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Constants;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class Building : MonoBehaviour
{
    [Header("level data")]
    public Vector2Int gridPosition;
    public bool interactible = true;
    
    [Header("building data")]
    public List<Vector2Int> gridSpots;

    [Header("accessors")]
    public Renderer selector;
    public Material materialSelected;
    public Material materialUninteractable;
    public Material materialPlayerRidden;
    
    [Header("debug")]
    public bool playerRidden;

    public bool MoveBuilding(Vector2Int delta)
    {
        if (playerRidden || !interactible)
        {
            return false;
        }
        
        List<Vector2Int> oldGridPosition = gridSpots.Clone(new ListCloner(), false);
        for (int i = 0; i < oldGridPosition.Count; i++)
        {
            oldGridPosition[i] += gridPosition;
        }
        
        List<Vector2Int> projectedGridPosition = gridSpots.Clone(new ListCloner(), false);
        for (int i = 0; i < projectedGridPosition.Count; i++)
        {
            projectedGridPosition[i] += delta + gridPosition;
        }
        
        bool validMove = true;
        List<Vector2Int> allPositions = TimestopManager.Instance.GetAllBuildingPositions();
        foreach (var p in projectedGridPosition)
        {
            if (p.x < 0 || p.y < 0 || p.x >= TimestopManager.Instance.levelSize.x || p.y >= TimestopManager.Instance.levelSize.y || (allPositions.Contains(p) && !oldGridPosition.Contains(p)))
            {
                validMove = false;
                break;
            }
        }
        
        if (validMove)
        {
            gridPosition += delta;
            transform.localPosition = new Vector3(
                transform.localPosition.x + delta.x * Constants.TimestopConstants.GridSize,
                transform.localPosition.y,
                transform.localPosition.z + delta.y * Constants.TimestopConstants.GridSize
            );
        }

        return validMove;
    }

    public void ShowSelector()
    {
        if (playerRidden)
        {
            selector.material = materialPlayerRidden;
            selector.gameObject.SetActive(true);
        }
        if (!interactible)
        {
            selector.material = materialUninteractable;
            selector.gameObject.SetActive(true);
        }
    }
    
    public void HideSelector()
    {
        selector.gameObject.SetActive(false);
    }

    public void OnSelected()
    {
        selector.material = materialSelected;
        selector.gameObject.SetActive(true);
    }

    public void OnDeselected()
    {
        HideSelector();
        ShowSelector();
    }
    
#if UNITY_EDITOR
    [ContextMenu("Calculate real position")]
    void CalculateRealPosition()
    {
        if (Application.isPlaying)
        {
            return;
        }

        transform.localPosition = new Vector3(gridPosition.x * Constants.TimestopConstants.GridSize, transform.localPosition.y, gridPosition.y * Constants.TimestopConstants.GridSize);
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif
}
