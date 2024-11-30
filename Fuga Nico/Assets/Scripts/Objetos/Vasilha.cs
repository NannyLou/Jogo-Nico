using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vasilha : MonoBehaviour
{
    public GameObject jogador; // Referência ao jogador
    public ValdivinoMove vilao; // Referência ao novo script do vilão

    private void OnMouseDown()
    {
        if (jogador != null && vilao != null)
        {
            jogador.SetActive(false); // Desativa o jogador
            Debug.Log("Jogador desapareceu do cenário.");

            vilao.StartMoving(); // Inicia o movimento do vilão
        }
        else
        {
            Debug.LogWarning("Referências de jogador ou vilão não estão configuradas!");
        }
    }
}
