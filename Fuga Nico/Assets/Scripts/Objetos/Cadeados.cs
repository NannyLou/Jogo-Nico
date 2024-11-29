using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cadeados : MonoBehaviour
{
    public ItemData.items requiredItem = ItemData.items.chave; // Item necessário para abrir o cadeado
    public Gaiola gaiola; // Referência à Gaiola que será aberta
    public GaiolaZeca gaiolaZeca;
    private MensagemManager mensagemManager;

    private UniqueID uniqueID; // Adicionado para identificar o cadeado

    private void Awake()
    {
        uniqueID = GetComponent<UniqueID>();
    }

    private void Start()
    {
        mensagemManager = FindObjectOfType<MensagemManager>();

        // Verifica se o cadeado já foi destruído
        if (StateManager.instance != null && uniqueID != null)
        {
            if (StateManager.instance.IsObjectDestroyed(uniqueID.uniqueID))
            {
                Destroy(gameObject);
                return;
            }
        }
    }

    private void OnMouseDown()
    {
        if (InventarioManager.instance.selectedItemID == requiredItem)
        {
            // Chama o método para abrir a gaiola
            if (gaiola != null)
            {
                gaiola.OpenGaiola();
            }

            if (gaiolaZeca != null)
            {
                gaiolaZeca.OpenGaiola();
            }

            // Remove a chave do inventário, se desejar
            InventarioManager.instance.collectedItems.RemoveAll(item => item.itemID == requiredItem);
            InventarioManager.instance.UpdateEquipmentCanvas();
            InventarioManager.instance.SelectItem(-1);

            // Registra o cadeado como destruído
            if (StateManager.instance != null && uniqueID != null)
            {
                StateManager.instance.RegisterDestroyedObject(uniqueID.uniqueID);
            }

            // Destrói o cadeado
            Destroy(gameObject);
        }
        else
        {
            // Exibe mensagem na tela
            if (mensagemManager != null)
            {
                mensagemManager.MostrarMensagem("O cadeado está trancado. Você precisa de uma chave.");
            }
        }
    }
}