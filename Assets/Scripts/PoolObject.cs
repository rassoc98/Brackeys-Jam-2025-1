using System;
using UnityEngine;

public abstract class PoolObject : MonoBehaviour
{
    public ObjectPoolManager.ObjectType objectType;
    public static event Action<PoolObject> OnPoolObjectRecycle;

    public void Recycle()
    {
        OnPoolObjectRecycle?.Invoke(this);
    }
}