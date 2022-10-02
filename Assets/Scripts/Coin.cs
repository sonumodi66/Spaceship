using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Coin : MonoBehaviour, IPoolableObject, ICollectable
{
    private Transform coinMesh;
    private float rotSpeed = 1f;

    private void OnEnable()
    {
        coinMesh = transform.GetChild(0);
        rotSpeed = Random.Range(100f, 600f);
    }

    private void Update()
    {
        coinMesh.Rotate(0f, 0f, rotSpeed * Time.deltaTime);
    }

    //-------------------------------------------------------------
    public void Collect()
    {
        gameObject.SetActive(false);
    }

    public GameObject itsGameObject => this.gameObject;
    
    public void Spawn(Vector3 _spawnPos, Vector3 _movingDirection)
    {
        transform.localPosition = _spawnPos;
        gameObject.SetActive(true);
    }

    public void Reset()
    {
        gameObject.SetActive(false);
    }
}
