using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimestopManager : MonoBehaviour
{
    private InputAction _moveAction;
    private Vector2 _oldMoveDir;
    private InputAction _cursorAction;
    private InputAction _shootAction;

    private List<Building> _childBuildings = new List<Building>();
    
    public Vector2Int levelSize;
    
    public Building selectedBuilding;
    public Camera mainCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Movement");
        _oldMoveDir = Vector2.zero;
        _cursorAction = InputSystem.actions.FindAction("Cursor");
        _shootAction = InputSystem.actions.FindAction("Shoot");

        mainCamera.transform.localPosition = new Vector3(
            Constants.TimestopConstants.GridSize * (levelSize.x / 2.0f - 0.5f),
            300.0f,
            Constants.TimestopConstants.GridSize * (levelSize.y / 2.0f  - 0.5f) - 100.0f
        );

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
                selectedBuilding = hit.collider.transform.parent.GetComponent<Building>();
            }
        }
        
        Vector2 moveDir = _moveAction.ReadValue<Vector2>();
        
        if (selectedBuilding)
        {
            if (moveDir.x != 0 && _oldMoveDir.x == 0)
            {
                selectedBuilding.MoveBuilding(new Vector2Int((int) moveDir.x, 0));
            }
            if (moveDir.y != 0 && _oldMoveDir.y == 0)
            {
                selectedBuilding.MoveBuilding(new Vector2Int(0, (int) moveDir.y));
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
