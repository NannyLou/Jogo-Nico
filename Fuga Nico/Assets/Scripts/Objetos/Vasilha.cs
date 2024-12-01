using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vasilha : MonoBehaviour
{
    public GameObject jogador; // Referência ao jogador (Nico)
    public ValdivinoMove vilao; // Referência ao script do vilão
    public UniqueID caldeiraoUniqueID; // UniqueID do caldeirao para verificar o estado
    public UniqueID vasilhaUniqueID; // UniqueID da vasilha para registrar uso

    private void Start()
    {
        // Verifica se a vasilha já foi usada
        if (StateManager.instance != null && vasilhaUniqueID != null)
        {
            if (StateManager.instance.IsObjectDestroyed(vasilhaUniqueID.uniqueID))
            {
                // Ação já foi executada, desativa a vasilha
                gameObject.SetActive(false);
                return;
            }
        }
    }

    private void OnMouseDown()
    {
        if (StateManager.instance != null && caldeiraoUniqueID != null && vasilhaUniqueID != null)
        {
            Debug.Log("Verificando estado 'FenoNoCaldeirao' para uniqueID: " + caldeiraoUniqueID.uniqueID);

            // Verifica se o feno foi colocado no caldeirao
            bool fenoNoCaldeirao = StateManager.instance.CheckObjectState(caldeiraoUniqueID.uniqueID, "FenoNoCaldeirao");
            Debug.Log("Estado 'FenoNoCaldeirao' é: " + fenoNoCaldeirao);

            // Verifica se a vasilha já foi usada
            bool vasilhaUsada = StateManager.instance.IsObjectDestroyed(vasilhaUniqueID.uniqueID);
            Debug.Log("Estado 'VasilhaUsada' é: " + vasilhaUsada);

            if (fenoNoCaldeirao && !vasilhaUsada)
            {
                if (jogador != null && vilao != null)
                {
                    // Desativa o jogador no cenário
                    jogador.SetActive(false);
                    Debug.Log("Jogador desativado.");

                    // Define o ponto inicial para o vilão (caldeirão)
                    // **Defina o currentWaypointIndex para o waypoint desejado*

                    // Inicia o movimento do vilão
                    vilao.StartMoving();
                    Debug.Log("Vilão iniciou o movimento.");

                    // Marca a vasilha como usada no StateManager
                    StateManager.instance.RegisterDestroyedObject(vasilhaUniqueID.uniqueID);
                    Debug.Log("Vasilha marcada como usada no StateManager.");

                    // Desativa a vasilha na cena
                    gameObject.SetActive(false);
                }
                else
                {
                    Debug.LogWarning("Referências de jogador ou vilão não estão configuradas!");
                }
            }
            else if (!fenoNoCaldeirao)
            {
                // O feno ainda não foi colocado no caldeirao
                Debug.Log("Nada acontece. Talvez eu precise colocar feno no caldeirao primeiro.");
            }
            else if (vasilhaUsada)
            {
                // A ação já foi executada
                Debug.Log("Ação da vasilha já foi executada.");
            }
        }
        else
        {
            Debug.LogWarning("StateManager, caldeiraoUniqueID ou vasilhaUniqueID não estão configurados.");
        }
    }
}
