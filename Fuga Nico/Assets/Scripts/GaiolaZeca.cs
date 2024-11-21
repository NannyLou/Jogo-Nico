using UnityEngine;

public class GaiolaZeca : MonoBehaviour
{
    public GameObject Zeca;
    public ItemData.items requiredItem = ItemData.items.chave; // Item necessário para abrir a gaiola
    private bool isOpen = false;

    public Sprite openedGaiolaSprite; // Sprite da gaiola aberta (se tiver)

    private SpriteRenderer spriteRenderer;

    private MensagemManager mensagemManager;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mensagemManager = FindObjectOfType<MensagemManager>();
    }

    private void OnMouseDown()
    {
        if (!isOpen)
        {
            if (InventarioManager.instance.selectedItemID == requiredItem)
            {
                OpenGaiola();

                // Remover a chave do inventário, se desejar
                InventarioManager.instance.collectedItems.RemoveAll(item => item.itemID == requiredItem);
                InventarioManager.instance.UpdateEquipmentCanvas();
                InventarioManager.instance.SelectItem(-1);
            }
            else
            {
                // Exibir mensagem na tela
                mensagemManager.MostrarMensagem("A gaiola está trancada. Você precisa de uma chave.");
            }
        }
    }

    private void OpenGaiola()
    {
        isOpen = true;
        // Se tiver um sprite de gaiola aberta, altere para ele
        if (openedGaiolaSprite != null)
        {
            spriteRenderer.sprite = openedGaiolaSprite;
        }
        else
        {
            // Se não, desative o sprite atual para sumir as grades
            spriteRenderer.enabled = false;
        }
        NPCDialogue zecaDialogue = Zeca.GetComponent<NPCDialogue>();
        if (zecaDialogue != null)
        {
            zecaDialogue.StartFreeDialogue();
        }
        else
        {
            Debug.LogError("NPCDialogue não encontrado no Zeca.");
        }

        // Destruir a gaiola
        Destroy(gameObject);
    }
}
