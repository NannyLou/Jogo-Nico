using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InventarioManager : MonoBehaviour
{
    public static InventarioManager instance;
    // Lista de itens atualmente no inventário
    public List<ItemData> collectedItems = new List<ItemData>();

    // Nova lista para rastrear os IDs dos itens já coletados
    public List<ItemData.items> collectedItemIDs = new List<ItemData.items>();

    [Header("UI do Inventário")]
    public GameObject equipmentCanvas;               // UI do inventário
    public Image[] equipmentSlots, equipmentImages;  // Slots de inventário e imagens dos itens
    public Sprite emptyItemSlotSprite;               // Sprite para slots vazios
    public Color defaultSlotColor = Color.white;     // Cor padrão dos slots
    public Color selectedItemColor = Color.yellow;   // Cor do slot selecionado
    public int selectedCanvasSlotID = -1;            // ID do slot atualmente selecionado
    public ItemData.items selectedItemID = ItemData.items.none;  // ID do item atualmente selecionado

    public bool HasItem(ItemData.items itemID)
    {
        foreach (ItemData item in collectedItems)
        {
            if (item.itemID == itemID)
                return true;
        }
        return false;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Mantém o InventarioManager entre cenas

            // Torna o EquipmentCanvas filho do InventarioManager
            if (equipmentCanvas != null)
            {
                DontDestroyOnLoad(equipmentCanvas);
                equipmentCanvas.transform.SetParent(this.transform);
            }

            // Inscreve-se no evento de cena carregada
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            // Se já existir uma instância, destrua o Canvas duplicado e este GameObject
            if (equipmentCanvas != null)
            {
                Destroy(equipmentCanvas);
            }
            Destroy(gameObject);
        }
    }

    // Método chamado quando uma nova cena é carregada
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Se a cena for "CenaFinalCre" ou "Menu", desativa o inventário
        if (scene.name == "CenaFinalCre" || scene.name == "Menu")
        {
            if (equipmentCanvas != null)
            {
                equipmentCanvas.SetActive(false);
            }
        }
        else
        {
            if (equipmentCanvas != null)
            {
                equipmentCanvas.SetActive(true);
            }
        }

        // Atualizar o inventário na nova cena
        UpdateEquipmentCanvas();
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
        if (equipmentCanvas == null || !equipmentCanvas.activeSelf)
            return;

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

    public void AddItem(ItemData item)
    {
        if (item == null)
            return;

        // Adiciona o item ao inventário
        collectedItems.Add(item);

        // Adiciona o ID do item à lista de IDs coletados, se ainda não estiver presente
        if (!collectedItemIDs.Contains(item.itemID))
        {
            collectedItemIDs.Add(item.itemID);
        }

        // Atualiza a UI do inventário
        UpdateEquipmentCanvas();
    }

    private void OnDestroy()
    {
        // Desinscreve-se do evento ao destruir o InventarioManager
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}