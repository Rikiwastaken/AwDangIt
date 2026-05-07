using UnityEngine;
using Random = UnityEngine.Random;

public class DroneFollow : MonoBehaviour
{
    public static DroneFollow Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private SFXDriver _sfxDriver;
    private float _lastBigMovement;

    private void Start()
    {
        _sfxDriver = GetComponent<SFXDriver>();
        _lastBigMovement = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // transform.position = Vector3.Lerp(transform.position, MovementScript.Instance.droneTarget, 0.05f);
        Vector3 newPos = Vector3.Lerp(transform.position, MovementScript.Instance.droneTarget, 0.05f);

        if ((newPos - transform.position).magnitude > 0.01f)
        {
            _lastBigMovement = Time.time;
        }
        else if (Time.time - _lastBigMovement > 5f && Random.Range(0, 600) == 0)
        {
            // _sfxDriver.PlayRandomSound();
        }

        transform.position = newPos;

        transform.forward = MovementScript.Instance.GetCamTranform().forward;

    }
}
