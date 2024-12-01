using UnityEngine;

public class GaiolaTeoTelma : MonoBehaviour
{
    public GameObject Teo;
    public GameObject Telma;
    public Sprite openedGaiolaSprite; // Sprite da gaiola aberta (se tiver)

    private bool isOpen = false;
    private SpriteRenderer spriteRenderer;

    private UniqueID uniqueID; // Identificador único da gaiola
    private DialogueManager dialogueManager;
    public string[] dialogLines; // Linhas de diálogo definidas no Unity

    public ItemData teoItemData;   // ItemData associado ao Teo
    public ItemData telmaItemData; // ItemData associado à Telma

    public string cadeadoID; // UniqueID do cadeado para verificação no StateManager

    public bool isTeoScene; // Define se este é o cenário de Teo
    public bool isTelmaScene; // Define se este é o cenário de Telma

    private void Awake()
    {
        uniqueID = GetComponent<UniqueID>();
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Verifica se a gaiola ou o cadeado já foram destruídos
        if (StateManager.instance != null)
        {
            bool gaiolaDestruida = uniqueID != null && StateManager.instance.IsObjectDestroyed(uniqueID.uniqueID);
            bool cadeadoDestruido = !string.IsNullOrEmpty(cadeadoID) && StateManager.instance.IsObjectDestroyed(cadeadoID);

            if (gaiolaDestruida || cadeadoDestruido)
            {
                // Destrói a gaiola
                Destroy(gameObject);

                // Se for o cenário do Teo, destrói apenas o Teo
                if (isTeoScene && Teo != null)
                {
                    Destroy(Teo);
                }

                // Se for o cenário da Telma, destrói apenas a Telma
                if (isTelmaScene && Telma != null)
                {
                    Destroy(Telma);
                }

                return;
            }
        }

        // Verifica se o item já está no inventário
        if (isTeoScene && InventarioManager.instance.HasItem(teoItemData.itemID))
        {
            if (Teo != null)
                Destroy(Teo);
        }
        else if (isTelmaScene && InventarioManager.instance.HasItem(telmaItemData.itemID))
        {
            if (Telma != null)
                Destroy(Telma);
        }

        dialogueManager = DialogueManager.instance;
    }

    public void OpenGaiola()
    {
        if (isOpen)
            return;

        isOpen = true;

        // Se tiver um sprite de gaiola aberta, altera para ele
        if (openedGaiolaSprite != null)
        {
            spriteRenderer.sprite = openedGaiolaSprite;
        }
        else
        {
            // Se não, desativa o sprite atual para sumir as grades
            spriteRenderer.enabled = false;
        }

        // Inicia o diálogo
        if (dialogueManager != null)
        {
            if (dialogLines != null && dialogLines.Length > 0)
            {
                dialogueManager.StartDialogue(dialogLines);
                dialogueManager.OnDialogueEnd += OnDialogueEnd; // Subscreve ao evento de fim de diálogo
            }
            else
            {
                Debug.LogWarning("As linhas de diálogo não foram definidas para a gaiola.");
            }
        }
        else
        {
            Debug.LogError("DialogueManager não encontrado.");
        }

        // Registra a gaiola como destruída
        if (StateManager.instance != null && uniqueID != null)
        {
            StateManager.instance.RegisterDestroyedObject(uniqueID.uniqueID);
        }
    }

    private void OnDialogueEnd()
    {
        // Remove o evento para evitar múltiplas chamadas
        dialogueManager.OnDialogueEnd -= OnDialogueEnd;

        // Verifica se é o cenário de Teo
        if (isTeoScene && Teo != null)
        {
            UniqueID teoUniqueID = Teo.GetComponent<UniqueID>();
            if (teoUniqueID != null && StateManager.instance != null)
            {
                StateManager.instance.RegisterDestroyedObject(teoUniqueID.uniqueID);
            }

            Destroy(Teo);

            // Adiciona Teo ao inventário
            InventarioManager.instance.AddItem(teoItemData);
        }
        // Verifica se é o cenário de Telma
        else if (isTelmaScene && Telma != null)
        {
            UniqueID telmaUniqueID = Telma.GetComponent<UniqueID>();
            if (telmaUniqueID != null && StateManager.instance != null)
            {
                StateManager.instance.RegisterDestroyedObject(telmaUniqueID.uniqueID);
            }

            Destroy(Telma);

            // Adiciona Telma ao inventário
            InventarioManager.instance.AddItem(telmaItemData);
        }

        // Destrói o GameObject da gaiola
        Destroy(gameObject);
    }
}