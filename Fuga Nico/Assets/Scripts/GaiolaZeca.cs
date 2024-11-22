using UnityEngine;

public class GaiolaZeca : MonoBehaviour
{
    public GameObject Zeca;
    public Sprite openedGaiolaSprite; // Sprite da gaiola aberta (se tiver)

    private bool isOpen = false;
    private SpriteRenderer spriteRenderer;

    private UniqueID uniqueID; // Identificador único da gaiola

    private void Awake()
    {
        uniqueID = GetComponent<UniqueID>();
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Verifica se a gaiola já foi destruída
        if (StateManager.instance != null && uniqueID != null)
        {
            if (StateManager.instance.IsObjectDestroyed(uniqueID.uniqueID))
            {
                // A gaiola já foi destruída; ativamos o hintButton se necessário
                HintButtonController hintButtonController = FindObjectOfType<HintButtonController>();
                if (hintButtonController != null)
                {
                    hintButtonController.gameObject.SetActive(true);
                }

                Destroy(gameObject);
                return;
            }
        }
    }

    // Remova o método OnMouseDown(), pois a interação será feita no cadeado

    public void OpenGaiola()
    {
        if (isOpen)
            return;

        isOpen = true;

        // Se tiver um sprite de gaiola aberta, altera para ele
        if (openedGaiolaSprite != null)
        {
            spriteRenderer.sprite = openedGaiolaSprite;
        }
        else
        {
            // Se não, desativa o sprite atual para sumir as grades
            spriteRenderer.enabled = false;
        }

        // Inicia o diálogo com o Zeca
        NPCDialogue zecaDialogue = Zeca.GetComponent<NPCDialogue>();
        if (zecaDialogue != null)
        {
            zecaDialogue.StartFreeDialogue();
        }
        else
        {
            Debug.LogError("NPCDialogue não encontrado no Zeca.");
        }

        // Registra a gaiola como destruída
        if (StateManager.instance != null && uniqueID != null)
        {
            StateManager.instance.RegisterDestroyedObject(uniqueID.uniqueID);
        }

        // Destrói o GameObject da gaiola, se desejar
        Destroy(gameObject);
    }
}
