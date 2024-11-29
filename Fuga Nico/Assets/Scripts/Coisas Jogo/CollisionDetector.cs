using UnityEngine;
using System;

public class CollisionDetector : MonoBehaviour
{
    public event Action OnCollisionDetected;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollisionDetected?.Invoke();
    }
}
