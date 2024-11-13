using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [TextArea(3, 10)]
    public string[] dialogueLinesWhenTrapped; // Diálogo quando está preso
    [TextArea(3, 10)]
    public string[] dialogueLinesWhenFree;    // Diálogo quando está livre

    public GameObject hintButton; // Referência ao botão de dica

    private bool isFree = false; // Estado do Zeca

    private void OnMouseDown()
    {
        if (isFree)
        {
            // O Zeca já está livre e não deve responder a cliques
            return;
        }

        if (DialogueManager.instance == null)
        {
            Debug.LogError("DialogueManager não encontrado na cena.");
            return;
        }

        if (!isFree)
        {
            // Iniciar diálogo quando está preso
            if (dialogueLinesWhenTrapped.Length > 0)
            {
                DialogueManager.instance.StartDialogue(dialogueLinesWhenTrapped);
            }
        }
    }

    // Método para iniciar o diálogo automaticamente após ser libertado
    public void StartFreeDialogue()
    {
        isFree = true;

        if (DialogueManager.instance == null)
        {
            Debug.LogError("DialogueManager não encontrado na cena.");
            return;
        }

        if (dialogueLinesWhenFree.Length > 0)
        {
            // Inscrever-se no evento de fim de diálogo
            DialogueManager.instance.OnDialogueEnd += OnFreeDialogueEnd;

            // Iniciar o diálogo
            DialogueManager.instance.StartDialogue(dialogueLinesWhenFree);
        }
        else
        {
            // Se não houver diálogo, chama diretamente o método de fim
            OnFreeDialogueEnd();
        }
    }

    // Método chamado quando o diálogo termina
    private void OnFreeDialogueEnd()
    {
        // Desinscrever-se do evento para evitar chamadas duplicadas
        DialogueManager.instance.OnDialogueEnd -= OnFreeDialogueEnd;

        // Destruir o GameObject do Zeca
        Destroy(gameObject);

        // Ativar o botão de dica
        if (hintButton != null)
        {
            hintButton.SetActive(true);
        }
        else
        {
            Debug.LogError("HintButton não está atribuído no NPCDialogue.");
        }
    }
}
