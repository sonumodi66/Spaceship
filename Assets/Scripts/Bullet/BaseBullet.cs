using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour, IPoolableObject
{
    [SerializeField] BulletData bulletData;

    private float resetDuration = 2f;
    private float firedTime;
    private bool isFired;

    private Vector3 fireDirection;
    Transform child;
    
    private PlayerSpaceship playerSpaceship;

    public BulletData BulletData { get { return bulletData; } }
    //----------------------------------------------------------------------------------
    private void Start()
    {
        playerSpaceship = GameManager.instance.PlayerSpaceship;
        child = transform.GetChild(0);
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
        transform.Translate(fireDirection * BulletData.movingSpeed * Time.deltaTime);
    }

    //-------------------------------------------------------------------------------
    public GameObject itsGameObject => this.gameObject;

    public void Spawn(Vector3 _spawnPos, Vector3 _movingDirection)
    {
        if(playerSpaceship == null)
            playerSpaceship = GameManager.instance.PlayerSpaceship;

        if(child == null)
            child = transform.GetChild(0);

        child.rotation = Quaternion.identity;
        child.rotation = Quaternion.Euler(0f, 0f, child.eulerAngles.z + playerSpaceship.transform.eulerAngles.z);

        fireDirection = _movingDirection;
        fireDirection.z = 0;
        transform.position = _spawnPos;
        gameObject.SetActive(true);
        firedTime = Time.time;
        isFired = true;
    }

    public void Reset()
    {
        ObjectPoolManager.instance.ResetBackPooledObject(gameObject);
        isFired = false;
    }
}