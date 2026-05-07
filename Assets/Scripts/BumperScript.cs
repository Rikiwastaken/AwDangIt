using UnityEngine;

public class BumperScript : MonoBehaviour
{

    public float forceamount;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            MovementScript.Instance.velocity += transform.up * forceamount;
            MovementScript.Instance.GetComponent<CharacterController>()
                .Move(transform.up * forceamount * Time.deltaTime);
        }
    }

}
