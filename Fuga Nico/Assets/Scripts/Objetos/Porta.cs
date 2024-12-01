using System.Collections;
using UnityEngine;

public class Porta : MonoBehaviour
{
    public ItemData.items requiredItem = ItemData.items.teo; // Item necessário (Tatu)
    public Transform waypoint; // Ponto específico onde o Tatu aparecerá
    public GameObject porta; // Objeto da porta
    public GameObject seta; // Objeto da seta
    public GameObject tatuPrefab; // Prefab do Tatu que aparecerá no cenário
    public AnimationData tatuAnimation; // Animação do Tatu quebrando a porta

    private GameObject tatuInstance; // Instância do Tatu no cenário
    private SpriteAnimator tatuAnimator; // Referência ao SpriteAnimator

    private UniqueID uniqueID; // Identificação única para persistir o estado

    private void Awake()
    {
        uniqueID = GetComponent<UniqueID>();
    }

    private void Start()
    {
        // Verifica se a porta já foi destruída
        if (StateManager.instance != null && uniqueID != null)
        {
            if (StateManager.instance.IsObjectDestroyed(uniqueID.uniqueID))
            {
                // Se a porta foi destruída anteriormente, desativa a porta e ativa a seta
                if (porta != null)
                {
                    porta.SetActive(false);
                }

                if (seta != null)
                {
                    seta.SetActive(true);
                }

                // Não precisa continuar, já ajustou o estado
                return;
            }
        }

        // Certifique-se de que a seta esteja desativada inicialmente se a porta ainda existe
        if (seta != null)
        {
            seta.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        // Verifica se o item selecionado é o Tatu
        if (InventarioManager.instance.selectedItemID == requiredItem)
        {
            // Remove o Tatu do inventário
            InventarioManager.instance.collectedItems.RemoveAll(item => item.itemID == requiredItem);
            InventarioManager.instance.UpdateEquipmentCanvas();
            InventarioManager.instance.SelectItem(-1);

            // Inicia o processo de animação e manipulação da cena
            StartCoroutine(QuebrarPortaComTatu());
        }
        else
        {
            Debug.Log("O item necessário não está selecionado.");
        }
    }

    private IEnumerator QuebrarPortaComTatu()
    {
        // Instancia o Tatu no waypoint
        tatuInstance = Instantiate(tatuPrefab, waypoint.position, waypoint.rotation);
        tatuInstance.transform.position = waypoint.position; // Garantir posição correta
        tatuInstance.transform.localScale = Vector3.one;     // Garantir escala padrão

        // Obtém o SpriteAnimator do Tatu e inicia a animação
        tatuAnimator = tatuInstance.GetComponent<SpriteAnimator>();
        if (tatuAnimator != null && tatuAnimation != null)
        {
            tatuAnimator.PlayAnimation(tatuAnimation);
        }

        // Aguarda 3 segundos enquanto a animação é executada
        yield return new WaitForSeconds(3);

        // Remove o Tatu da cena
        if (tatuInstance != null)
        {
            Destroy(tatuInstance);
        }

        // Desativa a porta e ativa a seta
        if (porta != null)
        {
            porta.SetActive(false);
        }

        if (seta != null)
        {
            seta.SetActive(true);
        }

        // Registra a porta como destruída no StateManager
        if (StateManager.instance != null && uniqueID != null)
        {
            StateManager.instance.RegisterDestroyedObject(uniqueID.uniqueID);
        }
    }
}
