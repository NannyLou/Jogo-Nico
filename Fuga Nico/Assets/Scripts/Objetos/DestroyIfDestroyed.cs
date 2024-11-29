using UnityEngine;

public class DestroyIfDestroyed : MonoBehaviour
{
    private UniqueID uniqueID;

    private void Awake()
    {
        uniqueID = GetComponent<UniqueID>();
    }

    private void Start()
    {
        if (StateManager.instance != null && uniqueID != null)
        {
            if (StateManager.instance.IsObjectDestroyed(uniqueID.uniqueID))
            {
                Destroy(gameObject);
                Debug.Log("Teo foi destruído ao iniciar a cena.");
            }
        }
    }
}
