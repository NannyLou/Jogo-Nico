using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cadeados : MonoBehaviour
{
    public ItemData.items requiredItem = ItemData.items.chaveZeca; // Item necessário para abrir o cadeado

    // Referências às gaiolas que podem ser abertas
    public Gaiola gaiola;               // Gaiola genérica
    public GaiolaTeoTelma gaiolaTeoTelma; // Gaiola do Teo e Telma
    public GaiolaZeca gaiolaZeca;         // Gaiola do Zeca

    private MensagemManager mensagemManager;

    private UniqueID uniqueID; // Identificador único do cadeado

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
            // Remove a chave do inventário
            InventarioManager.instance.collectedItems.RemoveAll(item => item.itemID == requiredItem);
            InventarioManager.instance.UpdateEquipmentCanvas();
            InventarioManager.instance.SelectItem(-1);

            // Abre a gaiola correspondente
            bool gaiolaAberta = false;

            if (gaiola != null)
            {
                gaiola.OpenGaiola();
                gaiolaAberta = true;
            }

            if (gaiolaTeoTelma != null)
            {
                gaiolaTeoTelma.OpenGaiola();
                gaiolaAberta = true;
            }

            if (gaiolaZeca != null)
            {
                gaiolaZeca.OpenGaiola();
                gaiolaAberta = true;
            }

            if (!gaiolaAberta)
            {
                Debug.LogWarning("Nenhuma gaiola atribuída ao cadeado na cena atual.");
            }

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
            else
            {
                Debug.LogWarning("MensagemManager não encontrado na cena.");
            }
        }
    }
}