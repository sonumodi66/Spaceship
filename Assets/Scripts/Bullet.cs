using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolableObject
{
    private PlayerSpaceship playerSpaceship;
    float fireSpeed = 10f;

    private float firedTime;
    private float resetDuration = 3f;
    private bool isFired;

    private Vector3 fireDirection;
    
    private void Start()
    {
        playerSpaceship = GameManager.instance.PlayerSpaceship;
    }

    private void Update()
    {
        MoveBullet();
        CheckIfNeedsToBeReset();
    }

    private void CheckIfNeedsToBeReset()
    {
        if (!isFired) return;
        
        bool isReset = Time.time > (firedTime + resetDuration);
        
        if(isReset)
            Reset();
    }

    private void MoveBullet()
    {
        transform.Translate(fireDirection * fireSpeed * Time.deltaTime);
    }

    //-------------------------------------------------------------------------------
    public GameObject itsGameObject => this.gameObject;

    public void Spawn(Vector3 _spawnPos)
    {
        if(playerSpaceship == null)
            playerSpaceship = GameManager.instance.PlayerSpaceship;

        fireDirection = playerSpaceship.transform.up;
        transform.position = _spawnPos;
        gameObject.SetActive(true);
        firedTime = Time.time;
        isFired = true;
    }

    public void Reset()
    {
        ObjectPooler_Sonu.instance.ResetBackPooledObject(gameObject);
        isFired = false;
    }
}