using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// It resets the IResetable if its enough away from player
/// </summary>
public class ObjectReseter : MonoBehaviour
{
    [SerializeField] private float checkDuration = 5f;
    private Coroutine checkForReseting_Coroutine;

    private IResetable iResetable;

    private PlayerSpaceship playerSpaceship;

    //----------------------------------------------------------------
    private void OnEnable()
    {
        checkForReseting_Coroutine = StartCoroutine(CheckForReseting_Enum());
    }

    private void Start()
    {
        GetBitDelayed();
    }


    private void OnDisable()
    {
        if (checkForReseting_Coroutine != null)
            StopCoroutine(checkForReseting_Coroutine);
    }

    //----------------------------------------------------------------
    private void GetBitDelayed()
    {
        if (iResetable == null)
            iResetable = GetComponent<IResetable>();

        if (playerSpaceship == null)
            playerSpaceship = GameManager.instance.PlayerSpaceship;
    }

    //----------------------------------------------------------------

    IEnumerator CheckForReseting_Enum()
    {
        yield return new WaitForSeconds(checkDuration);

        while (true && gameObject.activeInHierarchy)
        {
            bool canReset = gameObject.activeInHierarchy &&
                            iResetable != null &&
                            Vector3.Distance(transform.position, playerSpaceship.transform.position) > 20f;

            if (canReset) iResetable.Reset();
            yield return new WaitForSeconds(checkDuration);
        }
    }
}