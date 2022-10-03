using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// GameCinematicController is for showing Cinemetic effect at start of the game
/// </summary>
public class GameCinematicController : MonoBehaviour
{
    [SerializeField] PlayerSpaceship playerSpaceship;
    [SerializeField] Camera camera;

    public static Action onCinemeticCompleted;

    private void Start()
    {
        StartCinemeticProcess();
    }

    private void StartCinemeticProcess()
    {
        playerSpaceship.transform.DOMoveX(0f, 2f);
        camera.transform.DOMoveX(0f, 2.5f).OnComplete(SetCameraAndPlayerToPlayingState);
    }

    private void SetCameraAndPlayerToPlayingState()
    {
        float fView = camera.fieldOfView;
        DOTween.To(() => fView, x => fView = x, 60, 2f)
            .OnUpdate(() =>
            {
                camera.fieldOfView = fView;
            });

        float zRot = playerSpaceship.transform.eulerAngles.z;
        DOTween.To(() => zRot, x => zRot = x, 0, 2f)
            .OnComplete(InvokeEventWhenCompleted)
            .OnUpdate(() =>
            {
                playerSpaceship.transform.rotation = Quaternion.Euler(0f, 0f, zRot);
            });
    }

    private void InvokeEventWhenCompleted()
    {
        onCinemeticCompleted?.Invoke();
    }
}
