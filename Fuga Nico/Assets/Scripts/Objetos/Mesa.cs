using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mesa : MonoBehaviour
{
    public GameObject jogador; // Referência ao jogador
    public DanielMove vilao;   // Referência ao vilão
    public UniqueID copoUniqueID; // UniqueID do copo (para verificar o estado)

    private void OnMouseDown()
    {
        if (StateManager.instance != null && copoUniqueID != null)
        {
            Debug.Log("Verificando estado 'RemedioNoCopo' para uniqueID: " + copoUniqueID.uniqueID);

            // Verifica se o remédio foi colocado no copo
            bool remedioNoCopo = StateManager.instance.CheckObjectState(copoUniqueID.uniqueID, "RemedioNoCopo");
            Debug.Log("Estado 'RemedioNoCopo' é: " + remedioNoCopo);

            if (remedioNoCopo)
            {
                if (jogador != null && vilao != null)
                {
                    // Desativa o jogador no cenário
                    jogador.SetActive(false);
                    Debug.Log("Jogador desativado.");

                    // Define o ponto inicial para o vilão (copo)
                    vilao.currentWaypointIndex = vilao.waypointIndex;

                    // Inicia o movimento do vilão
                    vilao.StartMoving();
                    Debug.Log("Vilão iniciou o movimento.");
                }
                else
                {
                    Debug.LogWarning("Referências de jogador ou vilão não estão configuradas!");
                }
            }
            else
            {
                // O remédio ainda não foi colocado no copo
                Debug.Log("Nada acontece. Talvez eu precise fazer algo primeiro.");
            }
        }
        else
        {
            Debug.LogWarning("StateManager ou copoUniqueID não estão configurados.");
        }
    }
}
