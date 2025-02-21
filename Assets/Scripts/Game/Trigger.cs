using System;
using UnityEngine;

namespace Game
{
[RequireComponent(typeof(Collider2D))]
public class Trigger : MonoBehaviour
{
    [SerializeField] private bool triggeredOnce = true;

    private bool _isTriggered;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || (triggeredOnce && _isTriggered)) return;
        OnTrigger?.Invoke();
        _isTriggered = true;
    }

    public event Action OnTrigger;
}
}