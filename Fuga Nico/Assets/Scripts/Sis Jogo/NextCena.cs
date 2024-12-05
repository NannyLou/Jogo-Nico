using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextCena : MonoBehaviour
{
    public string lvlName; // Nome da cena a ser carregada
    public Vector2 playerStartingPosition; // Posição inicial do jogador na nova cena

    [Header("Condições de Acesso")]
    public bool requiresHintButton; // Se a seta requer que o HintButton esteja ativo
    public ItemData.items requiredItem; // Item necessário no inventário (opcional)
    public ItemData.items[] alternativeItems; // Lista de itens alternativos
    public bool requiresItem = false; // Define se é necessário verificar o inventário
    public bool blockIfHasItem = false; // Bloqueia se o jogador tiver o item no inventário

    private UniqueID uniqueID; // Identificador único da seta
    private bool hasAccess = false; // Controle de acesso à cena

    private MensagemManager mensagemManager; // Referência ao MensagemManager

    private void Awake()
    {
        uniqueID = GetComponent<UniqueID>();
    }

    private void Start()
    {
        mensagemManager = FindObjectOfType<MensagemManager>();

        // Verifica se a cena já foi acessada
        if (StateManager.instance != null && uniqueID != null)
        {
            hasAccess = StateManager.instance.IsObjectDestroyed(uniqueID.uniqueID);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Se já foi acessada anteriormente, concede acesso diretamente
            if (hasAccess)
            {
                LoadScene();
                return;
            }

            // Caso contrário, verifica as condições de acesso
            if (!CheckAccessConditions())
            {
                // Exibe mensagem de erro usando o MensagemManager
                if (mensagemManager != null)
                {
                    mensagemManager.MostrarMensagem("Você não pode acessar esta área ainda.");
                }
                return;
            }

            // Marca a cena como acessada
            if (StateManager.instance != null && uniqueID != null)
            {
                StateManager.instance.RegisterDestroyedObject(uniqueID.uniqueID);
                hasAccess = true; // Atualiza o estado local
            }

            // Carrega a nova cena
            LoadScene();
        }
    }

   private bool CheckAccessConditions()
{
    // Verifica se o HintButton está ativo, se necessário
    if (requiresHintButton)
    {
        HintButtonController hintButtonController = FindObjectOfType<HintButtonController>();
        if (hintButtonController == null || !hintButtonController.gameObject.activeInHierarchy)
        {
            Debug.Log("HintButton não está ativo.");
            return false;
        }
    }

    // Verifica se o jogador possui o item necessário ou um item alternativo
    if (requiresItem)
    {
        // Linha nova: Verifica o item principal
        bool hasRequiredItem = InventarioManager.instance != null &&
                               InventarioManager.instance.HasItem(requiredItem);

        // Linhas novas: Verifica itens alternativos
        bool hasAlternativeItem = false;
        if (alternativeItems != null && alternativeItems.Length > 0)
        {
            foreach (var item in alternativeItems)
            {
                if (InventarioManager.instance != null && InventarioManager.instance.HasItem(item))
                {
                    hasAlternativeItem = true;
                    break;
                }
            }
        }

        // Linha nova: Combina condições para determinar acesso
        if (!hasRequiredItem && !hasAlternativeItem)
        {
            Debug.Log("Nenhum dos itens necessários foi encontrado no inventário.");
            return false;
        }
    }

    // Verifica se o jogador **não deve ter** um item
    if (blockIfHasItem)
    {
        if (InventarioManager.instance != null && 
            InventarioManager.instance.HasItem(requiredItem))
        {
            Debug.Log("O jogador possui o item bloqueador.");
            return false;
        }
    }

    // Todas as condições foram atendidas
    return true;
}
    private void LoadScene()
    {
        // Define a posição inicial no GameManager
        GameManager.playerStartPosition = playerStartingPosition;

        // Carrega a nova cena
        SceneManager.LoadScene(lvlName);
    }
}
