using System.Collections;
using UnityEngine;

public class ClickMove : MonoBehaviour
{
    public bool playerWalking = false;
    public Transform player;
    GameManager gameManager;
    float goToClickMaxY = 1.7f;
    Camera myCamera;
    Coroutine goToClickCoroutine;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        myCamera = GetComponent<Camera>();

        // Reposiciona o jogador se uma posição inicial foi definida
        if (GameManager.playerStartPosition != Vector2.zero)
        {
            player.position = GameManager.playerStartPosition;

            // Opcional: Reseta a posição inicial no GameManager
            // GameManager.playerStartPosition = Vector2.zero;
        }
    }

    public void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            // Verifica se o clique foi em um obstáculo
            RaycastHit2D hit = Physics2D.Raycast(myCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                // Se o objeto clicado tem a tag "Obstacle", não faz nada
                if (hit.collider.CompareTag("Obstacle"))
                    return;
            }

            // Interrompe a corrotina anterior se houver
            if (goToClickCoroutine != null)
            {
                StopCoroutine(goToClickCoroutine);
            }
            goToClickCoroutine = StartCoroutine(GoToClick(Input.mousePosition));
        }
    }

    public void StopPlayerMovement()
    {
        if (goToClickCoroutine != null)
        {
            StopCoroutine(goToClickCoroutine);
            goToClickCoroutine = null;
        }

        playerWalking = false;
        player.GetComponent<SpriteAnimator>().PlayAnimation(null); // Interrompe a animação de caminhada
    }


    public IEnumerator GoToClick(Vector2 mousePos)
    {
        // Aguarda um pouco para evitar conflitos com outras ações
        yield return new WaitForSeconds(0.05f);

        Vector2 targetPos = myCamera.ScreenToWorldPoint(mousePos);
        if (targetPos.y > goToClickMaxY)
            yield break;

        // Interrompe qualquer movimento atual
        if (playerWalking)
        {
            playerWalking = false;
            yield return null; // Aguarda o final do quadro para garantir que o estado seja atualizado
        }

        // Inicia o movimento
        playerWalking = true;
        goToClickCoroutine = StartCoroutine(gameManager.MoveToPoint(player, targetPos));

        // Inicia a animação de caminhada
        player.GetComponent<SpriteAnimator>().PlayAnimation(gameManager.playerAnimations[1]);

        // Aguarda o término do movimento
        StartCoroutine(CleanAfterClick());
    }

    public void MovePlayerToPoint(Vector2 targetPos)
    {
        // Interrompe a corrotina anterior se houver
        if (goToClickCoroutine != null)
        {
            StopCoroutine(goToClickCoroutine);
        }

        // Interrompe qualquer movimento atual
        if (playerWalking)
        {
            playerWalking = false;
        }

        // Inicia o movimento
        playerWalking = true;
        goToClickCoroutine = StartCoroutine(gameManager.MoveToPoint(player, targetPos));

        // Inicia a animação de caminhada
        player.GetComponent<SpriteAnimator>().PlayAnimation(gameManager.playerAnimations[1]);

        // Aguarda o término do movimento
        StartCoroutine(CleanAfterClick());
    }

    private IEnumerator CleanAfterClick()
    {
        while (playerWalking)
            yield return new WaitForSeconds(0.05f);

        // Para a animação de caminhada
        player.GetComponent<SpriteAnimator>().PlayAnimation(null);
        goToClickCoroutine = null;
        yield return null;
    }
}
