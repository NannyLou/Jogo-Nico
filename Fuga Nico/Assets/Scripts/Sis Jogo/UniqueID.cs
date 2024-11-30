using UnityEngine;
using System;

public class UniqueID : MonoBehaviour
{
    public string uniqueID;

    private void Awake()
    {
        // Se o uniqueID não estiver definido, gera um novo
        if (string.IsNullOrEmpty(uniqueID))
        {
            uniqueID = Guid.NewGuid().ToString();
        }
    }
}
