using UnityEngine;

public class TelefoneController : MonoBehaviour
{
    [Header("Configurações do Telefone")]
    public string[] dialogueLines = {
        "Celular: Alô, precisamos resolver isso agora!",
        "Celular: Tudo está encaminhado."
    };

    [Header("Referências")]
    public UniqueID phoneUniqueID; // Identificador único do telefone
    public ItemData.items activationItemID; // Item necessário para ativar o Sprite e o Collider
    public ItemData.items requiredItemID; // Item necessário para interagir com o telefone
    public Collider2D phoneCollider; // Collider do telefone
    public SpriteRenderer phoneSpriteRenderer; // Sprite Renderer do telefone

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

        // Verifica se o item necessário para ativação está no inventário
        bool hasActivationItem = InventarioManager.instance.collectedItems.Exists(item => item.itemID == activationItemID);

        // Ativa ou desativa o Collider e o Sprite Renderer
        if (phoneCollider != null) phoneCollider.enabled = hasActivationItem;
        if (phoneSpriteRenderer != null) phoneSpriteRenderer.enabled = hasActivationItem;

        if (hasActivationItem)
        {
            Debug.Log("Item necessário para ativação encontrado no inventário. Telefone disponível para visualização.");
        }
        else
        {
            Debug.Log("Item necessário para ativação não encontrado no inventário. Telefone oculto.");
        }
    }

    private void OnMouseDown()
    {
        Debug.Log($"Clique detectado no GameObject com TelefoneController. Ativo: {gameObject.activeSelf}");

        if (!phoneCollider.enabled)
        {
            Debug.Log("Telefone não está disponível para interação.");
            return;
        }

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

        // Verifica se o item necessário para interação está selecionado
        if (InventarioManager.instance.selectedItemID == requiredItemID)
        {
            // Inicia o diálogo
            DialogueManager.instance.OnDialogueEnd += HandleDialogueEnd;
            DialogueManager.instance.StartDialogue(dialogueLines);
            Debug.Log($"Iniciando diálogo para o celular com ID {phoneUniqueID.uniqueID}.");
        }
        else
        {
            Debug.Log("Item correto para interação não está selecionado no inventário.");
        }
    }

    private void HandleDialogueEnd()
    {
        // Desinscreve-se do evento para evitar múltiplas chamadas
        DialogueManager.instance.OnDialogueEnd -= HandleDialogueEnd;

        // Remove o item necessário para interação do inventário
        if (InventarioManager.instance.selectedItemID == requiredItemID)
        {
            InventarioManager.instance.collectedItems.RemoveAll(item => item.itemID == requiredItemID);
            InventarioManager.instance.UpdateEquipmentCanvas();
            InventarioManager.instance.SelectItem(-1);
            Debug.Log($"Celular com ID {phoneUniqueID.uniqueID} removido do inventário.");
        }

        // Marca o telefone como destruído no StateManager
        if (phoneUniqueID != null && StateManager.instance != null)
        {
            StateManager.instance.RegisterDestroyedObject(phoneUniqueID.uniqueID);
            Debug.Log($"Celular com ID {phoneUniqueID.uniqueID} registrado como destruído no StateManager.");
        }

        // Marca que o telefone já foi usado
        hasBeenUsed = true;

        // Destrói o GameObject do telefone
        Destroy(gameObject);
        Debug.Log($"GameObject com o script TelefoneController foi destruído.");
    }
}
