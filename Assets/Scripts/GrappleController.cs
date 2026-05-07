using System;
using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrappleController : MonoBehaviour
{
    public static GrappleController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private GameObject[] grapplepointList;



    private GameObject SelectedGrapplePoint;

    public float mindistgorgrapple;
    public float minangleforgrapple;

    private Transform CameraTransform;

    private InputAction _grappleAction;

    public bool Isgrappling;

    public float grapplingspeed;
    public float MinDistToStopGrappling;

    private Vector3 previousvelocity;

    private CharacterController _characterController;

    public float xrotationpersec;
    public float yrotationpersec;
    public float zrotationpersec;

    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        GetGrapplepoint();
        CameraTransform = GetComponentInChildren<CinemachineVirtualCamera>().transform;
        _characterController = GetComponent<CharacterController>();
        _grappleAction = InputSystem.actions.FindAction("Grapple");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!Isgrappling)
        {
            FindGrappinable();
        }

        SetSelectedImage();
        Grapple();
    }

    private void Grapple()
    {
        if (Isgrappling)
        {
            if (Vector3.Distance(SelectedGrapplePoint.transform.position, CameraTransform.position) <= MinDistToStopGrappling || _grappleAction.WasPerformedThisFrame())
            {
                Isgrappling = false;
                _characterController.Move(previousvelocity);
                audioSource.Stop();
                return;
            }
            Vector3 velocity = (SelectedGrapplePoint.transform.position - CameraTransform.position).normalized * grapplingspeed * Time.deltaTime;
            previousvelocity = velocity;
            _characterController.Move(velocity);
        }
        else
        {
            if (SelectedGrapplePoint != null && _grappleAction.WasPerformedThisFrame())
            {
                Isgrappling = true;
                audioSource.Play();
            }
        }
    }

    private void FindGrappinable()
    {
        // get all potential grapple points
        List<GameObject> grappinable = new List<GameObject>();
        foreach (GameObject grapplepoint in grapplepointList)
        {
            if (Vector3.Distance(CameraTransform.position, grapplepoint.transform.position) > mindistgorgrapple)
            {
                continue;
            }
            if (Vector3.Angle(CameraTransform.forward, (grapplepoint.transform.position - CameraTransform.position).normalized) > minangleforgrapple)
            {
                continue;
            }
            RaycastHit hit;
            if (Physics.Raycast(CameraTransform.position, (grapplepoint.transform.position - CameraTransform.position).normalized, out hit, mindistgorgrapple, LayerMask.GetMask("Ground", "GrapplePoint")))
            {
                if (hit.collider.gameObject == grapplepoint)
                {
                    grappinable.Add(grapplepoint);
                }
            }
        }

        // only keep the closest one

        float mindist = 99999;
        SelectedGrapplePoint = null;
        foreach (GameObject grapplepoint in grappinable)
        {
            if (Vector3.Distance(CameraTransform.position, grapplepoint.transform.position) < mindist)
            {
                mindist = Vector3.Distance(CameraTransform.position, grapplepoint.transform.position);
                SelectedGrapplePoint = grapplepoint;
            }
        }

    }

    private void SetSelectedImage()
    {
        foreach (GameObject grapple in grapplepointList)
        {
            GameObject Canvas = grapple.transform.GetChild(0).gameObject;
            GameObject Model = grapple.transform.GetChild(1).gameObject;
            if (grapple == SelectedGrapplePoint)
            {
                if (!Canvas.activeSelf)
                {
                    Canvas.SetActive(true);
                }
                Canvas.transform.LookAt(CameraTransform);
                Model.transform.rotation = Quaternion.Euler(Model.transform.rotation.eulerAngles + new Vector3(xrotationpersec, yrotationpersec, zrotationpersec) * Time.deltaTime);
            }
            else
            {
                if (Canvas.activeSelf)
                {
                    Canvas.SetActive(false);
                }
            }
        }
    }

    private void GetGrapplepoint()
    {
        grapplepointList = GameObject.FindGameObjectsWithTag("GrapplePoint");
    }
}
