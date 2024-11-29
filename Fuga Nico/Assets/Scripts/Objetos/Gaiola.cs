using UnityEngine;

public class Gaiola : MonoBehaviour
{
    public Sprite openedGaiolaSprite; // Sprite da gaiola aberta (se tiver)
    private bool isOpen = false;

    private SpriteRenderer spriteRenderer;

    private UniqueID uniqueID; // Adicionado para identificar a gaiola

    private void Awake()
    {
        uniqueID = GetComponent<UniqueID>();
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Verifica se a gaiola já foi destruída
        if (StateManager.instance != null && uniqueID != null)
        {
            if (StateManager.instance.IsObjectDestroyed(uniqueID.uniqueID))
            {
                Destroy(gameObject);
                return;
            }
        }
    }

    // Remova o método OnMouseDown(), pois a interação agora será no cadeado

    public void OpenGaiola()
    {
        if (isOpen)
            return;

        isOpen = true;

        // Se tiver um sprite de gaiola aberta, altera para ele
        if (openedGaiolaSprite != null)
        {
            spriteRenderer.sprite = openedGaiolaSprite;
        }
        else
        {
            // Se não, desativa o sprite atual para sumir as grades
            spriteRenderer.enabled = false;
        }

        // Opcional: desativar o collider para não interagir mais
        GetComponent<Collider2D>().enabled = false;

        // Registra a gaiola como destruída
        if (StateManager.instance != null && uniqueID != null)
        {
            StateManager.instance.RegisterDestroyedObject(uniqueID.uniqueID);
        }

        // Destrói o GameObject da gaiola, se desejar
        // Destroy(gameObject); // Descomente esta linha se quiser destruir a gaiola
    }
}