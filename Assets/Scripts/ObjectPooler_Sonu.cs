using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler_Sonu : MonoBehaviour
{
    public static ObjectPooler_Sonu instance;

    public List<PoolProfile> objectsToPool_List;

    private Dictionary<string, List<IPoolableObject>> pooledData_Dict = new Dictionary<string, List<IPoolableObject>>();

    //-----------------------------------------------------------------------------
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        PoolObjectsInitial();
    }

    public IPoolableObject GetPooledObject(string _pooledObjectName)
    {
        if (!pooledData_Dict.ContainsKey(_pooledObjectName))
        {
            Debug.Log("Not Found : " + _pooledObjectName);
            return null;
        }

        //No Remaining Object Pool More
        if (pooledData_Dict[_pooledObjectName].Count <= 0)
        {
        }
        else
        {
            IPoolableObject tempObje = pooledData_Dict[_pooledObjectName][0];
            pooledData_Dict[_pooledObjectName].RemoveAt(0);
            return tempObje;
        }

        return null;
    }

    public void ResetBackPooledObject(GameObject _pooledObject)
    {
        if (!pooledData_Dict.ContainsKey(_pooledObject.name))
        {
            Debug.Log("Can't be reset back : " + _pooledObject.name);
            return;
        }

        pooledData_Dict[_pooledObject.name].Add(_pooledObject.GetComponent<IPoolableObject>());
        _pooledObject.SetActive(false);
    }

    private void PoolObjectsInitial()
    {
        GameObject tempPooledObject;
        GameObject poolParent;
        pooledData_Dict.Clear();

        for (int i = 0; i < objectsToPool_List.Count; i++)
        {
            poolParent = new GameObject(objectsToPool_List[i].objectToPool.name + "s");
            List<IPoolableObject> pooledList = new List<IPoolableObject>();
            pooledList.Clear();

            for (int j = 0; j < objectsToPool_List[i].amount; j++)
            {
                tempPooledObject = Instantiate(objectsToPool_List[i].objectToPool, poolParent.transform);
                tempPooledObject.SetActive(false);
                tempPooledObject.name = objectsToPool_List[i].objectToPool.name;
                pooledList.Add(tempPooledObject.GetComponent<IPoolableObject>());
            }

            pooledData_Dict.Add(objectsToPool_List[i].objectToPool.name, pooledList);
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