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

    private bool canMove = true; // ADICIONADO: Flag para controlar o movimento

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        myCamera = GetComponent<Camera>();

        // Reposiciona o jogador se uma posição inicial foi definida
        if (GameManager.playerStartPosition != Vector2.zero)
        {
            player.position = GameManager.playerStartPosition;
        }

        // ADICIONADO: Log para verificar a referência do player
        if (player != null)
        {
            //Debug.Log("Player referenciado em ClickMove: " + player.name);
        }
        else
        {
            //Debug.LogWarning("Player não está referenciado no ClickMove!");
        }
    }

    public void Update()
    {
        // MODIFICADO: Adicionada verificação da flag canMove
        if (!canMove || (DialogueManager.instance != null && !DialogueManager.instance.CanPlayerMove))
        {
            //Debug.Log("Movimento bloqueado: canMove = " + canMove);
            return;
        }
        else
        {
            //Debug.Log("Movimento permitido: canMove = " + canMove);
        }

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
        //Debug.Log("StopPlayerMovement chamado: Movimento parado.");
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
            //Debug.Log("Interrompendo movimento atual.");
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
        // ADICIONADO: Verifica se o movimento está permitido
        if (!canMove)
        {
            //Debug.Log("Cannot move: Movement is disabled.");
            return;
        }

        // Interrompe a corrotina anterior se houver
        if (goToClickCoroutine != null)
        {
            StopCoroutine(goToClickCoroutine);
        }

        // Interrompe qualquer movimento atual
        if (playerWalking)
        {
            playerWalking = false;
            //Debug.Log("Interrompendo movimento durante MovePlayerToPoint.");
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
        //Debug.Log("CleanAfterClick concluído: Movimento finalizado.");
        yield return null;
    }

    // ADICIONADO: Método para desabilitar o movimento do personagem
    public void DisableMovement()
    {
        canMove = false;
        StopPlayerMovement(); // Opcional: interrompe qualquer movimento atual
        //Debug.Log("DisableMovement chamado: Movimento desabilitado.");
    }

    // ADICIONADO: Método para habilitar o movimento do personagem
    public void EnableMovement()
    {
        canMove = true;
        //Debug.Log("EnableMovement chamado: Movimento habilitado.");
    }

    // ADICIONADO: Método para verificar se o movimento está permitido
    public bool CanMove()
    {
        return canMove;
    }
}
