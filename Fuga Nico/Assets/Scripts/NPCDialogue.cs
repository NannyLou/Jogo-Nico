using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [TextArea(3, 10)]
    public string[] dialogueLines;

    private void OnMouseDown()
    {
        if (DialogueManager.instance == null)
        {
            Debug.LogError("DialogueManager não encontrado na cena.");
            return;
        }

        if (dialogueLines.Length == 0)
        {
            Debug.LogWarning("Nenhuma linha de diálogo atribuída no NPCDialogue.");
            return;
        }

        DialogueManager.instance.StartDialogue(dialogueLines);
    }
}
