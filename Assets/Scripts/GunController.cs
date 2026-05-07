using UnityEngine;
using UnityEngine.InputSystem;

public class GunController : MonoBehaviour
{
    public static GunController Instance { get; private set; }

    public GameObject bulletHolePrefab;
    public Transform spawnTransform;

    [Header("Gun Properties")]
    public float falloffDistance;

    private InputAction _shootAction;
    public GameObject LaserPrefab;

    private GameObject PreviousLaser;

    private void Awake()
    {
        Instance = this;
    }

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
                    out hit, falloffDistance))
            {
                if (hit.collider.gameObject.layer == 6 && bulletHolePrefab)
                {
                    //GameObject o = Instantiate(bulletHolePrefab);
                    //o.transform.position = hit.point;
                }
                else if (hit.collider.gameObject.layer == 9)
                {
                    // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                    Target t = hit.collider.gameObject.GetComponent<Target>();
                    t.Hit();
                }
                CylinderInstantiator(hit.point);
            }
        }
    }

    private void CylinderInstantiator(Vector3 hitposition)
    {
        if (PreviousLaser == null)
        {
            PreviousLaser = Instantiate(LaserPrefab);
        }

        Transform DroneTransform = DroneFollow.Instance.transform;
        PreviousLaser.GetComponent<LaserScript>().ResetMat();
        PreviousLaser.transform.position = (DroneTransform.position + hitposition) / 2f;
        PreviousLaser.transform.up = (hitposition - DroneTransform.position).normalized;
        PreviousLaser.transform.localScale = new Vector3(PreviousLaser.transform.localScale.x, Vector3.Distance(hitposition, DroneTransform.position) / 2f, PreviousLaser.transform.localScale.z);
    }
}
