using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Copo : MonoBehaviour
{
    public ItemData.items requiredItem = ItemData.items.remedio; // ID do item "remédio"
    public string[] dialogLines; // Linhas de diálogo definidas no Unity

    private UniqueID uniqueID; // Identificador único do copo
    private DialogueManager dialogueManager;

    private void Awake()
    {
        uniqueID = GetComponent<UniqueID>();
    }

    private void Start()
    {
        // Verifica se o copo já foi "destruído" (ocultado)
        if (StateManager.instance != null && uniqueID != null)
        {
            if (StateManager.instance.IsObjectDestroyed(uniqueID.uniqueID))
            {
                gameObject.SetActive(false); // Oculta o copo
                return;
            }
        }

        dialogueManager = DialogueManager.instance;
    }

    private void OnMouseDown()
    {
        // Verifica se o jogador selecionou o item "remédio" no inventário
        if (InventarioManager.instance.selectedItemID == requiredItem)
        {
            // Remove o item "remédio" do inventário
            InventarioManager.instance.collectedItems.RemoveAll(item => item.itemID == requiredItem);
            InventarioManager.instance.UpdateEquipmentCanvas();
            InventarioManager.instance.SelectItem(-1);

            // Registra que o remédio foi colocado no copo
            if (StateManager.instance != null && uniqueID != null)
            {
                StateManager.instance.UpdateObjectState(uniqueID.uniqueID, "RemedioNoCopo", true);
                Debug.Log("RemedioNoCopo state updated to true for uniqueID: " + uniqueID.uniqueID);
            }

            // Inicia o diálogo
            if (dialogueManager != null)
            {
                if (dialogLines != null && dialogLines.Length > 0)
                {
                    dialogueManager.StartDialogue(dialogLines);
                }
                else
                {
                    Debug.LogWarning("As linhas de diálogo não foram definidas para o Copo.");
                }
            }

            // Não destrói o copo aqui
        }
    }
}
