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

    bool canMove;
    bool hasCoinsPooled;
    Vector3 direction;
    float moveSpeed;

    //---------------------------------------------------------------------
    private void OnEnable()
    {
        GameCinematicController.onCinemeticCompleted += StartCoinPoolAndMovement;
    }
    private void OnDisable()
    {
        GameCinematicController.onCinemeticCompleted -= StartCoinPoolAndMovement;
    }

    private void Start()
    {
        Invoke(nameof(PoolCoins), 10f);
    }
    private void Update()
    {
        if (!canMove || !hasCoinsPooled) return;

        transform.Translate(direction * Time.deltaTime * moveSpeed);
    }
    //---------------------------------------------------------------------
    void StartCoinPoolAndMovement()
    {
        canMove = true;
    }

    private void ResetCoinsBack()
    {
        if (coinsList.Count <= 0) return;

        int coinsAmount = Random.Range(coinsList.Count - 4, coinsList.Count + 1);

        for (int i = 0; i < coinsAmount; i++)
        {
            if (coinsList[i] != null)
                coinsList[i].itsGameObject.SetActive(true);
        }

        for (int i = coinsAmount; i < coinsList.Count; i++)
        {
            if (coinsList[i] != null)
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
            tempCoin = ObjectPoolManager.instance.GetPooledObject(coinPrefab.name);
            coinsList.Add(tempCoin);

            tempCoin.itsGameObject.transform.parent = this.transform;
            tempCoin.Spawn(new Vector3(0f, i, 0f), Vector3.zero);
        }

        ResetCoinsBack();

        hasCoinsPooled = true;
    }
    //---------------------------------------------------------------------
    public void Reset()
    {
        if (!canMove) return;

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