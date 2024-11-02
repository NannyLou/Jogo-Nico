using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public GameObject dialogPanel;
    public TextMeshProUGUI dialogText; // Usando TextMeshProUGUI
    private Queue<string> sentences;
    private bool isDialogueActive = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        sentences = new Queue<string>();
        dialogPanel.SetActive(false);
    }

    public void StartDialogue(string[] dialogLines)
    {
        if (isDialogueActive)
            return;

        isDialogueActive = true;
        dialogPanel.SetActive(true);
        sentences.Clear();

        foreach (string line in dialogLines)
        {
            sentences.Enqueue(line);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogText.text += letter;
            yield return null; // Você pode ajustar a velocidade aqui
        }
    }

    public void EndDialogue()
    {
        dialogPanel.SetActive(false);
        isDialogueActive = false;
    }

    void Update()
    {
        if (isDialogueActive && Input.GetMouseButtonDown(0))
        {
            DisplayNextSentence();
        }
    }
}
