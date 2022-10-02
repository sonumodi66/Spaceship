using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CoinsPattern : MonoBehaviour, IResetable
{
    [SerializeField] private Coin coinPrefab;
    [SerializeField] InGameObjectsManager ingameObjManager;

    private List<IPoolableObject> coinsList = new List<IPoolableObject>();
    private int coinsAmountToPool = 10;
    private PlayerSpaceship playerSpaceship;

    bool canMove;
    Vector3 direction;
    float moveSpeed;


    //---------------------------------------------------------------------
    private void Start()
    {
        playerSpaceship = GameManager.instance.PlayerSpaceship;
        Invoke(nameof(PoolCoins), 1f);
    }

    private void Update()
    {
        transform.Translate(direction * Time.deltaTime * moveSpeed);
    }
    //---------------------------------------------------------------------
    private void ResetCoinsBack()
    {
        int coinsAmount = Random.Range(coinsList.Count - 4, coinsList.Count + 1);
        
        for (int i = 0; i < coinsAmount; i++)
        {
            coinsList[i].itsGameObject.SetActive(true);
        }

        for (int i = coinsAmount; i < coinsList.Count; i++)
        {
            coinsList[i].itsGameObject.SetActive(false);
        }

        InitCoinMovement();
    }
    //---------------------------------------------------------------------
    private void PoolCoins()
    {
        coinsList.Clear();
        IPoolableObject tempCoin;

        for (int i = 0; i < coinsAmountToPool; i++)
        {
            tempCoin = ObjectPooler_Sonu.instance.GetPooledObject(coinPrefab.name);
            coinsList.Add(tempCoin);

            tempCoin.itsGameObject.transform.parent = this.transform;
            tempCoin.Spawn(new Vector3(0f, i, 0f), Vector3.zero);
        }
        
        ResetCoinsBack();
    }
    //---------------------------------------------------------------------
    public void Reset()
    {
        canMove = false;
        ResetCoinsBack();
    }

    public void InitCoinMovement()
    {
        Vector3 insPos, dir;
        ingameObjManager.GetRandomPointAndDirection(out insPos, out dir);

        transform.position = insPos;
        direction = dir;
        canMove = true;
        moveSpeed = Random.Range(0.05f, 0.2f);
    }
}