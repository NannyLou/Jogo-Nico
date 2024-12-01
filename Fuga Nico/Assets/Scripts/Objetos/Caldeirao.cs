using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caldeirao : MonoBehaviour
{
    public ItemData.items requiredItem = ItemData.items.feno; // ID do item "feno"
    public string[] dialogLines; // Linhas de diálogo definidas no Unity (opcional)

    private UniqueID uniqueID; // Identificador único do caldeirão
    private DialogueManager dialogueManager;

    private void Awake()
    {
        uniqueID = GetComponent<UniqueID>();
    }

    private void Start()
    {
        // Verifica se o caldeirão já foi usado
        if (StateManager.instance != null && uniqueID != null)
        {
            if (StateManager.instance.CheckObjectState(uniqueID.uniqueID, "FenoNoCaldeirao"))
            {
                // Opcional: Execute ações se o caldeirão já foi usado
                gameObject.SetActive(false); // Por exemplo, desativa o caldeirão
                return;
            }
        }

        dialogueManager = DialogueManager.instance;
    }

    private void OnMouseDown()
    {
        // Verifica se o jogador selecionou o item "feno" no inventário
        if (InventarioManager.instance.selectedItemID == requiredItem)
        {
            // Remove o item "feno" do inventário
            InventarioManager.instance.collectedItems.RemoveAll(item => item.itemID == requiredItem);
            InventarioManager.instance.UpdateEquipmentCanvas();
            InventarioManager.instance.SelectItem(-1);

            // Registra que o feno foi usado no caldeirão
            if (StateManager.instance != null && uniqueID != null)
            {
                StateManager.instance.UpdateObjectState(uniqueID.uniqueID, "FenoNoCaldeirao", true);
                Debug.Log("FenoNoCaldeirao state updated to true for uniqueID: " + uniqueID.uniqueID);
            }

            // Opcional: Iniciar diálogo ou outras ações
            if (dialogueManager != null)
            {
                if (dialogLines != null && dialogLines.Length > 0)
                {
                    dialogueManager.StartDialogue(dialogLines);
                }
                else
                {
                    Debug.LogWarning("As linhas de diálogo não foram definidas para o Caldeirao.");
                }
            }
        }
        else
        {
            // Exibe mensagem informando que o caldeirão está bloqueado ou que o item correto não foi selecionado
            if (dialogueManager != null)
            {
                dialogueManager.StartDialogue(new string[]
                {
                    "Caldeirao: Você precisa de feno para usar aqui.",
                });
            }
        }
    }
}
