using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager instance;

    // Conjunto de IDs de objetos destruídos
    private HashSet<string> destroyedObjects = new HashSet<string>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre as cenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Método para registrar um objeto como destruído
    public void RegisterDestroyedObject(string objectID)
    {
        if (!destroyedObjects.Contains(objectID))
        {
            destroyedObjects.Add(objectID);
        }
    }

    // Método para verificar se um objeto está destruído
    public bool IsObjectDestroyed(string objectID)
    {
        return destroyedObjects.Contains(objectID);
    }
}
