using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Carrega a cena especificada
    public void LoadScene(string Cena1)
    {
        SceneManager.LoadScene(Cena1);
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
}
