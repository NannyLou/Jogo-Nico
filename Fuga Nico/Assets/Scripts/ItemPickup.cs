using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData itemData;  // Referência ao ScriptableObject do item
    public float maxPickupDistance = 1.5f; // Distância máxima para pegar o item

    private Collider2D playerCollider;
    private Collider2D itemCollider;

    private void Start()
    {
        // Encontre o jogador na cena (assumindo que ele tem a tag "Player")
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerCollider = player.GetComponentInChildren<Collider2D>();
            if (playerCollider == null)
            {
                Debug.LogError("Collider2D não encontrado no jogador.");
            }
        }
        else
        {
            Debug.LogError("Jogador não encontrado na cena. Certifique-se de que o jogador tem a tag 'Player'.");
        }

        itemCollider = GetComponent<Collider2D>();
        if (itemCollider == null)
        {
            Debug.LogError("Collider2D não encontrado no item.");
        }
    }

    private void OnMouseDown()
    {
        if (playerCollider == null || itemCollider == null)
            return;

        // Calcula a distância entre os pontos mais próximos dos dois colisores
        Vector2 closestPointOnPlayer = playerCollider.ClosestPoint(itemCollider.bounds.center);
        Vector2 closestPointOnItem = itemCollider.ClosestPoint(playerCollider.bounds.center);

        float distance = Vector2.Distance(closestPointOnPlayer, closestPointOnItem);

        // Logs de depuração
        Debug.Log("Distância entre os colisores: " + distance);
        Debug.Log("Distância máxima para pegar o item: " + maxPickupDistance);

        if (distance <= maxPickupDistance)
        {
            // Adiciona o item ao inventário
            InventarioManager.collectedItems.Add(itemData);
            InventarioManager.instance.UpdateEquipmentCanvas();

            // Remove o objeto da cena
            Destroy(gameObject);
        }
        else
        {
            // Exibe mensagem informando que o personagem está longe
            MensagemManager mensagemManager = FindObjectOfType<MensagemManager>();
            if (mensagemManager != null)
            {
                mensagemManager.MostrarMensagem("Você está muito longe para pegar o item.");
            }
        }
    }
}
