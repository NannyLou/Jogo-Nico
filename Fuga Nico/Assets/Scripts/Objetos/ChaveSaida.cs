using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaveSaida : MonoBehaviour
{
    public ItemData.items requiredItem = ItemData.items.telma; // Item necessário (Telma)
    public Transform telmaWaypoint; // Ponto onde Telma aparecerá
    public Transform chaveWaypoint; // Ponto onde a chave cairá
    public GameObject telmaPrefab; // Prefab de Telma
    public GameObject chavePrefab; // Prefab da chave
    public AnimationData telmaAnimation; // Animação de Telma pegando a chave
    private GameObject telmaInstance; // Instância de Telma
    private ClickMove clickMove; // ADICIONADO: Referência para o script ClickMove do jogador

    private void Awake()
    {
        // Localiza o componente ClickMove no cenário
        clickMove = FindObjectOfType<ClickMove>(); // ADICIONADO: Inicializa a referência ao ClickMove
    }

    public void OnMouseDown()
    {
        // Verifica se o item selecionado é Telma
        if (InventarioManager.instance.selectedItemID == requiredItem)
        {
            // Remove Telma do inventário
            InventarioManager.instance.collectedItems.RemoveAll(item => item.itemID == requiredItem);
            InventarioManager.instance.UpdateEquipmentCanvas();
            InventarioManager.instance.SelectItem(-1);

            // ADICIONADO: Desabilita a movimentação do jogador
            if (clickMove != null)
            {
                clickMove.DisableMovement();
                Debug.Log("Movimentação do jogador desabilitada.");
            }

            // Inicia o processo de Telma pegar a chave
            StartCoroutine(TelmaPegaChave());
        }
        else
        {
            Debug.Log("O item necessário não está selecionado.");
        }
    }

    private IEnumerator TelmaPegaChave()
    {
        // Instancia Telma no waypoint
        telmaInstance = Instantiate(telmaPrefab, telmaWaypoint.position, telmaWaypoint.rotation);
        SpriteAnimator telmaAnimator = telmaInstance.GetComponent<SpriteAnimator>();

        // Executa a animação de Telma pegando a chave
        if (telmaAnimator != null && telmaAnimation != null)
        {
            telmaAnimator.PlayAnimation(telmaAnimation);
        }

        // Aguarda a duração da animação (2 segundos)
        yield return new WaitForSeconds(2); // Ajuste conforme a duração real da animação

        // Instancia a chave no ponto designado
        Instantiate(chavePrefab, chaveWaypoint.position, chaveWaypoint.rotation);

        // Remove Telma da cena após a animação
        if (telmaInstance != null)
        {
            Destroy(telmaInstance);
        }

        // Remove o objeto original da chave do cenário
        Destroy(gameObject); // Assumindo que este script está anexado ao objeto original da chave

        // ADICIONADO: Reabilita a movimentação do jogador
        if (clickMove != null)
        {
            clickMove.EnableMovement();
            Debug.Log("Movimentação do jogador habilitada.");
        }
    }
}
