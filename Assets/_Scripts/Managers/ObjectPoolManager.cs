using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// The Object Pool Manager stores pools which are a list for active objects and a queue for inactive objects,
/// the OPM's purpose is to recycle destroyed objects for later use to save on memory and performance overheads
/// associated with Instantiating/Destroying game objects.
///
/// To use the OPM the object has to inherit from PoolObject (which inherits from MonoBehaviour), and there has to be
/// a specific state for it in the ObjectPoolManager.ObjectType enum. For accessing the class make a private variable
/// and assign it at awake with FindFirstObjectByType and the ObjectPoolManager type, or if performance is not needed,
/// use FindFirstObjectByType or FindWithTag directly.
///
/// use Spawn(objectType, [optional] position, [optional] rotation) for spawning objects instead of Instantiate for
/// the object to be reused and managed by the OPM.
///
/// and use Deactivate(poolObject) and passing the PoolObject to it for deactivating the object.
/// </summary>

public class ObjectPoolManager : MonoBehaviour
{
    public enum ObjectType
    {
        NullType = 0,
        SoundObject
    }
    
    [SerializeField] private PoolObject[] objects;

    private readonly List<Pool> _objectPools = new();

    private void Awake()
    {
        PoolObject.OnPoolObjectRecycle += Deactivate;
        
        if (GetPrefabTypeDuplicates() == -1) return;
        Debug.LogError($"{objects[GetPrefabTypeDuplicates()].objectType} type is specified more than once!");
        Application.Quit();
    }

    public GameObject Spawn(ObjectType objectType, Vector3 position = new(), Quaternion rotation = new())
    {
        Pool pool = _objectPools.Find(x => x.ObjectPoolType == objectType);
        if (pool == null)
        {
            pool = new Pool(objectPoolType: objectType);
            _objectPools.Add(pool);
            return Spawn(objectType, position, rotation);
        }

        var obj = pool.InactiveObjects.FirstOrDefault();

        if(obj == null)
        {
            var prefab = Array.Find(objects, prefab => prefab.objectType == objectType);
            obj = Instantiate(prefab.gameObject, position, rotation);
            pool.ActiveObjects.Add(obj);
        }
        else
        {
            pool.InactiveObjects.Dequeue();
            obj.SetActive(true);
        }

        return obj;
    }

    public void Deactivate(PoolObject objectToRecycle)
    {
        var pool = _objectPools.Find(pool => pool.ObjectPoolType == objectToRecycle.objectType);
        
        if (pool == null)
        {
            Debug.LogWarning("Failed to recycle object, the object has no pool!");
            return;
        }
        
        if (!pool.ActiveObjects.Contains(objectToRecycle.gameObject))
        {
            Debug.LogWarning("Failed to recycle object, the object was not found in the pool!");
            return;
        }
        
        objectToRecycle.gameObject.SetActive(false);
        pool.ActiveObjects.Remove(objectToRecycle.gameObject);
        pool.InactiveObjects.Enqueue(objectToRecycle.gameObject);
    }
    
    // Returns index of duplicate, -1 means none.
    private int GetPrefabTypeDuplicates()
    {
        var types = new ObjectType[objects.Length];
        for (int i = 0; i < objects.Length; i++)
        {
            if (types.Contains(objects[i].objectType)) return i;
            types[i] = objects[i].objectType;
        }
        
        return -1;
    }
}

// Contains a type for the type of objects that the pool stores, a list for active objects,
// And a queue for inactive objects.
public class Pool
{
    public readonly ObjectPoolManager.ObjectType ObjectPoolType;
    public readonly List<GameObject> ActiveObjects;
    public readonly Queue<GameObject> InactiveObjects;

    public Pool(ObjectPoolManager.ObjectType objectPoolType)
    {
        ObjectPoolType = objectPoolType;
        ActiveObjects = new List<GameObject>();
        InactiveObjects = new Queue<GameObject>();
    }
}