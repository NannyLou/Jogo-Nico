using UnityEngine;

public class GradeController : MonoBehaviour
{
    [Header("Configurações da Grade e Cadeado")]
    public GameObject cadeado;       // Objeto do cadeado
    public GameObject grade;         // Objeto da grade
    public GameObject seta;          // Objeto da seta (já deve estar desativado na cena)
    public ItemData.items chaveItem; // Item necessário para destrancar o cadeado

    [Header("Diálogo")]
    public string[] dialogLines;    // Linhas de diálogo ao destrancar

    private UniqueID cadeadoUniqueID; // UniqueID do cadeado
    private UniqueID gradeUniqueID;   // UniqueID da grade
    private DialogueManager dialogueManager;
    private MensagemManager mensagemManager;

    private void Awake()
    {
        // Obtém os UniqueIDs dos objetos
        if (cadeado != null)
            cadeadoUniqueID = cadeado.GetComponent<UniqueID>();

        if (grade != null)
            gradeUniqueID = grade.GetComponent<UniqueID>();
    }

    private void Start()
    {
        dialogueManager = DialogueManager.instance;
        mensagemManager = FindObjectOfType<MensagemManager>();

        // Verifica se o cadeado ou a grade já foram destruídos
        if (StateManager.instance != null)
        {
            if ((cadeadoUniqueID != null && StateManager.instance.IsObjectDestroyed(cadeadoUniqueID.uniqueID)) ||
                (gradeUniqueID != null && StateManager.instance.IsObjectDestroyed(gradeUniqueID.uniqueID)))
            {
                if (cadeado != null) Destroy(cadeado);
                if (grade != null) Destroy(grade);
                if (seta != null) seta.SetActive(true);
            }
        }
    }

    private void OnMouseDown()
    {
        if (InventarioManager.instance == null)
        {
            Debug.LogError("InventarioManager não encontrado.");
            return;
        }

        // Verifica se o jogador possui a chave
        if (InventarioManager.instance.selectedItemID == chaveItem)
        {
            // Remove a chave do inventário
            InventarioManager.instance.collectedItems.RemoveAll(item => item.itemID == chaveItem);
            InventarioManager.instance.UpdateEquipmentCanvas();
            InventarioManager.instance.SelectItem(-1);

            // Registra o cadeado e a grade como destruídos no StateManager
            if (StateManager.instance != null)
            {
                if (cadeadoUniqueID != null)
                    StateManager.instance.RegisterDestroyedObject(cadeadoUniqueID.uniqueID);

                if (gradeUniqueID != null)
                    StateManager.instance.RegisterDestroyedObject(gradeUniqueID.uniqueID);
            }

            // Destrói os objetos
            if (cadeado != null) Destroy(cadeado);
            if (grade != null) Destroy(grade);

            // Ativa a seta
            if (seta != null) seta.SetActive(true);

            // Inicia o diálogo
            if (dialogueManager != null && dialogLines.Length > 0)
            {
                dialogueManager.StartDialogue(dialogLines);
            }
        }
        else
        {
            // Exibe mensagem de área bloqueada
            if (mensagemManager != null)
            {
                mensagemManager.MostrarMensagem("Esta área está bloqueada. Você precisa de uma chave.");
            }
            else
            {
                Debug.LogWarning("MensagemManager não encontrado.");
            }
        }
    }
}
