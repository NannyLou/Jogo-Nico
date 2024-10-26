using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData itemData;  // Referência ao ScriptableObject do item

    private void OnMouseDown()
    {
        // Adiciona o item ao inventário
        InventarioManager.collectedItems.Add(itemData);  // Verifique se o item é adicionado corretamente
        InventarioManager.instance.UpdateEquipmentCanvas();  // Atualiza a UI do inventário

        // Remove o objeto da cena
        Destroy(gameObject);
    }
}
