using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VilaoMovimento : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 2f;

    private int currentWaypointIndex = 0;
    private SpriteAnimator spriteAnimator;

    public AnimationData walkAnimation;
    public AnimationData idleAnimation;

    public GameObject chavePrefab;
    private GameObject chaveInstanciada;
    public Transform chavePosition;

    private Vector3 originalScale;

    private bool isReturning = false;
    private bool hasDroppedSecondKey = false;

    public int waypointIndexToDropKey = 1;
    public GameObject secondKeyPrefab;

    public string[] dialogueLines;
    private DialogueManager dialogueManager;
    private bool isDialogueActive = false;

    private UniqueID uniqueID; // Identificador único do vilão

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
    }

    private void Update()
    {
        if (!isReturning)
        {
            MoveAlongPath();
        }
        else
        {
            MoveBackAlongPath();
        }
    }

    private void MoveAlongPath()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            Transform target = waypoints[currentWaypointIndex];

            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            UpdateSpriteDirection(target.position);
            UpdateKeyPosition();

            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                currentWaypointIndex++;
            }
        }
        else
        {
            if (spriteAnimator != null && idleAnimation != null)
            {
                spriteAnimator.PlayAnimation(idleAnimation);
            }

            if (dialogueManager != null && !isDialogueActive)
            {
                dialogueManager.StartDialogue(dialogueLines);
                isDialogueActive = true;
            }
        }
    }

    private void MoveBackAlongPath()
    {
        if (currentWaypointIndex >= 0)
        {
            Transform target = waypoints[currentWaypointIndex];

            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            UpdateSpriteDirection(target.position);

            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                if (!hasDroppedSecondKey && currentWaypointIndex == waypointIndexToDropKey)
                {
                    DropSecondKey();
                }

                currentWaypointIndex--;
            }
        }
        else
        {
            // Registra o vilão como destruído e o remove da cena
            if (StateManager.instance != null && uniqueID != null)
            {
                StateManager.instance.RegisterDestroyedObject(uniqueID.uniqueID);
            }

            Destroy(gameObject);
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

    public void StartReturn()
    {
        if (!isReturning)
        {
            isReturning = true;
            currentWaypointIndex = waypoints.Length - 1;

            if (spriteAnimator != null && walkAnimation != null)
            {
                spriteAnimator.PlayAnimation(walkAnimation);
            }

            if (chaveInstanciada != null)
            {
                Destroy(chaveInstanciada);
            }

            if (dialogueManager != null && isDialogueActive)
            {
                dialogueManager.EndDialogue();
                isDialogueActive = false;
            }
        }
    }
}
