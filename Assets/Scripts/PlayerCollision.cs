using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public GameManager gameManager; // drag your GameManager here

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            gameManager.GameOver();
        }
    }
}


