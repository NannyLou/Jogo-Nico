using System.Collections;
using UnityEngine;

public class VilaoMovimento : MonoBehaviour
{
    [Header("Waypoints")]
    public Transform[] waypoints;
    public float speed = 2f;

    private int currentWaypointIndex = 0;
    private SpriteAnimator spriteAnimator;

    [Header("Animações")]
    public AnimationData walkAnimation;
    public AnimationData idleAnimation;

    [Header("Chave")]
    public GameObject chavePrefab;
    private GameObject chaveInstanciada;
    public Transform chavePosition;

    private Vector3 originalScale;

    private bool isReturning = false;
    private bool hasDroppedSecondKey = false;

    [Header("Segunda Chave")]
    public int waypointIndexToDropKey = 1;
    public GameObject secondKeyPrefab;

    [Header("Diálogo")]
    public string[] dialogueLines;

    private DialogueManager dialogueManager;
    private bool isDialogueActive = false;

    private UniqueID uniqueID; // Identificador único do vilão

    [Header("Seta")]
    public GameObject seta; // Objeto que será ativado ao desaparecer o vilão

    // Referência ao ClickMove
    private ClickMove clickMove;

    // Novo campo para definir o diálogo após o vilão desaparecer
    [Header("Diálogo Pós-Desaparecimento")]
    [TextArea]
    public string[] postDesapareceuDialogueLines;

    private void Awake()
    {
        uniqueID = GetComponent<UniqueID>();
    }

    private void Start()
    {
        // Verifica se o vilão já foi destruído
        if (StateManager.instance != null && uniqueID != null)
        {
            if (StateManager.instance.IsObjectDestroyed(uniqueID.uniqueID))
            {
                if (seta != null)
                {
                    seta.SetActive(true); // Ativa a seta se o vilão já estiver destruído
                }

                Destroy(gameObject); // Destrói o vilão imediatamente
                return;
            }
        }

        spriteAnimator = GetComponent<SpriteAnimator>();
        originalScale = transform.localScale;

        if (spriteAnimator != null && walkAnimation != null)
        {
            spriteAnimator.PlayAnimation(walkAnimation);
        }

        if (chavePrefab != null && chavePosition != null)
        {
            chaveInstanciada = Instantiate(chavePrefab, chavePosition.position, Quaternion.identity, transform);

            ItemPickup itemPickup = chaveInstanciada.GetComponent<ItemPickup>();
            if (itemPickup != null)
            {
                itemPickup.vilao = this;
            }
        }

        dialogueManager = DialogueManager.instance;

        // Garante que a seta esteja inicialmente desativada
        if (seta != null)
        {
            seta.SetActive(false);
        }

        // Obtém a referência ao ClickMove
        clickMove = FindObjectOfType<ClickMove>();
        if (clickMove == null)
        {
            // Nenhum Debug.Log conforme solicitado
        }

        // Inicia a movimentação do vilão
        StartCoroutine(MoveAlongPath());
    }

    private IEnumerator MoveAlongPath()
    {
        if (clickMove != null)
        {
            clickMove.DisableMovement();
        }

        while (currentWaypointIndex < waypoints.Length)
        {
            Transform target = waypoints[currentWaypointIndex];
            while (Vector3.Distance(transform.position, target.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                UpdateSpriteDirection(target.position);
                UpdateKeyPosition();
                yield return null;
            }
            currentWaypointIndex++;
        }

        // Chegou ao final do caminho (ida)
        if (spriteAnimator != null && idleAnimation != null)
        {
            spriteAnimator.PlayAnimation(idleAnimation);
        }

        if (dialogueManager != null && !isDialogueActive)
        {
            dialogueManager.StartDialogue(dialogueLines);
            isDialogueActive = true;
        }

        if (clickMove != null)
        {
            clickMove.EnableMovement();
        }
    }

    private IEnumerator MoveBackAlongPath()
    {
        if (waypoints.Length == 0)
            yield break;

        isReturning = true;
        if (clickMove != null)
        {
            clickMove.DisableMovement();
        }

        // Reset currentWaypointIndex para o último waypoint válido
        currentWaypointIndex = waypoints.Length - 1;

        // Inicia a animação de caminhada na volta
        if (spriteAnimator != null && walkAnimation != null)
        {
            spriteAnimator.PlayAnimation(walkAnimation);
        }

        while (currentWaypointIndex >= 0)
        {
            Transform target = waypoints[currentWaypointIndex];
            while (Vector3.Distance(transform.position, target.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                UpdateSpriteDirection(target.position);
                UpdateKeyPosition();
                yield return null;
            }

            if (!hasDroppedSecondKey && currentWaypointIndex == waypointIndexToDropKey)
            {
                DropSecondKey();
            }

            currentWaypointIndex--;

            // Evita que currentWaypointIndex fique negativo
            if (currentWaypointIndex < 0)
            {
                break;
            }
        }

        // Movimentação de retorno concluída

        // Registra o vilão como destruído no StateManager
        if (StateManager.instance != null && uniqueID != null)
        {
            StateManager.instance.RegisterDestroyedObject(uniqueID.uniqueID);
        }

        // Ativa a seta antes de destruir o vilão
        if (seta != null)
        {
            seta.SetActive(true);
        }

        // Inicia o diálogo após a movimentação de volta
        if (dialogueManager != null && postDesapareceuDialogueLines.Length > 0)
        {
            // Aguardar um breve momento para garantir que a seta foi ativada
            yield return new WaitForSeconds(0.1f);
            dialogueManager.StartDialogue(postDesapareceuDialogueLines);
        }

        if (clickMove != null)
        {
            clickMove.EnableMovement();
        }

        Destroy(gameObject);
    }

    public void StartReturn()
    {
        if (!isReturning)
        {
            StartCoroutine(MoveBackAlongPath());
        }
    }

    private void DropSecondKey()
    {
        if (secondKeyPrefab != null)
        {
            GameObject droppedKey = Instantiate(secondKeyPrefab, transform.position, Quaternion.identity);
            droppedKey.transform.localScale = new Vector3(2, 2, 1);
            hasDroppedSecondKey = true;
        }
    }

    private void UpdateSpriteDirection(Vector3 targetPosition)
    {
        if (targetPosition.x > transform.position.x)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }

    private void UpdateKeyPosition()
    {
        if (chaveInstanciada != null && chavePosition != null)
        {
            chaveInstanciada.transform.position = chavePosition.position;
        }
    }
}
