using System.Collections;
using UnityEngine;

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

        // Inicia a reprodução da primeira música
        PlayNextMusic();
    }

    private void PlayNextMusic()
    {
        audioSource.clip = musicClips[currentClipIndex];
        audioSource.Play();
        Debug.Log("Tocando: " + audioSource.clip.name);
        StartCoroutine(WaitForMusicEnd());
    }

    private IEnumerator WaitForMusicEnd()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        currentClipIndex = (currentClipIndex + 1) % musicClips.Length;
        PlayNextMusic();
    }
}
