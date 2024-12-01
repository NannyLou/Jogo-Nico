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

    [Header("Referências do Celular")]
    public UniqueID phoneUniqueID;         // Referência ao UniqueID do celular
    public GameObject phoneGameObject;     // Referência ao GameObject do celular

    private int currentWaypointIndex = 0;  // Índice do waypoint atual
    private SpriteAnimator spriteAnimator; // Componente de animação do vilão
    private bool isMoving = false;         // Flag para verificar se o vilão está se movendo
    private GameObject chaveInstanciada;   // Referência à chave instanciada

    private void Start()
    {
        spriteAnimator = GetComponent<SpriteAnimator>();

        // Instancia a chave na posição definida
        if (chavePrefab != null && chavePosition != null)
        {
            chaveInstanciada = Instantiate(chavePrefab, chavePosition.position, Quaternion.identity, transform);
        }

        // Verifica se o celular já foi usado e desativa se necessário
        if (phoneUniqueID != null && phoneGameObject != null)
        {
            if (StateManager.instance.IsObjectDestroyed(phoneUniqueID.uniqueID))
            {
                phoneGameObject.SetActive(false);
                Destroy(gameObject);
            }
            else
            {
                phoneGameObject.SetActive(true);
            }
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            MoveAlongPath();
        }

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

            if (spriteAnimator != null && walkAnimation != null)
            {
                spriteAnimator.PlayAnimation(walkAnimation);
            }
        }
    }

    private void OnReachWaypoint()
    {
        if (jogador != null && jogadorRespawnPoint != null)
        {
            ClickMove clickMove = FindObjectOfType<ClickMove>();
            if (clickMove != null)
            {
                Debug.Log("ClickMove encontrado via FindObjectOfType. Chamando StopPlayerMovement.");
                clickMove.StopPlayerMovement(); // Interrompe o movimento pendente do jogador
            }
            else
            {
                Debug.LogWarning("ClickMove não encontrado via FindObjectOfType.");
            }

            jogador.transform.position = jogadorRespawnPoint.position;
            jogador.SetActive(true); // Reativa o jogador no cenário
        }

        isMoving = false;

        if (spriteAnimator != null && idleAnimation != null)
        {
            spriteAnimator.PlayAnimation(idleAnimation);
        }
    }

}
