using System;
using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimestopManager : MonoBehaviour
{
    public static TimestopManager Instance { get; private set; }
    
    private InputAction _moveAction;
    private Vector2 _oldMoveDir;
    private InputAction _cursorAction;
    private InputAction _shootAction;

    private List<Building> _childBuildings = new List<Building>();

    [Header("level data")]
    public Vector2Int levelSize;
    public float averageHeight;
    
    [Header("accessors")]
    public Camera mainCamera;
    public CinemachineVirtualCamera virtualCamera;

    [Header("public variables (debug)")]
    public Building selectedBuilding;
    public bool buildingMoved = false;
    public int moves = 0;

    private void Awake()
    {
        Instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Movement");
        _oldMoveDir = Vector2.zero;
        _cursorAction = InputSystem.actions.FindAction("Cursor");
        _shootAction = InputSystem.actions.FindAction("Shoot");

        virtualCamera.transform.localPosition = new Vector3(
            Constants.TimestopConstants.GridSize * (levelSize.x / 2.0f - 0.5f),
            50.0f + Math.Max(levelSize.x, levelSize.y) * 25.0f,
            Constants.TimestopConstants.GridSize * (levelSize.y / 2.0f - 0.5f) - Math.Max(levelSize.x, levelSize.y) * 25.0f
        );
        virtualCamera.transform.LookAt(new Vector3(
            Constants.TimestopConstants.GridSize * (levelSize.x / 2.0f - 0.5f),
            averageHeight,
            Constants.TimestopConstants.GridSize * (levelSize.y / 2.0f - 0.5f)
        ));

        foreach (Transform child in transform)
        {
            Building building = child.gameObject.GetComponent<Building>();
            _childBuildings.Add(building);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_shootAction.WasPressedThisFrame())
        {
            Vector2 cursorPos = _cursorAction.ReadValue<Vector2>();
            Ray ray = mainCamera.ScreenPointToRay(new Vector3(cursorPos.x, cursorPos.y, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("BuildingCollider")))
            {
                // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                Building aimedBuilding = hit.collider.transform.parent.GetComponent<Building>();
                if (!(aimedBuilding.playerRidden || !aimedBuilding.interactible))
                {
                    selectedBuilding = aimedBuilding;
                    ArrowSelectIdle.Instance.gameObject.SetActive(true);
                }

            }
        }

        Vector2 moveDir = _moveAction.ReadValue<Vector2>();

        if (selectedBuilding)
        {
            bool validMove = false;
            ArrowSelectIdle.Instance.position = selectedBuilding.transform.position + new Vector3(0, 20.0f, 0);
            if (moveDir.x != 0 && _oldMoveDir.x == 0)
            {
                validMove = selectedBuilding.MoveBuilding(new Vector2Int((int)moveDir.x, 0));
            }
            if (moveDir.y != 0 && _oldMoveDir.y == 0)
            {
                validMove = selectedBuilding.MoveBuilding(new Vector2Int(0, (int)moveDir.y));
            }

            if (validMove)
            {
                buildingMoved = true;
                moves++;
            }
        }

        _oldMoveDir = moveDir;
    }

    public List<Vector2Int> GetAllBuildingPositions()
    {
        List<Vector2Int> result = new List<Vector2Int>();
        foreach (Building building in _childBuildings)
        {
            foreach (var gs in building.gridSpots)
            {
                result.Add(gs + building.gridPosition);
            }
        }
        return result;
    }
}
