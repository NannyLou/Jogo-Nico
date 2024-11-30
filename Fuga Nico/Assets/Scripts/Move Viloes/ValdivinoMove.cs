using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValdivinoMove : MonoBehaviour
{
    public Transform[] waypoints;          // Waypoints de movimento do vilão
    public float speed = 2f;               // Velocidade do vilão
    public AnimationData walkAnimation;    // Animação de caminhar
    public AnimationData idleAnimation;    // Animação de parado (de costas)

    public GameObject chavePrefab;         // Prefab da chave
    public Transform chavePosition;        // Posição da chave no vilão
    public GameObject jogador;             // Referência ao jogador
    public Transform jogadorRespawnPoint;  // Ponto de respawn do jogador
    public int waypointIndex;              // Índice do waypoint específico para interação (caldeirão)

    private int currentWaypointIndex = 0;  // Índice do waypoint atual
    private SpriteAnimator spriteAnimator; // Componente de animação do vilão
    private bool isMoving = false;         // Flag para verificar se o vilão está se movendo
    private GameObject chaveInstanciada;   // Referência à chave instanciada

    private void Start()
    {
        spriteAnimator = GetComponent<SpriteAnimator>();
        // Não inicie a animação de "parado" aqui! Deixe isso para o momento em que o vilão atingir o caldeirão.

        // Instancia a chave na posição definida
        if (chavePrefab != null && chavePosition != null)
        {
            chaveInstanciada = Instantiate(chavePrefab, chavePosition.position, Quaternion.identity, transform);
        }
    }

    private void Update()
    {
        // Verifica se o jogador está presente no cenário
        if (jogador != null)
        {
            if (jogador.activeSelf)
            {
                // Jogador está no cenário, vilão fica parado com animação normal
                // Não altere a animação aqui; deixe o vilão em seu estado atual
            }
            else
            {
                // Jogador desapareceu, vilão começa a se mover
                if (!isMoving)
                {
                    StartMoving(); // Inicia o movimento
                }
            }
        }

        if (isMoving)
        {
            MoveAlongPath();
        }

        // Atualiza a posição da chave
        UpdateKeyPosition();
    }


    private void MoveAlongPath()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            Transform target = waypoints[currentWaypointIndex];
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            UpdateSpriteDirection(target.position);

            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                currentWaypointIndex++;

                // Verifica se o vilão chegou no waypoint desejado (caldeirão)
                if (currentWaypointIndex == waypointIndex)
                {
                    OnReachWaypoint();
                }
            }
        }
    }

    private void UpdateSpriteDirection(Vector3 targetPosition)
    {
        if (targetPosition.x > transform.position.x)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    private void UpdateKeyPosition()
    {
        if (chaveInstanciada != null && chavePosition != null)
        {
            chaveInstanciada.transform.position = chavePosition.position;
        }
    }

    public void StartMoving()
    {
        if (!isMoving)
        {
            isMoving = true;
            currentWaypointIndex = 0;

            // Inicia a animação de caminhada
            if (spriteAnimator != null && walkAnimation != null)
            {
                spriteAnimator.PlayAnimation(walkAnimation); // Começa a animação de caminhada
            }
        }
    }

    private void OnReachWaypoint()
    {
        if (jogador != null && jogadorRespawnPoint != null)
        {
            jogador.transform.position = jogadorRespawnPoint.position;
            jogador.SetActive(true); // Reativa o jogador
        }

        isMoving = false; // O vilão para de se mover

        // Altera a animação do vilão para parado (segurando a chave)
        if (spriteAnimator != null && idleAnimation != null)
        {
            spriteAnimator.PlayAnimation(idleAnimation); // Agora ele vai ficar "parado" quando alcançar o caldeirão
        }

        // O vilão já está segurando a chave, a chave foi instanciada previamente
    }
}
