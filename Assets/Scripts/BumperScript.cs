using UnityEngine;

public class BumperScript : MonoBehaviour
{

    public float forceamount;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.transform.GetComponentInChildren<Rigidbody>().velocity += transform.up * forceamount;
        }
    }

}
