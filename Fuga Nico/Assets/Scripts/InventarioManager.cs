using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventarioManager : MonoBehaviour
{
    public static InventarioManager instance;
    // Lista de itens coletados pelo jogador
    public static List<ItemData> collectedItems = new List<ItemData>();

    [Header("Transições de Cena")]
    public Image blockingImage;               // Imagem de bloqueio para transições de cena
    public GameObject[] localScenes;          // Array de cenas locais
    int activeLocalScene = 0;                 // Índice da cena ativa
    public Transform[] playerStartPositions;  // Posições de início do jogador em cada cena

    [Header("UI do Inventário")]
    public GameObject equipmentCanvas;               // UI do inventário
    public Image[] equipmentSlots, equipmentImages;  // Slots de inventário e imagens dos itens
    public Sprite emptyItemSlotSprite;               // Sprite para slots vazios
    public Color selectedItemColor;                  // Cor do slot selecionado
    public int selectedCanvasSlotID = 0;             // ID do slot atualmente selecionado
    public ItemData.items selectedItemID = ItemData.items.none;  // ID do item atualmente selecionado

    // Função para selecionar um item no inventário
    public void SelectItem(int equipmentCanvasID)
    {
        Color c = Color.white;
        c.a = 0;

        // Define a transparência do slot anteriormente selecionado para 0
        equipmentSlots[selectedCanvasSlotID].color = c;

        // Se um slot inválido ou vazio foi clicado, deseleciona
        if (equipmentCanvasID >= collectedItems.Count || equipmentCanvasID < 0)
        {
            selectedItemID = ItemData.items.none;
            selectedCanvasSlotID = 0;
            return;
        }

        // Define a cor do novo slot selecionado e atualiza os IDs selecionados
        equipmentSlots[equipmentCanvasID].color = selectedItemColor;
        selectedCanvasSlotID = equipmentCanvasID;
        selectedItemID = collectedItems[selectedCanvasSlotID].itemID;
    }

    // Função para exibir o nome do item quando um slot é clicado
    public void ShowItemName(int equipmentCanvasID)
    {
        if (equipmentCanvasID < collectedItems.Count)
        {
            // Implementar lógica para mostrar o nome do item
            // Por exemplo, atualizar um TextMeshProUGUI com o nome do item
        }
    }

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

    // Atualiza o inventário na UI
    public void UpdateEquipmentCanvas()
    {
        int itemsAmount = collectedItems.Count;
        int itemSlotsAmount = equipmentSlots.Length;

        // Atualiza cada slot com o sprite do item correspondente ou um sprite vazio
        for (int i = 0; i < itemSlotsAmount; i++)
        {
            if (i < itemsAmount && collectedItems[i].itemSlotSprite != null)
                equipmentImages[i].sprite = collectedItems[i].itemSlotSprite;
            else
                equipmentImages[i].sprite = emptyItemSlotSprite;
        }

        // Seleciona automaticamente um item se o inventário tiver apenas um
        if (itemsAmount == 0)
            SelectItem(-1);
        else if (itemsAmount == 1)
            SelectItem(0);
    }

    // Corrotina para transição entre cenas com atraso
    public IEnumerator ChangeScene(int sceneNumber, float delay)
    {
        yield return new WaitForSeconds(delay);

        Color c = blockingImage.color;
        blockingImage.enabled = true;

        // Gradualmente escurece a tela para a transição
        while (blockingImage.color.a < 1)
        {
            c.a += Time.deltaTime;
            blockingImage.color = c;
            yield return null;
        }

        localScenes[activeLocalScene].SetActive(false);
        localScenes[sceneNumber].SetActive(true);

        activeLocalScene = sceneNumber;

        // Atualize a posição do jogador, se necessário
        // Exemplo:
        // player.transform.position = playerStartPositions[sceneNumber].position;

        // Gradualmente clareia a tela para concluir a transição
        while (blockingImage.color.a > 0)
        {
            c.a -= Time.deltaTime;
            blockingImage.color = c;
            yield return null;
        }
        blockingImage.enabled = false;
    }

    // Reinicia o jogo, recarregando a cena atual
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        collectedItems.Clear();
    }
}
