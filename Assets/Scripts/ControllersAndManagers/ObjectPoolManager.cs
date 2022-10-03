using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object pool manager is for Object Pooling system in the game
/// </summary>
public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;

    public List<PoolProfile> objectsToPool_List;

    private Dictionary<string, PooledData> pooledData_Dict = new Dictionary<string, PooledData>();

    //--------------------------------------------------------------------------------------
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        PoolObjectsInitial();
    }
    //--------------------------------------------------------------------------------------
    /// <summary>
    /// Get Pooled Object By Object Name
    /// </summary>
    /// <param name="_pooledObjectName"></param>
    /// <returns></returns>
    public IPoolableObject GetPooledObject(string _pooledObjectName)
    {
        if (!pooledData_Dict.ContainsKey(_pooledObjectName))
        {
            Debug.Log("Not Found : " + _pooledObjectName);
            return null;
        }

        //No Remaining Object Pool More
        if (pooledData_Dict[_pooledObjectName].pooledList.Count <= 0)
        {
            return PoolMoreObjects(_pooledObjectName);
        }
        else
        {
            IPoolableObject tempObje = pooledData_Dict[_pooledObjectName].pooledList[0];
            pooledData_Dict[_pooledObjectName].pooledList.RemoveAt(0);
            return tempObje;
        }
    }

    /// <summary>
    /// Pool more objects if already pooled objects are in use and new one is needed
    /// </summary>
    /// <param name="_pooledObjectName"></param>
    /// <returns></returns>
    private IPoolableObject PoolMoreObjects(string _pooledObjectName)
    {
        if (!pooledData_Dict.ContainsKey(_pooledObjectName)) return null;

        GameObject prefabToPool = null;
        GameObject tempPooledObject = null;
        int index = -1;

        for (int i = 0; i < objectsToPool_List.Count; i++)
        {
            if (String.Equals(objectsToPool_List[i].objectToPool.name, _pooledObjectName))
            {
                prefabToPool = objectsToPool_List[i].objectToPool;
                index = i;
            }
        }

        if (prefabToPool == null || !objectsToPool_List[index].isAutoExpand)
            return null;

        tempPooledObject = Instantiate(prefabToPool);
        tempPooledObject.SetActive(false);
        tempPooledObject.name = _pooledObjectName;
        tempPooledObject.transform.parent =  pooledData_Dict[_pooledObjectName].pooledParent.transform;

        return tempPooledObject.GetComponent<IPoolableObject>();
    }

    /// <summary>
    /// Reset back to its respected pooled list when use has been ended
    /// </summary>
    /// <param name="_pooledObject"></param>
    public void ResetBackPooledObject(GameObject _pooledObject)
    {
        if (!pooledData_Dict.ContainsKey(_pooledObject.name))
        {
            Debug.Log("Can't be reset back : " + _pooledObject.name);
            return;
        }

        pooledData_Dict[_pooledObject.name].pooledList.Add(_pooledObject.GetComponent<IPoolableObject>());
        _pooledObject.SetActive(false);
    }

    /// <summary>
    /// Pool objects initally as per data set in the inspector
    /// </summary>
    private void PoolObjectsInitial()
    {
        GameObject tempPooledObject;
        GameObject poolParent;
        pooledData_Dict.Clear();

        for (int i = 0; i < objectsToPool_List.Count; i++)
        {
            if (objectsToPool_List[i].objectToPool == null)
                break;

            poolParent = new GameObject(objectsToPool_List[i].objectToPool.name);
            List<IPoolableObject> pooledList = new List<IPoolableObject>();
            pooledList.Clear();

            for (int j = 0; j < objectsToPool_List[i].amount; j++)
            {
                tempPooledObject = Instantiate(objectsToPool_List[i].objectToPool, poolParent.transform);
                tempPooledObject.SetActive(false);
                tempPooledObject.name = objectsToPool_List[i].objectToPool.name;
                pooledList.Add(tempPooledObject.GetComponent<IPoolableObject>());
            }

            pooledData_Dict.Add(objectsToPool_List[i].objectToPool.name, new PooledData(poolParent, pooledList));
        }
    }
}


[System.Serializable]
public class PoolProfile
{
    public string poolName;
    public GameObject objectToPool;
    public int amount;
    public bool isAutoExpand;
}

[System.Serializable]
public class PooledData
{
    public GameObject pooledParent;
    public List<IPoolableObject> pooledList = new List<IPoolableObject>();

    public PooledData(GameObject _poolParent, List<IPoolableObject> _pooledList)
    {
        this.pooledParent = _poolParent;
        this.pooledList = _pooledList;
    }
}