using UnityEngine;

namespace Audio
{
[RequireComponent(typeof(AudioSource))]
public class SoundObject : PoolObject
{
    private void Awake()
    {
        objectType = ObjectPoolManager.ObjectType.SoundObject;
    }
}
}