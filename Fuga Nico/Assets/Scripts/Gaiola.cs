using UnityEngine;

public class Gaiola : MonoBehaviour
{
    public ItemData.items requiredItem = ItemData.items.chave; // Item necessário para abrir a gaiola
    private bool isOpen = false;

    public Sprite openedGaiolaSprite; // Sprite da gaiola aberta (se tiver)

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        if (!isOpen)
        {
            if (InventarioManager.instance.selectedItemID == requiredItem)
            {
                OpenGaiola();

                // Remover a chave do inventário, se desejar
                InventarioManager.collectedItems.RemoveAll(item => item.itemID == requiredItem);
                InventarioManager.instance.UpdateEquipmentCanvas();
                InventarioManager.instance.SelectItem(-1);
            }
            else
            {
                Debug.Log("A gaiola está trancada. Você precisa de uma chave.");
            }
        }
    }

    private void OpenGaiola()
    {
        isOpen = true;
        // Se tiver um sprite de gaiola aberta, altere para ele
        if (openedGaiolaSprite != null)
        {
            spriteRenderer.sprite = openedGaiolaSprite;
        }
        else
        {
            // Se não, desative o sprite atual para sumir as grades
            spriteRenderer.enabled = false;
        }

        // Opcional: Desativar o collider para não interagir mais
        GetComponent<Collider2D>().enabled = false;

        // Opcional: Executar alguma animação ou efeito
    }
}
