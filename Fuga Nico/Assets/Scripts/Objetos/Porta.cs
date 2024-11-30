using System.Collections;
using UnityEngine;

public class Porta : MonoBehaviour
{
    public GameObject teoPrefab; // Prefab do Teo, posicionado e desativado na cena
    public Transform[] waypoints; // Waypoints para o movimento do Teo
    public float teoSpeed = 2f; // Velocidade do Teo
    public string[] dialogueLines; // Linhas de diálogo após a destruição
    private bool isTeoActive = false; // Evita cliques múltiplos

    private void OnMouseDown()
    {
        // Verifica se o Teo está no inventário e evita múltiplas execuções
        if (InventarioManager.instance.selectedItemID == ItemData.items.teo && !isTeoActive)
        {
            isTeoActive = true;

            // Ativa o prefab do Teo
            if (teoPrefab != null)
            {
                teoPrefab.SetActive(true);
                StartCoroutine(MoveTeoAndDestroyPorta());
            }
        }
        else
        {
            Debug.Log("Você precisa do Teo para abrir esta porta!");
        }
    }

    private IEnumerator MoveTeoAndDestroyPorta()
    {
        // Move o Teo pelos waypoints
        foreach (Transform waypoint in waypoints)
        {
            while (Vector3.Distance(teoPrefab.transform.position, waypoint.position) > 0.1f)
            {
                teoPrefab.transform.position = Vector3.MoveTowards(
                    teoPrefab.transform.position,
                    waypoint.position,
                    teoSpeed * Time.deltaTime
                );
                yield return null;
            }
        }

        // Exibe o diálogo
        if (DialogueManager.instance != null && dialogueLines.Length > 0)
        {
            DialogueManager.instance.StartDialogue(dialogueLines);
        }

        // Desativa o Teo após o movimento
        if (teoPrefab != null)
        {
            teoPrefab.SetActive(false);
        }

        // Remove a porta
        Destroy(gameObject);
    }
}
