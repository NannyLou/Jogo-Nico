using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextCena : MonoBehaviour
{
    public string lvlName;
    public Vector2 playerStartingPosition; // Posição onde o jogador deve iniciar na nova cena

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            // Armazena a posição inicial no GameManager
            GameManager.playerStartPosition = playerStartingPosition;

            // Carrega a nova cena
            SceneManager.LoadScene(lvlName);
        }
    }
}
