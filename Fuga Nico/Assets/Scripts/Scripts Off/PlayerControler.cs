using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isColliding = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Você pode filtrar as colisões por tag ou camada, se desejar
        isColliding = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isColliding = false;
    }
}
