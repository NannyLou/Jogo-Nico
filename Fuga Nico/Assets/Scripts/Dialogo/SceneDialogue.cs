using System.Collections;
using UnityEngine;

public class SceneDialogue : MonoBehaviour
{
    [Header("Configurações do Diálogo")]
    public string uniqueID; // Identificador único para este diálogo no StateManager
    public string[] dialogueLines; // Linhas do diálogo a serem exibidas

    [Header("Condição para Mostrar Diálogo")]
    public bool requireItem = false; // Se ativado, o diálogo só aparecerá se o jogador tiver o item
    public ItemData.items requiredItem; // Item necessário para iniciar o diálogo

    private void Start()
    {
        // Se não for necessário um item, ou se o jogador possui o item requerido, inicie o diálogo
        if (!requireItem || (InventarioManager.instance != null && InventarioManager.instance.HasItem(requiredItem)))
        {
            // Verifica se o diálogo já foi mostrado
            if (StateManager.instance != null && StateManager.instance.IsObjectDestroyed(uniqueID))
            {
                // O diálogo já foi exibido antes; nada acontece
                return;
            }

            // Inicia uma coroutine para aguardar antes de exibir o diálogo
            StartCoroutine(StartDialogueAfterDelay(0.01f)); // Aguarda 1 segundo
        }
    }

    private IEnumerator StartDialogueAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.OnDialogueEnd += HandleDialogueEnd;
            DialogueManager.instance.StartDialogue(dialogueLines);
        }
        else
        {
            Debug.LogError("DialogueManager não foi encontrado na cena!");
        }
    }

    private void HandleDialogueEnd()
    {
        // Desinscreve-se do evento
        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.OnDialogueEnd -= HandleDialogueEnd;
        }

        // Registra que o diálogo foi exibido
        if (StateManager.instance != null)
        {
            StateManager.instance.RegisterDestroyedObject(uniqueID);
        }
    }
}
