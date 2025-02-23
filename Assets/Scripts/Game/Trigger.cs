using System;
using System.Collections;
using UnityEngine;

namespace Game
{
[RequireComponent(typeof(Collider2D))]
public class Trigger : MonoBehaviour
{
    [SerializeField] protected bool triggeredOnce = true;
    [SerializeField] protected float delay;
    
    protected bool IsTriggered;

    protected void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || (triggeredOnce && IsTriggered)) return;
        
        IsTriggered = true;
        StartCoroutine(HandleTrigger());
    }

    protected virtual IEnumerator HandleTrigger()
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