using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowSpaceship : MonoBehaviour
{
    [Header("Hierarachy References")] [SerializeField]
    private PlayerSpaceship playerSpaceship;

    [Header("RunTime Uses")] float smoothFollowFactor = 0.3f;

    private float initialOffset;
    private float zPos;
    private float distanceFactor;

    //------------------------------------------------------------------------------
    private void Start()
    {
        zPos = transform.position.z;

        if (playerSpaceship == null)
            playerSpaceship = FindObjectOfType<PlayerSpaceship>();

        initialOffset = playerSpaceship.transform.position.y - transform.position.y;
    }

    private void LateUpdate()
    {
        FollowPlayerSpaceship();
    }

    private void FollowPlayerSpaceship()
    {
        float distance = Vector3.Distance(playerSpaceship.transform.position, transform.position);

        if (distance < 1f)
            distanceFactor = 1f;
        else
            distanceFactor = distance;

        float xClamp = Mathf.Clamp(transform.position.x, -1f, 1f);
        
        Vector3 targetPos = new Vector3
        (
            // 0f,
            playerSpaceship.transform.position.x,
            // Mathf.Clamp(playerSpaceship.transform.position.x, -1f, 1f),
            playerSpaceship.transform.position.y - initialOffset,
            zPos
        );

        transform.position = Vector3.Lerp(transform.position, targetPos,
            distanceFactor * smoothFollowFactor * Time.deltaTime);
    }
}