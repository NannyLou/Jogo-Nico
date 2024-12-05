using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData itemData;  // Referência ao ScriptableObject do item
    public VilaoMovimento vilao; // Referência ao script do vilão

    private ClickMove clickMove;
    private bool isPlayerMovingToItem = false;

    private void Start()
    {
        // Verifica se o item já foi coletado anteriormente
        if (InventarioManager.instance != null && itemData != null)
        {
            if (InventarioManager.instance.collectedItemIDs.Contains(itemData.itemID))
            {
                // Destrói o objeto se o item já foi coletado
                Destroy(gameObject);
                return;
            }
        }

        // Obtém referência ao ClickMove
        clickMove = FindObjectOfType<ClickMove>();
        if (clickMove == null)
        {
            Debug.LogError("ClickMove não encontrado na cena.");
        }
    }

    private void OnMouseDown()
    {
        if (clickMove != null)
        {
            // ADICIONADO: Verifica se o movimento está permitido antes de mover
            if (clickMove.CanMove())
            {
                // Move o jogador até a posição do item
                clickMove.MovePlayerToPoint(transform.position);
                isPlayerMovingToItem = true;
                Debug.Log("MovePlayerToPoint chamado em ItemPickup.");
            }
            else
            {
                Debug.Log("Movimento desabilitado. Não é possível mover o jogador para o item.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPlayerMovingToItem && collision.CompareTag("Player"))
        {
            // Adiciona o item ao inventário
            if (InventarioManager.instance != null)
            {
                InventarioManager.instance.AddItem(itemData);
            }
            else
            {
                Debug.LogError("InventarioManager.instance é nulo.");
            }

            // Notifica o vilão para iniciar o retorno
            if (vilao != null)
            {
                vilao.StartReturn();
            }

            // Remove o objeto da cena
            Destroy(gameObject);

            isPlayerMovingToItem = false;
        }
    }
}
