using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader2 : MonoBehaviour
{
    // Carrega a cena especificada
    public void LoadScene(string sceneName)
    {
        if (sceneName == "Menu") // Substitua pelo nome real da sua cena de jogo
        {
            ResetGameState(); // Reseta o estado do jogo
        }

        SceneManager.LoadScene(sceneName);
    }

    // Sai do jogo
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    // Método para resetar o estado do jogo
    private void ResetGameState()
    {
        // Reseta o inventário
        if (InventarioManager.instance != null)
        {
            InventarioManager.instance.collectedItems.Clear();
            InventarioManager.instance.collectedItemIDs.Clear();
            InventarioManager.instance.UpdateEquipmentCanvas();
        }

        // Reseta os estados de objetos
        if (StateManager.instance != null)
        {
            StateManager.instance.ClearStates();
        }

        // Resetar outros sistemas persistentes, se necessário
        // Exemplo: resetar progresso, variáveis globais, etc.
    }
}
