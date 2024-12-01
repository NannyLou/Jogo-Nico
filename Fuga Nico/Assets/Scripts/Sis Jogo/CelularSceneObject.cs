using System.Collections;
using UnityEngine;

public class TelefoneItem : MonoBehaviour
{
    [Header("Configurações do Telefone")]
    public string[] dialogueLines = {
        "Celular: Alô, precisamos resolver isso agora!",
        "Celular: Tudo está encaminhado."
    }; // Linhas de diálogo a serem exibidas

    [Header("Referências do Celular")]
    public UniqueID phoneUniqueID; // Referência ao UniqueID do celular
    public GameObject phoneGameObject; // Referência ao GameObject do celular

    private bool hasBeenUsed = false; // Garantir que o Telefone seja usado apenas uma vez

    private void OnMouseDown()
    {
        if (hasBeenUsed)
        {
            Debug.Log($"O celular com ID {phoneUniqueID.uniqueID} já foi usado e não pode ser reutilizado.");
            return;
        }

        // Verifica se o Telefone está selecionado no inventário
        if (InventarioManager.instance.selectedItemID == ItemData.items.celular)
        {
            // Inicia o diálogo
            DialogueManager.instance.OnDialogueEnd += HandleDialogueEnd;
            DialogueManager.instance.StartDialogue(dialogueLines);
            Debug.Log($"Iniciando diálogo para o celular com ID {phoneUniqueID.uniqueID}.");
        }
    }

    private void HandleDialogueEnd()
    {
        // Desinscreve-se do evento para evitar múltiplas chamadas
        DialogueManager.instance.OnDialogueEnd -= HandleDialogueEnd;

        // Remove o celular do inventário de forma semelhante ao seu exemplo
        if (InventarioManager.instance.selectedItemID == ItemData.items.celular)
        {
            // Remove todos os itens com itemID igual a celular
            InventarioManager.instance.collectedItems.RemoveAll(item => item.itemID == ItemData.items.celular);
            InventarioManager.instance.UpdateEquipmentCanvas();
            InventarioManager.instance.SelectItem(-1);
            Debug.Log($"Celular com ID {phoneUniqueID.uniqueID} removido do inventário.");
        }

        // Marca o celular como destruído no StateManager
        if (phoneUniqueID != null && StateManager.instance != null)
        {
            StateManager.instance.RegisterDestroyedObject(phoneUniqueID.uniqueID);
            Debug.Log($"Celular com ID {phoneUniqueID.uniqueID} registrado como destruído no StateManager.");
        }

        // Marca que o Telefone já foi usado
        hasBeenUsed = true;

        // Desativa o GameObject do celular na cena
        if (phoneGameObject != null)
        {
            phoneGameObject.SetActive(false);
            Debug.Log($"GameObject do celular com ID {phoneUniqueID.uniqueID} desativado na cena.");
        }
    }
}
