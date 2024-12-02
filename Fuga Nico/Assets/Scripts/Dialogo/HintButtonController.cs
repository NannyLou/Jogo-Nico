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
            bool hasAnyItem = false;

            if (InventarioManager.instance.HasItem(ItemData.items.chaveTeo))
            {
                dialogueLines.Add("Zeca: Ta querendo dica até agora?");
                dialogueLines.Add("Zeca: Melhor eu jogar por ti então");
                dialogueLines.Add("Nico: Poxa vida, papagaio grosseiro.");
                hasAnyItem = true;
            }
            if (InventarioManager.instance.HasItem(ItemData.items.remedio))
            {
                dialogueLines.Add("Zeca: Talvez dê para entrar na casa agora.");
                hasAnyItem = true;
            }
            if (InventarioManager.instance.HasItem(ItemData.items.feno))
            {
                dialogueLines.Add("Zeca: Ta sentindo esse cheiro?");
                dialogueLines.Add("Zeca: O Valdivino deve estar cozinhando algo na cozinha.");
                hasAnyItem = true;
            }
            if (InventarioManager.instance.HasItem(ItemData.items.chaveTelma))
            {
                dialogueLines.Add("Zeca: Vamos na Telma testar a chave que estava com o Valdivino");
                hasAnyItem = true;
            }
            if (InventarioManager.instance.HasItem(ItemData.items.telma))
            {
                dialogueLines.Add("Zeca: Bora rapido atrás da chave da saida");
                hasAnyItem = true;
            }
            if (!hasAnyItem)
            {
                dialogueLines.Add("Zeca: Precisamos salvar nossos outros amigos animais!!!");
                dialogueLines.Add("Zeca: Eu ouvi a voz da Telma vindo ali de cima, do lado da porta.");
            }
        }
        else if (sceneName == "Cena2")
        {
            bool hasAnyItem = false;

            if (InventarioManager.instance.HasItem(ItemData.items.teo))
            {
                dialogueLines.Add("Zeca: Talvez o Teo possa ajudar a quebrar essa porta.");
                hasAnyItem = true;
            }
            if (InventarioManager.instance.HasItem(ItemData.items.chaveTeo))
            {
                dialogueLines.Add("Zeca: A chave não parece funcionar na porta.");
                hasAnyItem = true;
            }
            if (InventarioManager.instance.HasItem(ItemData.items.feno))
            {
                dialogueLines.Add("Zeca: Não acho que tenha nada para fazer aqui por enquanto");
                hasAnyItem = true;
            }
            if (InventarioManager.instance.HasItem(ItemData.items.chaveTelma))
            {
                dialogueLines.Add("Zeca: Talvez o essa chave nova sirva na gaiola da Telma.");
                hasAnyItem = true;
            }
            if (InventarioManager.instance.HasItem(ItemData.items.telma))
            {
                dialogueLines.Add("Zeca: Vamos tentar ligar para o Valdivino usando o celular do Daniel");
                hasAnyItem = true;
            }
            if (!hasAnyItem)
            {
                dialogueLines.Add("Zeca: Seria bom alguém forte para DESTRUIR essa porta.");
                dialogueLines.Add("Nico: Quanta agressividade...");
            }
        }
        else if (sceneName == "Cena4")
        {
            bool hasAnyItem = false;
            if (InventarioManager.instance.HasItem(ItemData.items.remedio))
            {
                dialogueLines.Add("Zeca: Esse remédio parece ser bem forte.");
                dialogueLines.Add("Zeca: Um desse e a pessoa não acorda mais.");
                hasAnyItem = true;
            }
            if (InventarioManager.instance.HasItem(ItemData.items.chaveTeo))
            {
                dialogueLines.Add("Zeca: Testa essa Chave aqui.");
                hasAnyItem = true;
            }
            if (!hasAnyItem)
            {
                dialogueLines.Add("Zeca: Deve ter algo para pegar aqui...");
            }
        }
        else if (sceneName == "Cena3")
        {
            bool hasAnyItem = false;

            if (InventarioManager.instance.HasItem(ItemData.items.remedio))
            {
                dialogueLines.Add("Zeca: Olha Nico, o Daniel está distraído lendo o jornal.");
                dialogueLines.Add("Zeca: Momento perfeito para botar ele pra dormir.");
                hasAnyItem = true;
            }
            if (InventarioManager.instance.HasItem(ItemData.items.feno))
            {
                dialogueLines.Add("Zeca: Cheiro de ensopado de mucunza.");
                dialogueLines.Add("Zeca: Delícia!");
                hasAnyItem = true;
            }
            if (InventarioManager.instance.HasItem(ItemData.items.chaveTeo))
            {
                dialogueLines.Add("Zeca: O que será que essa chave nova que você achou abre?");
                dialogueLines.Add("Nico: Espero abra a porta da Telma.");
                dialogueLines.Add("Zeca: Aquela porta ali só abre quebrando.");
                dialogueLines.Add("Nico: Quanta convicção.");
                hasAnyItem = true;
            }
            if (InventarioManager.instance.HasItem(ItemData.items.telma))
            {
                dialogueLines.Add("Zeca: Quase lá");
                hasAnyItem = true;
            }
            if (!hasAnyItem)
            {
                dialogueLines.Add("Zeca: Bora ver se o Daniel dormiu mesmo.");
            }
        }
        else if (sceneName == "Cena3_1")
        {
            bool hasAnyItem = false;
            if (InventarioManager.instance.HasItem(ItemData.items.telma))
            {
                dialogueLines.Add("Zeca: Pare de enrolar, não tá vendo que o homi tá capotado?");
                hasAnyItem = true;
            }
            if (InventarioManager.instance.HasItem(ItemData.items.chaveTelma))
            {
                dialogueLines.Add("Zeca: Vamos na Telma testar a chave que estava com o Valdivino");
                hasAnyItem = true;
            }
            if (!hasAnyItem)
            {
                dialogueLines.Add("Zeca: Oh caba atrapalhado, deixou tudo jogado.");
            }
        }
        else if (sceneName == "Cena2_1")
        {
            bool hasAnyItem = false;
            if (InventarioManager.instance.HasItem(ItemData.items.feno))
            {
                dialogueLines.Add("Zeca: Feno me lembra fogo...");
                dialogueLines.Add("Zeca: EU ODEIO FOGO, JOGA FORA ISSO!!!");
                hasAnyItem = true;
            }
            if (InventarioManager.instance.HasItem(ItemData.items.chaveTelma))
            {
                dialogueLines.Add("Zeca: Testa a chave na gaiola da Telma para eu ver um negocio.");
                hasAnyItem = true;
            }
            if (InventarioManager.instance.HasItem(ItemData.items.telma))
            {
                dialogueLines.Add("Zeca: Vamos la fora!");
                hasAnyItem = true;
            }
            if (!hasAnyItem)
            {
                dialogueLines.Add("Zeca: Deve ter algo para pegar aqui...");
            }
        }
        else if (sceneName == "Cena3_2")
        {
            bool hasAnyItem = false;
            if (InventarioManager.instance.HasItem(ItemData.items.feno))
            {
                dialogueLines.Add("Zeca: Tenta distrair o Valdivino de alguma forma.");
                dialogueLines.Add("Zeca: O cozido dele parece estar quase no ponto.");
                dialogueLines.Add("Zeca: Que fomee.");
                hasAnyItem = true;
            }
            if (InventarioManager.instance.HasItem(ItemData.items.chaveTelma))
            {
                dialogueLines.Add("Zeca: Uma chave nova!!");
                hasAnyItem = true;
            }
            if (InventarioManager.instance.HasItem(ItemData.items.telma))
            {
                dialogueLines.Add("Zeca: A Telma deve conseguir alcançar a chave com a lingua");
                hasAnyItem = true;
            }
            if (!hasAnyItem)
            {
                dialogueLines.Add("Zeca: Deve ter algo para pegar aqui...");
            }
        }

        return dialogueLines.ToArray();
    }
}
