using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour, IPoolableObject, ICollectable
{
    public GameObject itsGameObject => this.gameObject;

    public void Spawn(Vector3 _spawnPos)
    {
        transform.position = _spawnPos;
        gameObject.SetActive(true);
    }

    public void Reset()
    {
        ObjectPooler_Sonu.instance.ResetBackPooledObject(gameObject);
        gameObject.SetActive(false);
    }

    public void Collect()
    {
        Reset();
    }
}
