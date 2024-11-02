using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float correr;
    Vector2 lastClickedPos;
    bool movimento;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 clickedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Direção do personagem para o ponto clicado
            Vector2 direction = clickedPos - (Vector2)transform.position;
            float distance = direction.magnitude;

            // Realiza um Raycast para verificar se há obstáculos entre o personagem e o ponto clicado
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, distance);

            if(hit.collider != null && !hit.collider.isTrigger && hit.collider.gameObject != gameObject)
            {
                // Obstacle detected between character and clicked point
                // Adjust the target position to the point just before the obstacle
                lastClickedPos = hit.point - (hit.normal * 0.1f); // Ajuste o deslocamento conforme necessário
            }
            else
            {
                // No obstacle, move to the clicked position
                lastClickedPos = clickedPos;
            }

            movimento = true;
        }

        if(movimento)
        {
            if((Vector2)transform.position != lastClickedPos)
            {
                float step = correr * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, lastClickedPos, step);
            }
            else
            {
                movimento = false;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Para o movimento ao colidir
        movimento = false;
    }
}