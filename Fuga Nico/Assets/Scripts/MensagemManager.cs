using UnityEngine;
using TMPro;  // Importante: adiciona o namespace do TextMeshPro
using System.Collections;

public class MensagemManager : MonoBehaviour
{
    public TextMeshProUGUI mensagemText; // Usa TextMeshProUGUI para texto de UI
    public float duracaoMensagem = 2f; // Duração que a mensagem ficará na tela

    private Coroutine mensagemCoroutine;

    public void MostrarMensagem(string mensagem)
    {
        if (mensagemCoroutine != null)
        {
            StopCoroutine(mensagemCoroutine);
        }
        mensagemCoroutine = StartCoroutine(MostrarMensagemCoroutine(mensagem));
    }

    private IEnumerator MostrarMensagemCoroutine(string mensagem)
    {
        mensagemText.text = mensagem;
        mensagemText.enabled = true;

        yield return new WaitForSeconds(duracaoMensagem);

        mensagemText.enabled = false;
        mensagemText.text = "";
    }
}
