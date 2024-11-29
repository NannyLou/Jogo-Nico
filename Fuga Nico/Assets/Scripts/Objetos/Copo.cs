using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Copo : MonoBehaviour
{
    public DanielMove vilao; // Referência ao vilão
    public ItemData.items requiredItem = ItemData.items.remedio; // ID do item "remédio"
    public GameObject copo; // Referência ao objeto copo

    private void OnMouseDown()
    {
        // Verifica se o jogador selecionou o item "remédio" no inventário
        if (InventarioManager.instance.selectedItemID == requiredItem && copo != null)
        {
            // Remove o item "remédio" do inventário
            InventarioManager.instance.collectedItems.RemoveAll(item => item.itemID == requiredItem);
            InventarioManager.instance.UpdateEquipmentCanvas();
            InventarioManager.instance.SelectItem(-1);
            
            // Apenas remove o remédio, não inicia o movimento do vilão
        }
    }
}
