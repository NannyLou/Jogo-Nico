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

    [Header("Item Bloqueador")]
    public ItemData.items blockingItem; // Item que pode bloquear o acesso
    public ItemData.items[] specificBlockingItems; // Itens específicos para bloquear a passagem
    public string customBlockingMessage; // Mensagem personalizada quando os itens específicos bloqueiam

    [Header("Verificação de Objeto Destruído")]
    public GameObject copo; // Referência ao objeto "copo"

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
            if (hasAccess)
            {
                // Mesmo tendo acessado antes, verifica se o jogador está bloqueado agora
                if (IsBlocked())
                {
                    // Exibe mensagem de erro usando o MensagemManager
                    if (mensagemManager != null)
                    {
                        mensagemManager.MostrarMensagem("Você não pode acessar esta área agora.");
                    }
                    return;
                }

                // Permite acessar a cena
                LoadScene();
                return;
            }

            // Caso contrário, verifica as condições de acesso normalmente
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
            bool hasRequiredItem = InventarioManager.instance != null &&
                                   InventarioManager.instance.HasItem(requiredItem);

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

            bool hasRequiredOrAlternative = hasRequiredItem || hasAlternativeItem;

            if (!hasRequiredOrAlternative)
            {
                Debug.Log("Nenhum dos itens necessários foi encontrado no inventário.");
                // Se não possui itens necessários, mas tem um item bloqueador, bloqueia
                if (blockingItem != ItemData.items.none && InventarioManager.instance != null &&
                    InventarioManager.instance.HasItem(blockingItem))
                {
                    Debug.Log("O jogador possui o item bloqueador.");
                    return false;
                }

                // Se não possui itens necessários e não há item bloqueador, ainda bloqueia o acesso
                Debug.Log("Acesso bloqueado: não possui itens necessários nem item bloqueador.");
                return false;
            }
        }

        // Verifica se o jogador possui os itens específicos bloqueadores (2 itens)
        if (specificBlockingItems != null && specificBlockingItems.Length == 2)
        {
            bool hasItem1 = InventarioManager.instance != null &&
                             InventarioManager.instance.HasItem(specificBlockingItems[0]);
            bool hasItem2 = InventarioManager.instance != null &&
                             InventarioManager.instance.HasItem(specificBlockingItems[1]);

            if (hasItem1 && hasItem2)
            {
                Debug.Log("Acesso bloqueado: o jogador possui ambos os itens bloqueadores.");

                // Exibe a mensagem personalizada se configurada
                if (!string.IsNullOrEmpty(customBlockingMessage))
                {
                    if (mensagemManager != null)
                    {
                        mensagemManager.MostrarMensagem(customBlockingMessage);
                    }
                }
                return false; // Bloqueia se ambos os itens estiverem presentes
            }
        }

        // Verifica se o jogador possui o item bloqueador **apenas** se não possui o item necessário ou alternativo
        if (blockingItem != ItemData.items.none)
        {
            bool hasBlockingItem = InventarioManager.instance != null &&
                                   InventarioManager.instance.HasItem(blockingItem);

            if (hasBlockingItem)
            {
                bool hasRequiredOrAlternative = false;
                if (requiresItem)
                {
                    bool hasRequiredItem = InventarioManager.instance.HasItem(requiredItem);

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

                    hasRequiredOrAlternative = hasRequiredItem || hasAlternativeItem;
                }

                if (!hasRequiredOrAlternative)
                {
                    Debug.Log("O jogador possui o item bloqueador.");
                    return false;
                }
            }
        }

        // Verifica se o objeto "copo" foi destruído
        if (copo != null && StateManager.instance != null)
        {
            UniqueID copoUniqueID = copo.GetComponent<UniqueID>();
            if (copoUniqueID != null && !StateManager.instance.IsObjectDestroyed(copoUniqueID.uniqueID))
            {
                Debug.Log("O objeto 'copo' ainda não foi destruído. Acesso bloqueado.");
                return false;
            }
        }

        // Todas as condições foram atendidas
        return true;
    }

    private bool IsBlocked()
    {
        if (blockingItem == ItemData.items.none)
            return false;

        bool hasBlockingItem = InventarioManager.instance != null &&
                               InventarioManager.instance.HasItem(blockingItem);

        if (hasBlockingItem)
        {
            bool hasRequiredOrAlternative = false;
            if (requiresItem)
            {
                bool hasRequiredItem = InventarioManager.instance.HasItem(requiredItem);

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
                hasRequiredOrAlternative = hasRequiredItem || hasAlternativeItem;
            }

            if (!hasRequiredOrAlternative)
            {
                Debug.Log("Bloqueado: possui blockingItem sem requiredItem ou alternativeItems.");
                return true; // Acesso bloqueado
            }
        }

        return false; // Não está bloqueado
    }

    private void LoadScene()
    {
        // Define a posição inicial no GameManager
        GameManager.playerStartPosition = playerStartingPosition;

        // Carrega a nova cena
        SceneManager.LoadScene(lvlName);
    }
}
