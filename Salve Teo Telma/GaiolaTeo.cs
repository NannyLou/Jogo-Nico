using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaiolaTeo : MonoBehaviour
{
    public GameObject Teo;
    public Sprite openedGaiolaSprite; // Sprite da gaiola aberta (se tiver)

    private bool isOpen = false;
    private SpriteRenderer spriteRenderer;

    private UniqueID uniqueID; // Identificador único da gaiola
    private DialogueManager dialogueManager;
    public string[] dialogLines; // Linhas de diálogo definidas no Unity

    public ItemData teoItemData;   // ItemData associado ao Teo
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

                return;
            }
        }

        // Verifica se o item já está no inventário
        if (InventarioManager.instance.HasItem(teoItemData.itemID))
        {
            if (Teo != null)
                Destroy(Teo);
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
        dialogueManager.OnDialogueEnd -= OnDialogueEnd;

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

        Destroy(gameObject);
    }
}