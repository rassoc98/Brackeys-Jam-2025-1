using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Player: MonoBehaviour
{
    public static event Action OnPlayerDeath;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Deadly")) return;
        
        OnPlayerDeath?.Invoke();
        Destroy(gameObject);
    }
}