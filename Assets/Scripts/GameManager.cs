using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Hierarachy References")] 
    [SerializeField] private UIManager uiManager;
    
    [Header("Runtime Uses")]
    [SerializeField] private int coinsAmount;
    [SerializeField] private int gamePointsCount;
    
    int pointsIncrementRate = 5;
    
    private PlayerSpaceship playerSpaceship;
    public PlayerSpaceship PlayerSpaceship
    {
        get
        {
            if (playerSpaceship == null)
                playerSpaceship = FindObjectOfType<PlayerSpaceship>();

            return playerSpaceship;
        }
    }

    //-----------------------------------------------------------------------------
    private void OnEnable()
    {
        PlayerSpaceship.onCoinCollected += IncrementCoin;
    }

    private void OnDisable()
    {
        PlayerSpaceship.onCoinCollected -= IncrementCoin;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
       
    }
    //-----------------------------------------------------------------------------
    public void IncrementCoin()
    {
        coinsAmount++;
        uiManager.UpdateCoinsAmount(coinsAmount);
    }

    public void IncrementPoints()
    {
        gamePointsCount += pointsIncrementRate;
        uiManager.UpdatePointsText(gamePointsCount);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
