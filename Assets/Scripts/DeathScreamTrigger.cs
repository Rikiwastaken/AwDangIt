using UnityEngine;

public class DeathScreamTrigger : MonoBehaviour
{
    private SFXDriver _sfxDriver;

    private void Start()
    {
        _sfxDriver = GetComponent<SFXDriver>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            _sfxDriver.PlayRandomSound();
            MovementScript.Instance.canControl = false;
            MovementScript.Instance.GetCamTranform().parent = null;
        }
    }
}