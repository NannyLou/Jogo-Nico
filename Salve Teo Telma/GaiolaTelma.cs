using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaiolaTelma : MonoBehaviour
{
    public GameObject Telma;
    public Sprite openedGaiolaSprite; // Sprite da gaiola aberta (se tiver)

    private bool isOpen = false;
    private SpriteRenderer spriteRenderer;

    private UniqueID uniqueID; // Identificador único da gaiola
    private DialogueManager dialogueManager;
    public string[] dialogLines; // Linhas de diálogo definidas no Unity

    public ItemData telmaItemData; // ItemData associado à Telma
    public string cadeadoID; // UniqueID do cadeado para verificação no StateManager

    private void Awake()
    {
        uniqueID = GetComponent<UniqueID>();
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (StateManager.instance != null)
        {
            bool gaiolaDestruida = uniqueID != null && StateManager.instance.IsObjectDestroyed(uniqueID.uniqueID);
            bool cadeadoDestruido = !string.IsNullOrEmpty(cadeadoID) && StateManager.instance.IsObjectDestroyed(cadeadoID);

            if (gaiolaDestruida || cadeadoDestruido)
            {
                Destroy(gameObject);

                if (Telma != null)
                {
                    Destroy(Telma);
                }

                return;
            }
        }

        if (InventarioManager.instance.HasItem(telmaItemData.itemID))
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

        if (openedGaiolaSprite != null)
        {
            spriteRenderer.sprite = openedGaiolaSprite;
        }
        else
        {
            spriteRenderer.enabled = false;
        }

        if (dialogueManager != null)
        {
            if (dialogLines != null && dialogLines.Length > 0)
            {
                dialogueManager.StartDialogue(dialogLines);
                dialogueManager.OnDialogueEnd += OnDialogueEnd;
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

        if (StateManager.instance != null && uniqueID != null)
        {
            StateManager.instance.RegisterDestroyedObject(uniqueID.uniqueID);
        }
    }

    private void OnDialogueEnd()
    {
        dialogueManager.OnDialogueEnd -= OnDialogueEnd;

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

        Destroy(gameObject);
    }
}