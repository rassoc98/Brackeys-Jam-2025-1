using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class ObjPoolManager : MonoBehaviour
{
    public static List<PooledObjectInfo> objectPools = new();

    public static ObjPoolManager Instance;

    private void Awake() // singleton pattern, not using DontDestroyOnLoad, or else it would bring all of its pooled objects to the new scene.
    {
        if (Instance == null) Instance = this;
        else if (!Instance == this)
        {
            Debug.LogError("too many ObjPoolManagers");
            Instance = this;
        }
    }   

    public static GameObject SpawnObject(GameObject objectToSpawn) //use this to spawn prefabs, and it will set up the pool on its own
    {
        PooledObjectInfo pool = objectPools.Find(p => p.LookupString == objectToSpawn.name);

        if (pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
            objectPools.Add(pool);
        }

        GameObject spawnableObject = pool.InactiveObjects.FirstOrDefault();

        if(spawnableObject == null)
        {
            spawnableObject = Instantiate(objectToSpawn, Instance.transform);
        }
        else
        {
            pool.InactiveObjects.Remove(spawnableObject);
            spawnableObject.SetActive(true);
        }

        return spawnableObject;
    }

    public static void RecycleObject(GameObject obj)
    {
        if (!obj.name.Contains("(Clone)"))
        {
            //Debug.LogWarning("trying to recycle something that is not a spawned prefab");
            Destroy(obj);
            return;
        } else if (obj.name[0..^7].Contains("(Clone)"))
        {
            Debug.LogWarning("trying to recycle an obj(Clone)(Clone)");
            Destroy(obj);
            return;
        }

        string goName = obj.name[0..^7]; // removes "(Clone)" from the end of cloned objects, to make its name match the LookupString

        PooledObjectInfo pool = objectPools.Find(p => p.LookupString == goName);

        if (pool == null) Debug.LogWarning("trying to recycle object that has no pool");
        else
        {
            obj.SetActive(false);
            pool.InactiveObjects.Add(obj);
        }
    }
}

public class PooledObjectInfo
{
    public string LookupString;
    public List<GameObject> InactiveObjects = new();
}