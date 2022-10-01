using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CoinsPattern : MonoBehaviour, IResetable
{
    [SerializeField] private Coin coinPrefab;

    private List<IPoolableObject> coinsList = new List<IPoolableObject>();

    private int coinsAmountToPool = 10;

    private PlayerSpaceship playerSpaceship;
    //---------------------------------------------------------------------
    private void Start()
    {
        playerSpaceship = GameManager.instance.PlayerSpaceship;
        Invoke(nameof(PoolCoins), 1f);
    }

    //---------------------------------------------------------------------
    public void ResetCoinsBack()
    {
        transform.position = new Vector3(Random.Range(-3f, 3f), playerSpaceship.transform.position.y + 10f, 0f);
        int coinsAmount = Random.Range(coinsList.Count - 4, coinsList.Count + 1);
        
        for (int i = 0; i < coinsAmount; i++)
        {
            coinsList[i].itsGameObject.SetActive(true);
        }

        for (int i = coinsAmount; i < coinsList.Count; i++)
        {
            coinsList[i].itsGameObject.SetActive(false);
        }
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
            tempCoin.Spawn(new Vector3(0f, i, 0f));
        }
        
        ResetCoinsBack();
    }
    //---------------------------------------------------------------------
    public void Reset()
    {
        ResetCoinsBack();
    }
}