using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayBox : MonoBehaviour
{
    private Building _building;

    void Start()
    {
        _building = transform.parent.GetComponent<Building>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            if (MovementScript.Instance.IsGrounded())
            {
                MovementScript.Instance.lastBuilding = _building;
            }
            _building.playerRidden = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            _building.playerRidden = false;
        }
    }
}
