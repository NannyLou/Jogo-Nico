using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caldeirao : MonoBehaviour
{
    public ItemData.items requiredItem = ItemData.items.feno;

    private void OnMouseDown()
    {
        // Verifica se o jogador selecionou o item "feno" no inventário
        if (InventarioManager.instance.selectedItemID == requiredItem)
        {
            // Remove o item "feno" do inventário
            InventarioManager.instance.collectedItems.RemoveAll(item => item.itemID == requiredItem);
            InventarioManager.instance.UpdateEquipmentCanvas();
            InventarioManager.instance.SelectItem(-1);

            Debug.Log("Feno usado no caldeirão, mas nada acontece.");
        }
    }
}
