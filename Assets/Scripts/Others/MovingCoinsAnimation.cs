using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class MovingCoinsAnimation : MonoBehaviour
{
    [Header("Child References")] [SerializeField]
    private Transform[] movingCoins;

    [Header("Hierarachy References")] [SerializeField]
    private RectTransform coinHUD;

    [SerializeField] private bool useScreenUI;

    private PlayerSpaceship playerSpaceship;
    private Camera mainCam;

    private int coinIndex;
    //------------------------------------------------------------------------
    private void OnEnable()
    {
        PlayerSpaceship.onCoinCollected += ShowMovingCoinsAnimation;
    }

    private void OnDisable()
    {
        PlayerSpaceship.onCoinCollected -= ShowMovingCoinsAnimation;
    }

    private void Start()
    {
        mainCam = Camera.main;
        playerSpaceship = GameManager.instance.PlayerSpaceship;
    }

    public void ShowMovingCoinsAnimation()
    {
        _ = ShowMovingCoins(playerSpaceship.transform.position);
    }

    //------------------------------------------------------------------------
    async UniTask ShowMovingCoins(Vector3 _stratPos)
    {
        movingCoins[coinIndex].transform.position = _stratPos;
        movingCoins[coinIndex].gameObject.SetActive(true);
        var j = coinIndex;

        Vector3 targetPos = coinHUD.position;

        if (useScreenUI)
        {
            Vector3 hudPos = coinHUD.position;
            hudPos.z = -mainCam.transform.position.z;
            targetPos = mainCam.ScreenToWorldPoint(hudPos);
        }

        movingCoins[coinIndex].DOMove(targetPos, 0.5f).OnComplete(() => movingCoins[j].gameObject.SetActive(false));

        coinIndex++;
        if (coinIndex >= movingCoins.Length - 1)
            coinIndex = 0;
    }
}