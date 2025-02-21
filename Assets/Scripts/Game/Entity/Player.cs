using System;
using Audio;
using UnityEngine;

namespace Game.Entity
{
[RequireComponent(typeof(Collider2D))]
public class Player : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.PlaySound("Awake");
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Deadly")) return;
        
        OnPlayerDeath?.Invoke();
        AudioManager.Instance.PlaySound("Hurt");
        Destroy(gameObject);
    }

    public static event Action OnPlayerDeath;
}
}