using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventarioManager : MonoBehaviour
{
    public static InventarioManager instance;

    // Lista de itens coletados pelo jogador
    public static List<ItemData> collectedItems = new List<ItemData>();

    [Header("UI do Inventário")]
    public GameObject equipmentCanvas;               // UI do inventário
    public Image[] equipmentSlots, equipmentImages;  // Slots de inventário e imagens dos itens
    public Sprite emptyItemSlotSprite;               // Sprite para slots vazios
    public Color defaultSlotColor = Color.white;     // Cor padrão dos slots
    public Color selectedItemColor = Color.yellow;   // Cor do slot selecionado
    public int selectedCanvasSlotID = -1;            // ID do slot atualmente selecionado
    public ItemData.items selectedItemID = ItemData.items.none;  // ID do item atualmente selecionado

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Opcional: mantém o InventarioManager entre cenas
            equipmentCanvas.SetActive(true);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Função para selecionar um item no inventário
    public void SelectItem(int equipmentCanvasID)
    {
        // Remove a seleção do slot anterior
        if (selectedCanvasSlotID >= 0 && selectedCanvasSlotID < equipmentSlots.Length)
        {
            equipmentSlots[selectedCanvasSlotID].color = defaultSlotColor;
        }

        // Verifica se o slot clicado é válido
        if (equipmentCanvasID >= 0 && equipmentCanvasID < collectedItems.Count)
        {
            // Define a cor do novo slot selecionado e atualiza os IDs selecionados
            equipmentSlots[equipmentCanvasID].color = selectedItemColor;
            selectedCanvasSlotID = equipmentCanvasID;
            selectedItemID = collectedItems[selectedCanvasSlotID].itemID;
        }
        else
        {
            // Se o slot for inválido, deseleciona
            selectedItemID = ItemData.items.none;
            selectedCanvasSlotID = -1;
        }
    }

    // Atualiza o inventário na UI
    public void UpdateEquipmentCanvas()
    {
        int itemsAmount = collectedItems.Count;
        int itemSlotsAmount = equipmentSlots.Length;

        // Atualiza cada slot com o sprite do item correspondente ou um sprite vazio
        for (int i = 0; i < itemSlotsAmount; i++)
        {
            if (i < itemsAmount && collectedItems[i].itemSlotSprite != null)
            {
                equipmentImages[i].sprite = collectedItems[i].itemSlotSprite;
                equipmentSlots[i].color = defaultSlotColor; // Reseta a cor do slot
            }
            else
            {
                equipmentImages[i].sprite = emptyItemSlotSprite;
                equipmentSlots[i].color = defaultSlotColor; // Reseta a cor do slot
            }
        }

        // Seleciona automaticamente um item se o inventário tiver apenas um
        if (itemsAmount == 0)
            SelectItem(-1);
        else if (itemsAmount == 1)
            SelectItem(0);
        else
        {
            // Se houver mais de um item, não seleciona nenhum por padrão
            SelectItem(-1);
        }
    }
}
