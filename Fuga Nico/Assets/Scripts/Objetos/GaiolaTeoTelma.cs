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
                Destroy(gameObject);

                if (Teo != null)
                {
                    Destroy(Teo);
                }

                if (Telma != null)
                {
                    Destroy(Telma);
                }

                return;
            }
        }

        // Verifica se os itens já estão no inventário
        if (InventarioManager.instance.HasItem(teoItemData.itemID) && Teo != null)
        {
            Destroy(Teo);
        }

        if (InventarioManager.instance.HasItem(telmaItemData.itemID) && Telma != null)
        {
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

        // Adiciona Teo ao inventário e destrói o objeto, se existir
        if (Teo != null)
        {
            UniqueID teoUniqueID = Teo.GetComponent<UniqueID>();
            if (teoUniqueID != null && StateManager.instance != null)
            {
                StateManager.instance.RegisterDestroyedObject(teoUniqueID.uniqueID);
            }

            Destroy(Teo);

            InventarioManager.instance.AddItem(teoItemData);
        }

        // Adiciona Telma ao inventário e destrói o objeto, se existir
        if (Telma != null)
        {
            UniqueID telmaUniqueID = Telma.GetComponent<UniqueID>();
            if (telmaUniqueID != null && StateManager.instance != null)
            {
                StateManager.instance.RegisterDestroyedObject(telmaUniqueID.uniqueID);
            }

            Destroy(Telma);

            InventarioManager.instance.AddItem(telmaItemData);
        }

        // Destrói o GameObject da gaiola
        Destroy(gameObject);
    }
}