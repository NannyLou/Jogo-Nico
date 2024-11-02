using UnityEngine;

[System.Serializable][CreateAssetMenu(fileName = "NewItemData", menuName = "Items/ItemData")]
public class ItemData : ScriptableObject
{
    public enum items
    {
        none,
        chave,
        chave2,

        // Adicione outros itens conforme necessário
    }

    public items itemID;           // Identificador único do item
    public string itemName;        // Nome do item
    public Sprite itemSlotSprite;  // Sprite para exibição na UI do inventário
}
