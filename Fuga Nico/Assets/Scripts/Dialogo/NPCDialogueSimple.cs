using UnityEngine;

public class NPCDialogueSimple : MonoBehaviour
{
    [TextArea(3, 10)]
    public string[] dialogueLinesWhenTrapped; // Diálogo configurado no Inspector

    private void OnMouseDown()
    {
        if (DialogueManager.instance == null)
        {
            Debug.LogError("DialogueManager não encontrado na cena.");
            return;
        }

        // Verifica se há linhas de diálogo configuradas
        if (dialogueLinesWhenTrapped != null && dialogueLinesWhenTrapped.Length > 0)
        {
            DialogueManager.instance.StartDialogue(dialogueLinesWhenTrapped);
        }
        else
        {
            Debug.LogWarning("As linhas de diálogo não foram definidas para " + gameObject.name);
        }
    }
}
