using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorteValdivino : MonoBehaviour
{
    public ItemData.items itemNecessario = ItemData.items.panela; // Item necessário para interagir com o vilão
    public Transform waypointJogador; // Ponto onde o jogador será transportado
    public Transform waypointTelma, waypointTeo, waypointZeca; // Waypoints para Telma, Teo e Zeca
    public GameObject setaIndicativa; // Seta que aparecerá no final

    public GameObject telmaPrefab, teoPrefab, zecaPrefab; // Prefabs de Telma, Teo e Zeca
    private GameObject telmaInstance, teoInstance, zecaInstance; // Instâncias no cenário

    public AnimationData animacaoJogadorBater; // Animação do jogador bater no vilão
    public AnimationData animacaoTelmaBater, animacaoTeoBater, animacaoZecaBater; // Animações de Telma, Teo e Zeca

    private SpriteAnimator jogadorAnimator; // Referência ao animador do jogador
    private ClickMove clickMove; // Controle de movimento do jogador

    private Vector3 cameraFixedPosition; // Posição fixa da câmera

    private SpriteRenderer spriteRenderer; // Referência ao SpriteRenderer de Valdivino
    private Collider2D colliderValdivino; // Referência ao Collider2D de Valdivino

    private void Start()
    {
        clickMove = FindObjectOfType<ClickMove>();
        if (clickMove != null)
        {
            jogadorAnimator = clickMove.GetComponent<SpriteAnimator>();
        }

        if (setaIndicativa != null)
            setaIndicativa.SetActive(false); // Garante que a seta esteja oculta inicialmente

        // Captura a posição inicial da câmera
        cameraFixedPosition = Camera.main.transform.position;

        // Obtém referências ao SpriteRenderer e Collider2D do Valdivino
        spriteRenderer = GetComponent<SpriteRenderer>();
        colliderValdivino = GetComponent<Collider2D>();
    }

    private void OnMouseDown()
    {
        // Verifica se o item correto está selecionado no inventário
        if (InventarioManager.instance.selectedItemID == itemNecessario)
        {
            // Remove o item do inventário
            InventarioManager.instance.collectedItems.RemoveAll(item => item.itemID == itemNecessario);
            InventarioManager.instance.UpdateEquipmentCanvas();
            InventarioManager.instance.SelectItem(-1);

            // Inicia a interação
            StartCoroutine(InteragirComVilao());
        }
        else
        {
            Debug.Log("Você precisa usar o item correto!");
            // Opcional: Adicionar feedback visual ao jogador
        }
    }

    private IEnumerator InteragirComVilao()
    {
        // Desabilita o movimento do jogador
        clickMove.DisableMovement();

        // Transporta o jogador para o waypoint
        clickMove.transform.position = waypointJogador.position;

        // Mantém a câmera fixa na posição inicial
        Camera.main.transform.position = cameraFixedPosition;

        // Instancia Telma, Teo e Zeca no cenário
        telmaInstance = Instantiate(telmaPrefab, waypointTelma.position, waypointTelma.rotation);
        teoInstance = Instantiate(teoPrefab, waypointTeo.position, waypointTeo.rotation);
        zecaInstance = Instantiate(zecaPrefab, waypointZeca.position, waypointZeca.rotation);

        // Aguarda um pequeno tempo para simular transição
        yield return new WaitForSeconds(0.5f);

        // Telma, Teo, Zeca e Jogador fazem a animação de bater no vilão
        if (jogadorAnimator != null && animacaoJogadorBater != null)
        {
            jogadorAnimator.PlayAnimation(animacaoJogadorBater);
        }
        if (telmaInstance != null && telmaInstance.TryGetComponent<SpriteAnimator>(out var telmaAnimator) && animacaoTelmaBater != null)
        {
            telmaAnimator.PlayAnimation(animacaoTelmaBater);
        }
        if (teoInstance != null && teoInstance.TryGetComponent<SpriteAnimator>(out var teoAnimator) && animacaoTeoBater != null)
        {
            teoAnimator.PlayAnimation(animacaoTeoBater);
        }
        if (zecaInstance != null && zecaInstance.TryGetComponent<SpriteAnimator>(out var zecaAnimator) && animacaoZecaBater != null)
        {
            zecaAnimator.PlayAnimation(animacaoZecaBater);
        }

        // Aguarda a duração das animações antes de iniciar os timers de desativação
        yield return new WaitForSeconds(3f); // Tempo para Valdivino desaparecer antes dos personagens

        // Desativa o Sprite e o Collider do Valdivino após 3 segundos
        if (spriteRenderer != null)
            spriteRenderer.enabled = false;
        if (colliderValdivino != null)
            colliderValdivino.enabled = false;

        // Inicia um timer para desativar os personagens e ativar a seta após 2 segundos adicionais (total 5 segundos)
        StartCoroutine(DesativarPersonagensEAtivarSeta(2f));

        // Mantém a câmera fixa novamente, por garantia
        Camera.main.transform.position = cameraFixedPosition;

        // Aguarda o tempo total antes de reabilitar o movimento do jogador
        yield return new WaitForSeconds(2f);

        // Habilita o movimento do jogador novamente
        clickMove.EnableMovement();
    }

    private IEnumerator DesativarPersonagensEAtivarSeta(float delay)
    {
        // Aguarda o tempo especificado
        yield return new WaitForSeconds(delay);

        // Remove Telma, Teo e Zeca do cenário
        if (telmaInstance != null) Destroy(telmaInstance);
        if (teoInstance != null) Destroy(teoInstance);
        if (zecaInstance != null) Destroy(zecaInstance);

        // Mostra a seta indicativa
        if (setaIndicativa != null)
        {
            setaIndicativa.SetActive(true);
        }
    }
}
