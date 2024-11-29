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

    private void Awake()
    {
        uniqueID = GetComponent<UniqueID>();
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Verifica se a gaiola já foi destruída
        if (StateManager.instance != null && uniqueID != null)
        {
            if (StateManager.instance.IsObjectDestroyed(uniqueID.uniqueID))
            {
                Destroy(gameObject);
                return;
            }
        }

        // Verifica se o Teo já foi removido
        if (Teo != null && StateManager.instance != null)
        {
            UniqueID teoUniqueID = Teo.GetComponent<UniqueID>();
            if (teoUniqueID != null && StateManager.instance.IsObjectDestroyed(teoUniqueID.uniqueID))
            {
                Destroy(Teo);
            }
        }

        // Verifica se a Telma já foi removida
        if (Telma != null && StateManager.instance != null)
        {
            UniqueID telmaUniqueID = Telma.GetComponent<UniqueID>();
            if (telmaUniqueID != null && StateManager.instance.IsObjectDestroyed(telmaUniqueID.uniqueID))
            {
                Destroy(Telma);
            }
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

        // Inicia o diálogo com o Teo ou Telma
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

        // Remove Teo e Telma da cena
        if (Teo != null)
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

        if (Telma != null)
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

        // Destrói o GameObject da gaiola, se desejar
        Destroy(gameObject);
    }
}
