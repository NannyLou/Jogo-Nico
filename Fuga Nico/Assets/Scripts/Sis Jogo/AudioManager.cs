using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    // Singleton Instance
    public static MusicManager Instance { get; private set; }

    [Header("Áudios de Fundo")]
    public AudioClip musicClip1; // Primeira música
    public AudioClip musicClip2; // Segunda música

    private AudioSource audioSource;
    private AudioClip[] musicClips;
    private int currentClipIndex = 0;

    private void Awake()
    {
        // Implementa o padrão Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre as cenas
        }
        else
        {
            Destroy(gameObject); // Garante apenas uma instância
            return;
        }

        // Obtém o AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configurações iniciais do AudioSource
        audioSource.playOnAwake = false;
        audioSource.loop = false; // Controlaremos manualmente
    }

    private void Start()
    {
        // Verifica se os clipes de áudio estão atribuídos
        if (musicClip1 == null || musicClip2 == null)
        {
            Debug.LogWarning("Os clipes de áudio não estão atribuídos no MusicManager.");
            return;
        }

        // Armazena as músicas em um array para facilitar a alternância
        musicClips = new AudioClip[] { musicClip1, musicClip2 };

        // Inscreve-se no evento de mudança de cena
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Inicia a reprodução da música se não estiver no "Menu"
        if (SceneManager.GetActiveScene().name != "Menu")
        {
            PlayNextMusic();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Pausa ou retoma a música com base na cena
        if (scene.name == "Menu")
        {
            StopMusic();
        }
        else if (!audioSource.isPlaying)
        {
            RestartMusic();
        }
    }

    public void OnPlayButtonClicked()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            RestartMusic();
        }
    }

    private void PlayNextMusic()
    {
        audioSource.clip = musicClips[currentClipIndex];
        audioSource.Play();
        Debug.Log("Tocando: " + audioSource.clip.name);
        StartCoroutine(WaitForMusicEnd());
    }

    private void RestartMusic()
    {
        StopAllCoroutines();
        currentClipIndex = 0; // Reinicia a música para o início
        PlayNextMusic();
    }

    private void StopMusic()
    {
        StopAllCoroutines();
        audioSource.Stop();
    }

    private IEnumerator WaitForMusicEnd()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        currentClipIndex = (currentClipIndex + 1) % musicClips.Length;
        PlayNextMusic();
    }

    private void OnDestroy()
    {
        // Desinscreve-se do evento ao destruir o objeto
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
