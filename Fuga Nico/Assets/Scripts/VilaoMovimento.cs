using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VilaoMovimento : MonoBehaviour
{
    public Transform[] waypoints; // Array de pontos pelo qual o vilão vai passar
    public float speed = 2f;

    private int currentWaypointIndex = 0; // Índice do ponto atual
    private SpriteAnimator spriteAnimator; // Referência ao SpriteAnimator

    public AnimationData walkAnimation; // Animação de caminhada
    public AnimationData idleAnimation; // Animação de espera/parado

    public GameObject chavePrefab; // Prefab da chave
    private GameObject chaveInstanciada; // Instância da chave
    public Transform chavePosition; // Transform indicando onde a chave será posicionada

    private Vector3 originalScale; // Armazenar a escala original do Vilao

    private void Start()
    {
        spriteAnimator = GetComponent<SpriteAnimator>();
        originalScale = transform.localScale; // Salva a escala inicial

        if (spriteAnimator != null && walkAnimation != null)
        {
            spriteAnimator.PlayAnimation(walkAnimation);
        }

        // Instancia a chave como filho do vilão
        if (chavePrefab != null && chavePosition != null)
        {
            chaveInstanciada = Instantiate(chavePrefab, chavePosition.position, Quaternion.identity, transform);
        }
    }

    private void Update()
    {
        // Verifica se ainda há waypoints para alcançar
        if (currentWaypointIndex < waypoints.Length)
        {
            Transform target = waypoints[currentWaypointIndex];

            // Move em direção ao próximo waypoint
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            // Atualiza a direção do sprite baseado no destino
            if (target.position.x > transform.position.x)
                transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z); // Direita
            else
                transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z); // Esquerda

            // Atualiza a posição da chave
            if (chaveInstanciada != null && chavePosition != null)
            {
                chaveInstanciada.transform.position = chavePosition.position;
            }

            // Checa se chegou no waypoint
            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                currentWaypointIndex++; // Vai para o próximo ponto
            }
        }
        else
        {
            // Para o vilão ao terminar o caminho e muda para a animação idle
            if (spriteAnimator != null && idleAnimation != null)
            {
                spriteAnimator.PlayAnimation(idleAnimation);
            }
        }
    }
}
