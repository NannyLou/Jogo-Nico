using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValdivinoMove : MonoBehaviour
{
    public Transform[] waypoints;          // Waypoints de movimento do vilão
    public float speed = 2f;               // Velocidade do vilão
    public AnimationData walkAnimation;    // Animação de caminhar
    public AnimationData idleAnimation;    // Animação de parado (de costas)
    public AnimationData keyAnimation;     // Animação com a chave (de costas segurando a chave)
    
    public GameObject chavePrefab;         // Prefab da chave
    public GameObject caldeirao;           // Referência ao caldeirão onde o item feno será usado
    public GameObject jogador;             // Referência ao jogador
    public Transform caldeiraoWaypoint;    // Ponto de waypoint do caldeirão
    public Transform jogadorRespawnPoint;  // Ponto de respawn do jogador

    private int currentWaypointIndex = 0;  // Índice do waypoint atual
    private SpriteAnimator spriteAnimator; // Componente de animação do vilão
    private bool isMoving = false;         // Flag para verificar se o vilão está se movendo
    private bool hasDroppedKey = false;    // Flag para verificar se a chave já foi entregue ao jogador

    private void Start()
    {
        spriteAnimator = GetComponent<SpriteAnimator>();
        if (spriteAnimator != null && idleAnimation != null)
        {
            spriteAnimator.PlayAnimation(idleAnimation); // Começa com a animação de parado
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            MoveAlongPath();
        }
    }

    // Função de movimento ao longo do caminho
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
            }
        }
        else
        {
            // Se o vilão chegou no caldeirão (último waypoint), muda a animação e aparece o jogador
            if (currentWaypointIndex == waypoints.Length)
            {
                // Muda a animação para "de costas com a chave"
                if (spriteAnimator != null && keyAnimation != null)
                {
                    spriteAnimator.PlayAnimation(keyAnimation);
                }

                // Reativa o jogador
                if (jogador != null && jogadorRespawnPoint != null)
                {
                    jogador.transform.position = jogadorRespawnPoint.position;
                    jogador.SetActive(true);  // Reativa o jogador
                }
            }
        }
    }

    // Atualiza a direção do vilão (vira ele conforme a posição do waypoint)
    private void UpdateSpriteDirection(Vector3 targetPosition)
    {
        if (targetPosition.x > transform.position.x)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    // Inicia o movimento do vilão até o caldeirão
    public void StartMoving()
    {
        if (!isMoving)
        {
            isMoving = true;
            currentWaypointIndex = 0;

            // Inicia a animação de caminhada
            if (spriteAnimator != null && walkAnimation != null)
            {
                spriteAnimator.PlayAnimation(walkAnimation);
            }
        }
    }

    // Função chamada quando o item feno é usado no caldeirão
    public void UseFenoOnCaldeirao()
    {
        if (InventarioManager.instance.HasItem(ItemData.items.feno))
        {
            // Remove o feno do inventário
            InventarioManager.instance.collectedItems.RemoveAll(item => item.itemID == ItemData.items.feno);
            InventarioManager.instance.UpdateEquipmentCanvas();
            InventarioManager.instance.SelectItem(-1);
            
            // Não acontece nada no cenário, só remove o item
        }
    }

    // Função chamada quando o jogador coleta a chave
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o jogador pegou a chave
        if (collision.CompareTag("Player") && !hasDroppedKey)
        {
            // Adiciona a chave no inventário do jogador
            ItemData chaveItemData = chavePrefab.GetComponent<ItemPickup>().itemData;
            if (chaveItemData != null)
            {
                InventarioManager.instance.AddItem(chaveItemData);
                hasDroppedKey = true;
            }

            // Remove o vilão e a chave da cena
            Destroy(chavePrefab);
            Destroy(gameObject);
        }
    }
}
