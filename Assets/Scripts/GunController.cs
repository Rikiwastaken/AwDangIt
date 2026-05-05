using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunController : MonoBehaviour
{
    public GameObject bulletHolePrefab;
    public Transform spawnTransform;

    private InputAction _shootAction;

    private void Start()
    {
        _shootAction = InputSystem.actions.FindAction("Shoot");
    }

    void Update()
    {
        if (_shootAction.WasPressedThisFrame())
        {
            // GameObject o = Instantiate(bulletPrefab);
            // o.transform.position = playerCamera.transform.position;
            // // o.transform.eulerAngles.y = transform.eulerAngles.y;
            // o.transform.eulerAngles = new Vector3(playerCamera.transform.eulerAngles.x, transform.eulerAngles.y, 0);
            
            RaycastHit hit;
            if (Physics.Raycast(spawnTransform.transform.position,
                    spawnTransform.transform.forward,
                    out hit, Mathf.Infinity, LayerMask.GetMask("Ground", "Target")))
            {
                if (hit.collider.gameObject.layer == 6)
                {
                    GameObject o = Instantiate(bulletHolePrefab);
                    o.transform.position = hit.point;
                }
                else if (hit.collider.gameObject.layer == 9)
                {
                    // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                    Target t = hit.collider.gameObject.GetComponent<Target>();
                    t.Hit();
                }
            }
        }
    }
}
