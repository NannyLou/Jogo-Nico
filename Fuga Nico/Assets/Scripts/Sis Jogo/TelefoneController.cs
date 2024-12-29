using UnityEngine;
using UnityEngine.UI;

public class TelefoneController : MonoBehaviour
{
    [Header("Configurações do Telefone")]
    public string[] dialogueLines = {
        "Celular: Alô, precisamos resolver isso agora!",
        "Celular: Tudo está encaminhado."
    };

    [Header("Referências")]
    public UniqueID phoneUniqueID; // Identificador único do telefone
    public ItemData.items activationItemID; // Item necessário para ativar o telefone
    public ItemData.items requiredItemID; // Item necessário para interagir com o telefone
    public Button celularObj; // O botão que representa o telefone na cena

    private bool hasBeenUsed = false; // Garante que o telefone só seja usado uma vez

    private void Start()
    {
        // Verifica se o telefone já foi destruído
        if (phoneUniqueID != null && StateManager.instance != null)
        {
            if (StateManager.instance.IsObjectDestroyed(phoneUniqueID.uniqueID))
            {
                Debug.Log($"Celular com ID {phoneUniqueID.uniqueID} já foi destruído. Removendo da cena.");
                Destroy(gameObject);
                return;
            }
        }

        // Inicializa o estado do telefone
        UpdateTelefoneState();

        // Adiciona ouvinte de clique ao botão celularObj
        celularObj.onClick.AddListener(OnPhoneButtonClick);
    }

    private void Update()
    {
        // Atualiza dinamicamente o estado do telefone
        UpdateTelefoneState();
    }

    private void UpdateTelefoneState()
    {
        if (InventarioManager.instance == null)
        {
            Debug.LogError("InventarioManager não está configurado ou não foi encontrado.");
            return;
        }

        // Verifica se os itens necessários estão no inventário
        bool hasActivationItem = InventarioManager.instance.collectedItems.Exists(item => item.itemID == activationItemID);
        bool hasRequiredItem = InventarioManager.instance.collectedItems.Exists(item => item.itemID == requiredItemID);

        // Ativa o botão celularObj se os dois itens estiverem no inventário
        if (celularObj != null)
        {
            celularObj.gameObject.SetActive(hasActivationItem && hasRequiredItem);
        }

        if (hasActivationItem && hasRequiredItem)
        {
            Debug.Log("Itens necessários encontrados no inventário. Botão do telefone disponível.");
        }
        else
        {
            Debug.Log("Itens necessários não encontrados no inventário. Botão do telefone oculto.");
        }
    }

    private void OnPhoneButtonClick()
    {
        Debug.Log($"Clique detectado no botão com TelefoneController. Ativo: {celularObj.gameObject.activeSelf}");

        if (hasBeenUsed)
        {
            Debug.Log($"O celular com ID {phoneUniqueID.uniqueID} já foi usado e não pode ser reutilizado.");
            return;
        }

        if (InventarioManager.instance == null)
        {
            Debug.LogError("InventarioManager não está configurado ou não foi encontrado.");
            return;
        }

        // Verifica se o item necessário para interação está no inventário
        if (InventarioManager.instance.HasItem(ItemData.items.celular))
        {
            // Remove o item Celular do inventário
            InventarioManager.instance.collectedItems.RemoveAll(item => item.itemID == ItemData.items.celular);
            InventarioManager.instance.UpdateEquipmentCanvas();
            InventarioManager.instance.SelectItem(-1);
            Debug.Log("Item Celular removido do inventário.");

            // Inicia o diálogo
            DialogueManager.instance.OnDialogueEnd += HandleDialogueEnd;
            DialogueManager.instance.StartDialogue(dialogueLines);
            Debug.Log($"Iniciando diálogo para o celular com ID {phoneUniqueID.uniqueID}.");
        }
        else
        {
            Debug.Log("Item Celular não encontrado no inventário.");
        }
    }

    private void HandleDialogueEnd()
    {
        // Desinscreve-se do evento para evitar múltiplas chamadas
        DialogueManager.instance.OnDialogueEnd -= HandleDialogueEnd;

        // Marca o telefone como destruído no StateManager
        if (phoneUniqueID != null && StateManager.instance != null)
        {
            StateManager.instance.RegisterDestroyedObject(phoneUniqueID.uniqueID);
            Debug.Log($"Celular com ID {phoneUniqueID.uniqueID} registrado como destruído no StateManager.");
        }

        // Marca que o telefone já foi usado
        hasBeenUsed = true;

        // Desativa o botão celularObj
        if (celularObj != null)
        {
            celularObj.gameObject.SetActive(false);
            Debug.Log("Botão 'Celular Obj' foi desativado.");
        }

        // Destrói o GameObject do controlador
        Destroy(gameObject);
        Debug.Log($"GameObject com o script TelefoneController foi destruído.");
    }
}
