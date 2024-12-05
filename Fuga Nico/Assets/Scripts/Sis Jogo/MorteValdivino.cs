using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorteValdivino : MonoBehaviour
{
    public ItemData.items requiredItem = ItemData.items.panela; // Item necessário para interação
    public Transform playerRespawnPoint; // Ponto para onde o jogador será transportado
    public Transform telmaWaypoint, teoWaypoint, zecaWaypoint; // Waypoints para os personagens
    public GameObject telma, teo, zeca; // Referência aos personagens
    public GameObject seta; // Objeto da seta que aparece ao final
    public AnimationData telmaAttackAnimation, teoAttackAnimation, playerAttackAnimation; // Animações de ataque
    public float attackDuration = 3f; // Duração da animação de ataque
    public GameObject vilao; // Referência ao vilão

    private bool interactionDone = false; // Verifica se a interação já foi feita

    private void OnMouseDown()
    {
        if (interactionDone) return;

        // Verifica se o jogador possui o item necessário
        if (InventarioManager.instance.selectedItemID == requiredItem)
        {
            interactionDone = true;

            // Remove o item do inventário
            InventarioManager.instance.collectedItems.RemoveAll(item => item.itemID == requiredItem);
            InventarioManager.instance.UpdateEquipmentCanvas();
            InventarioManager.instance.SelectItem(-1);

            // Inicia a sequência de interação
            StartCoroutine(HandleInteraction());
        }
        else
        {
            Debug.Log("Item necessário não está selecionado.");
        }
    }

    private IEnumerator HandleInteraction()
    {
        // Transporta o jogador para o ponto específico
        var player = FindObjectOfType<ClickMove>().gameObject;
        player.transform.position = playerRespawnPoint.position;

        // Faz Telma, Teo e Zeca aparecerem no cenário
        telma.SetActive(true);
        teo.SetActive(true);
        zeca.SetActive(true);

        // Aguarda 1 frame para garantir que os objetos estejam ativos
        yield return null;

        // Move os personagens para suas posições corretas
        telma.transform.position = telmaWaypoint.position;
        teo.transform.position = teoWaypoint.position;
        zeca.transform.position = zecaWaypoint.position;

        // Aguarda um instante para garantir que os personagens estejam na posição
        yield return new WaitForSeconds(0.5f);

        // Obtém os animadores dos personagens e executa as animações de ataque
        var telmaAnimator = telma.GetComponent<SpriteAnimator>();
        var teoAnimator = teo.GetComponent<SpriteAnimator>();
        var playerAnimator = player.GetComponent<SpriteAnimator>();

        if (telmaAnimator != null && telma.activeSelf)
            telmaAnimator.PlayAnimation(telmaAttackAnimation);
        if (teoAnimator != null && teo.activeSelf)
            teoAnimator.PlayAnimation(teoAttackAnimation);
        if (playerAnimator != null)
            playerAnimator.PlayAnimation(playerAttackAnimation);

        // Aguarda o tempo de duração da animação de ataque
        yield return new WaitForSeconds(attackDuration);

        // Remove o vilão do cenário
        if (vilao != null)
        {
            Destroy(vilao);
        }

        // Aguarda um tempo antes de fazer os personagens saírem
        yield return new WaitForSeconds(1f);

        // Esconde os personagens após o ataque
        telma.SetActive(false);
        teo.SetActive(false);
        zeca.SetActive(false);

        // Faz a seta aparecer no cenário
        if (seta != null)
        {
            seta.SetActive(true);
        }

        Debug.Log("Interação concluída.");
    }
}
