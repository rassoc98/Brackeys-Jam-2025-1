using System;
using UnityEngine;

public abstract class PoolObject : MonoBehaviour
{
    public static event Action<PoolObject> OnPoolObjectRecycle;
    public ObjectPoolManager.ObjectType objectType;

    public void Recycle()
    {
        OnPoolObjectRecycle?.Invoke(this);
    }
}