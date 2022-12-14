using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// EnvironmentController is for Scrolling sky
/// </summary>
public class EnvironmentController : MonoBehaviour
{
    [SerializeField] private float skyboxSpeed = 1f;

    private Material skyBoxMat;
    
    private void Start()
    {
        skyBoxMat = RenderSettings.skybox;
    }

    private void Update()
    {
        skyBoxMat.SetFloat("_Rotation", Time.time * skyboxSpeed);
    }
}