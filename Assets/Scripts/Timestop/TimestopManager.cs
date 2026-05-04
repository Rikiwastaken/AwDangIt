using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimestopManager : MonoBehaviour
{
    private InputAction testAction;
    
    public Building selectedBuilding;
    
    // Start is called before the first frame update
    void Start()
    {
        testAction = InputSystem.actions.FindAction("Jump");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(testAction.IsPressed());
        if (testAction.IsPressed())
        {
            selectedBuilding.MoveBuilding(new Vector2Int(1,0));
        }
    }
}
