using UnityEngine;

public class BalloonTrigger : MonoBehaviour
{
    public GameManager gameManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("baloon"))
        {
            gameManager.SwitchObject();
        }
    }
}