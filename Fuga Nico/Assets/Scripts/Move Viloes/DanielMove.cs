using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanielMove : MonoBehaviour
{
    public Transform[] waypoints; // Posições do caminho do vilão
    public float speed = 2f;

    public int currentWaypointIndex = 0;
    private SpriteAnimator spriteAnimator;

    public AnimationData walkAnimation; // Animação de caminhar
    public AnimationData idleAnimation; // Animação de parado (sentado)

    public GameObject copo; // Referência ao copo
    public int waypointIndex; // Define qual waypoint está o copo

    public Transform jogadorRespawnPoint; // Ponto específico para o jogador reaparecer
    public GameObject jogador; // Referência ao jogador

    private Vector3 originalScale;

    private bool isMoving = false;

    private UniqueID uniqueID; // Identificador único do vilão

    private void Awake()
    {
        uniqueID = GetComponent<UniqueID>();
    }

    private void Start()
    {
        // Verifica se o vilão já foi destruído
        if (StateManager.instance != null && uniqueID != null)
        {
            if (StateManager.instance.IsObjectDestroyed(uniqueID.uniqueID))
            {
                Destroy(gameObject); // Remove o vilão imediatamente
                return;
            }
        }

        spriteAnimator = GetComponent<SpriteAnimator>();
        originalScale = transform.localScale;

        if (spriteAnimator != null && idleAnimation != null)
        {
            spriteAnimator.PlayAnimation(idleAnimation); // Começa com a animação de sentado
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            MoveAlongPath();
        }
    }

    private void MoveAlongPath()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            Transform target = waypoints[currentWaypointIndex];

            // Move o vilão em direção ao waypoint
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            // Atualiza a direção do sprite
            UpdateSpriteDirection(target.position);

            // Verifica se chegou no waypoint
            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                // Se o waypoint atual é onde o copo está, oculta o copo
                if (currentWaypointIndex == waypointIndex && copo != null)
                {
                    copo.SetActive(false); // Oculta o copo

                    // Registra o copo como "destruído" no StateManager
                    if (StateManager.instance != null)
                    {
                        var copoUniqueID = copo.GetComponent<UniqueID>();
                        if (copoUniqueID != null)
                        {
                            StateManager.instance.RegisterDestroyedObject(copoUniqueID.uniqueID);
                        }
                    }
                }

                currentWaypointIndex++;
            }
        }
        else
        {
            // Quando terminar o caminho, reaparece o jogador no ponto específico
            if (jogador != null && jogadorRespawnPoint != null)
            {
                ClickMove clickMove = FindObjectOfType<ClickMove>();
                if (clickMove != null)
                {
                    clickMove.StopPlayerMovement(); // Interrompe qualquer movimento pendente
                }
                jogador.transform.position = jogadorRespawnPoint.position; // Move o jogador ao ponto de respawn
                jogador.SetActive(true); // Reativa o jogador no cenário
            }

            // Registra o vilão como destruído no StateManager
            if (StateManager.instance != null && uniqueID != null)
            {
                StateManager.instance.RegisterDestroyedObject(uniqueID.uniqueID);
            }

            Destroy(gameObject); // Remove o vilão
        }
    }

    private void UpdateSpriteDirection(Vector3 targetPosition)
    {
        // Ajusta a escala para virar o sprite na direção correta
        if (targetPosition.x > transform.position.x)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }

    public void StartMoving()
    {
        if (!isMoving)
        {
            isMoving = true;
            currentWaypointIndex = 0;

            if (spriteAnimator != null && walkAnimation != null)
            {
                spriteAnimator.PlayAnimation(walkAnimation); // Inicia a animação de andar
            }
        }
    }
}
