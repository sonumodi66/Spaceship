using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class InGameObjectsManager : MonoBehaviour
{
    [Header("Childs References")]
    [SerializeField] Transform[] instantiationPoints;

    [Header("Assets References")]
    [SerializeField] private Asteroid[] asteroidPrefabs;

    [SerializeField] private Armor spaceshipArmorPrefab;
    [SerializeField] private CrescentArmPowerup crescentArmPowerup;

    private PlayerSpaceship playerSpaceship;

    private bool isGenerateAsteroids;
    private bool isGenerateArmor;


    private float farFromPlayer = 10f;
    private float asteroidInstInterval = 1f;
    private int armorInstInterval = 15;

    private float asteroidXRange = 5f;
    private float armorXRange = 3f;

    //----------------------------------------------------------------------------------
    private void Start()
    {
        playerSpaceship = GameManager.instance.PlayerSpaceship;
        isGenerateAsteroids = true;
        isGenerateArmor = true;

        HideInstantiationPointsMeshes();
        _ = GenerateAsteroidObstacles();
        _ = GenerateSpaceshipArmorAndCrescentMoor();
    }
    //----------------------------------------------------------------------------------
    void HideInstantiationPointsMeshes()
    {
        for (int i = 0; i < instantiationPoints.Length; i++)
        {
            instantiationPoints[i].GetComponent<MeshRenderer>().enabled = false;
        }
    }

    async UniTask GenerateAsteroidObstacles()
    {
        await UniTask.Delay(1000);

        while (isGenerateAsteroids)
        {
            SpawnPooledObject(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)].name, asteroidXRange);
            int delay = (int)asteroidInstInterval * 1000;
            await UniTask.Delay(500);
        }
    }

    async UniTask GenerateSpaceshipArmorAndCrescentMoor()
    {
        await UniTask.Delay(5000);
        while (isGenerateArmor)
        {
            if (Random.Range(0, 2f) == 0)
                SpawnPooledObject(spaceshipArmorPrefab.name, armorXRange);
            else
                SpawnPooledObject(crescentArmPowerup.name, armorXRange);

            await UniTask.Delay(armorInstInterval * 1000);
        }
    }

    //----------------------------------------------------------------------------------
    IPoolableObject SpawnPooledObject(string _objName, float _xRange)
    {
        int random = Random.Range(0, instantiationPoints.Length);
        int opposite = (random <= 3) ? (random + 4) : (random - 4);

        if (opposite > 0 && opposite < instantiationPoints.Length - 1)
        {
            if (Random.Range(0, 2) == 0) opposite++;
            else opposite--;
        }

        Vector3 insPos = instantiationPoints[random].position;

        Vector3 direction = instantiationPoints[opposite].position - insPos;

        var tempPooled = ObjectPooler_Sonu.instance.GetPooledObject(_objName);

        tempPooled.Spawn(insPos, direction);
        return tempPooled;
    }
}