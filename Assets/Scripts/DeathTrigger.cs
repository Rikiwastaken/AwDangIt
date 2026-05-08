using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == 3)
        {
            LoadingScreenScript.instance.LoadScene(SceneManager.GetActiveScene().buildIndex);
            //MovementScript.Instance.Respawn();
        }
    }
}
