using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movel : MonoBehaviour
{
    public GameObject jogador; // Referência ao jogador
    public DanielMove vilao;   // Referência ao vilão

    private void OnMouseDown()
    {
        if (jogador != null && vilao != null)
        {
            // Desativa o jogador no cenário
            jogador.SetActive(false);

            // Define o ponto inicial para o vilão (copo)
            vilao.currentWaypointIndex = vilao.waypointIndex;

            // Inicia o movimento do vilão
            vilao.StartMoving();
        }
        
        else
        {
            Debug.LogWarning("Referências de jogador ou vilão não estão configuradas!");
        }
    }
}
