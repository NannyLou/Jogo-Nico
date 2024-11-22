using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public event Action OnDialogueEnd;
    public GameObject dialogPanel;        // Painel do diálogo
    public TextMeshProUGUI dialogText;    // Texto do diálogo

    private Queue<string> sentences;      // Fila de frases do diálogo
    private bool isDialogueActive = false; // Controla se o diálogo está ativo

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

        // Certifica-se de que o painel e o texto começam desativados
        dialogPanel.SetActive(false);
        dialogText.gameObject.SetActive(false);
    }

    public void StartDialogue(string[] dialogLines)
    {
        if (isDialogueActive)
            return;

        isDialogueActive = true;

        // Ativa o painel e o texto do diálogo no início do diálogo
        dialogPanel.SetActive(true);
        dialogText.gameObject.SetActive(true);

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
        dialogText.text = ""; // Limpa o texto antes de começar a digitar

        foreach (char letter in sentence.ToCharArray())
        {
            dialogText.text += letter;
            yield return null; // Ajuste a velocidade de digitação se desejar
        }
    }

    public void EndDialogue()
    {
        dialogPanel.SetActive(false);
        dialogText.gameObject.SetActive(false);
        isDialogueActive = false;

        // Invoca o evento de fim de diálogo
        if (OnDialogueEnd != null)
        {
            OnDialogueEnd.Invoke();
        }
    }


    void Update()
    {
        // Avança o diálogo ao clicar com o mouse
        if (isDialogueActive && Input.GetMouseButtonDown(0))
        {
            DisplayNextSentence();
        }
    }
}
