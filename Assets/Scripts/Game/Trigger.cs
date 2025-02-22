using System;
using System.Collections;
using UnityEngine;

namespace Game
{
[RequireComponent(typeof(Collider2D))]
public class Trigger : MonoBehaviour
{
    [SerializeField] private bool triggeredOnce = true;
    [SerializeField] private float delay;
    
    private bool _isTriggered;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || (triggeredOnce && _isTriggered)) return;
        
        _isTriggered = true;
        StartCoroutine(HandleTrigger());
    }

    private IEnumerator HandleTrigger()
    {
        var isInvoked = false;
        var timer = 0f;
        
        while (!isInvoked)
        {
            yield return null;
            
            timer += Time.deltaTime;
            if (timer < delay) continue;
        
            OnTrigger?.Invoke();
            isInvoked = true;
        }
    }

    public event Action OnTrigger;
}
}