using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintButtonController : MonoBehaviour
{
  public static HintButtonController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre as cenas
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private Button hintButton;

    private void Start()
    {
        hintButton = GetComponent<Button>();
        hintButton.onClick.AddListener(OnHintButtonClick);
    }

    private void OnHintButtonClick()
    {
        // Obter o cenário atual
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        // Verificar os itens e determinar qual diálogo mostrar
        string[] hintDialogue = GetHintDialogue(currentScene);

        // Iniciar o diálogo
        if (hintDialogue.Length > 0)
        {
            DialogueManager.instance.StartDialogue(hintDialogue);
        }
    }

    private string[] GetHintDialogue(string sceneName)
    {
        List<string> dialogueLines = new List<string>();

        // Exemplo de verificação de itens e cenas
        if (sceneName == "Cena1")
        {
            if (InventarioManager.instance.HasItem(ItemData.items.chaveZeca))
            {
                dialogueLines.Add("Zeca: Eita, de onde vc tirou essa chave?");
            }
            else
            {
                dialogueLines.Add("Zeca: Precisamos salvar nossos outros amigos animais!!!");
                dialogueLines.Add("Zeca: Eu ouvi a voz da telma vindo ali de cima, do lado da porta.");
            }
        }
    //  else if (sceneName == "Cena2")
    //  {
    //     if (InventarioManager.instance.HasItem(ItemData.items.chave))
    //     {
    //         dialogueLines.Add("Zeca: Salva a muie logo meu fi");
    //     }
    //     else
    //     {
    //         dialogueLines.Add("Zeca: macho, salva logo essa coitada, acha a chave ai.");
    //     }
    //  }
        return dialogueLines.ToArray();
    }
}
