using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static List<int> collectedItems = new List<int>();
    public float moveSpeed = 3.5f;
    public float moveAccuracy = 0.15f;
    public AnimationData[] playerAnimations;
    int activeLocalScene = 0;

    public IEnumerator MoveToPoint(Transform myObject, Vector2 point)
    {
        Rigidbody2D rb = myObject.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D não encontrado no objeto do jogador.");
            yield break;
        }

        SpriteRenderer spriteRenderer = myObject.GetComponentInChildren<SpriteRenderer>();

        // Variável para detectar colisão
        bool isBlocked = false;

        // Obter ou adicionar o CollisionDetector
        CollisionDetector collisionDetector = myObject.GetComponent<CollisionDetector>();
        if (collisionDetector == null)
        {
            collisionDetector = myObject.gameObject.AddComponent<CollisionDetector>();
        }

        // Handler de colisão
        Action collisionHandler = () => { isBlocked = true; };

        // Subscrição ao evento
        collisionDetector.OnCollisionDetected += collisionHandler;

        while (true)
        {
            Vector2 currentPosition = rb.position;
            Vector2 direction = (point - currentPosition).normalized;
            float remainingDistance = Vector2.Distance(currentPosition, point);

            if (remainingDistance <= moveAccuracy || isBlocked)
                break;

            if (spriteRenderer && direction.x != 0)
            {
                spriteRenderer.flipX = direction.x > 0;
            }

            // Move o jogador usando o Rigidbody2D
            float step = moveSpeed * Time.fixedDeltaTime;
            Vector2 newPosition = Vector2.MoveTowards(rb.position, point, step);
            rb.MovePosition(newPosition);

            yield return new WaitForFixedUpdate();
        }

        // Remove a subscrição ao evento
        collisionDetector.OnCollisionDetected -= collisionHandler;

        // Para o jogador
        rb.velocity = Vector2.zero;

        // Informa ao ClickMove que o jogador chegou ou está bloqueado
        ClickMove clickMove = FindObjectOfType<ClickMove>();
        if (clickMove != null && (myObject == clickMove.player || activeLocalScene == 0))
            clickMove.playerWalking = false;

        yield return null;
    }
}
