using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager instance;

    // Conjunto de IDs de objetos destruídos
    private HashSet<string> destroyedObjects = new HashSet<string>();

    // Dicionário para armazenar estados adicionais dos objetos
    private Dictionary<string, Dictionary<string, bool>> objectStates = new Dictionary<string, Dictionary<string, bool>>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Garante que o objeto persiste entre cenas
        }
        else
        {
            Destroy(gameObject); // Evita duplicações do StateManager
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

    // Método para atualizar o estado de um objeto com uma chave e valor
    public void UpdateObjectState(string uniqueID, string key, bool value)
    {
        if (!objectStates.ContainsKey(uniqueID))
        {
            objectStates[uniqueID] = new Dictionary<string, bool>();
        }

        objectStates[uniqueID][key] = value;
    }

    // Método para verificar o estado de um objeto com uma chave
    public bool CheckObjectState(string uniqueID, string key)
    {
        if (objectStates.ContainsKey(uniqueID))
        {
            if (objectStates[uniqueID].ContainsKey(key))
            {
                return objectStates[uniqueID][key];
            }
        }
        return false;
    }

    // Método opcional para limpar estados e objetos destruídos (se necessário)
    public void ClearStates()
    {
        destroyedObjects.Clear();
        objectStates.Clear();
    }
}
