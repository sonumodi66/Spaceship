using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class BackgroundsRepeater : MonoBehaviour
{
    [SerializeField] private Transform[] backgrounds;

    private PlayerSpaceship playerSpaceship;


    private Coroutine checkForBackgroundPos_Coroutine;
    private int currentBgIndex = 0;

    private const float BG_GAP = 23f;
    private const float OFFSET = 5f;

    private float checkDuration = 1f;
    //-----------------------------------------------------------------
    private void Start()
    {
        playerSpaceship = GameManager.instance.PlayerSpaceship;
        checkForBackgroundPos_Coroutine = StartCoroutine(CheckForBackgroundPos_Enum());
    }

    IEnumerator CheckForBackgroundPos_Enum()
    {
        yield return new WaitForEndOfFrame();

        while (true)
        {
            if (playerSpaceship.transform.position.y > backgrounds[currentBgIndex].position.y + BG_GAP/2f + OFFSET)
            {
                backgrounds[currentBgIndex].position = new Vector3(0f, OtherBackground().position.y + BG_GAP,
                    OtherBackground().position.z);

                currentBgIndex = (currentBgIndex == 0) ? 1 : 0;
            }

            yield return new WaitForSeconds(checkDuration);
        }
    }

    Transform OtherBackground()
    {
        return currentBgIndex == 0 ? backgrounds[1] : backgrounds[0];
    }
}