using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class InGameObjectsManager : MonoBehaviour
{
    [Header("Toughness and Generation Data")]
    [SerializeField] ToughnessAndGenerationDetails toughnessDetails;

    [Header("Hierarachy References")]
    [SerializeField] Transform[] instantiationPoints;

    [Header("Assets References")]
    [SerializeField] private Asteroid[] asteroidPrefabs;
    [SerializeField] private Armor spaceshipArmorPrefab;
    [SerializeField] private CrescentArmPowerup crescentArmPowerup;

    private bool isGenerateAsteroids;
    private bool isGenerateArmor;
    private bool isGenerateCrescentBulletPowerup;

    private int asteroidInstInterval = 1000; //mili seconds

    float lastTimeRateChanged_Asteroid;
    float lastTimeRateChanged_armor;
    float lastTimeRateChanged_crescentPowerup;

    private float asteroidXRange = 5f;
    private float armorXRange = 3f;

    //----------------------------------------------------------------------------------
    private void OnEnable()
    {
        GameCinematicController.onCinemeticCompleted += StartGeneratingRequiredObject;
    }
    private void OnDisable()
    {
        GameCinematicController.onCinemeticCompleted -= StartGeneratingRequiredObject;
    }
    private void Start()
    {
        isGenerateAsteroids = true;
        isGenerateArmor = true;
        isGenerateCrescentBulletPowerup = true;

        HideInstantiationPointsMeshes();

    }

    private void StartGeneratingRequiredObject()
    {
        _ = GenerateAsteroidObstacles();
        _ = GenerateArmorPowerup();
        _ = GenerateCrescentBulletPowerup();
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

            if (asteroidInstInterval < 100)
                asteroidInstInterval = 100;

            if (Time.time > (lastTimeRateChanged_Asteroid + toughnessDetails.incrementTimeRate))
            {
                asteroidInstInterval -= toughnessDetails.incrementRate;
                lastTimeRateChanged_Asteroid = Time.time;
            }

            await UniTask.Delay(asteroidInstInterval);
        }
    }

    async UniTask GenerateArmorPowerup()
    {
        await UniTask.Delay(toughnessDetails.armorGenerationRate * 1000);

        while (isGenerateArmor)
        {
            SpawnPooledObject(spaceshipArmorPrefab.name, armorXRange);

            int waitDuration =
                toughnessDetails.isRandmizeArmorRate ?
                Random.Range(toughnessDetails.armorGenerationRate - 3, toughnessDetails.armorGenerationRate + 3) :
                toughnessDetails.armorGenerationRate;

            await UniTask.Delay(waitDuration * 1000);
        }
    }

    async UniTask GenerateCrescentBulletPowerup()
    {
        await UniTask.Delay(toughnessDetails.crescentPowerupRate * 1000);

        while (isGenerateCrescentBulletPowerup)
        {
            SpawnPooledObject(crescentArmPowerup.name, armorXRange);

            int waitDuration = 
                toughnessDetails.isRandmizeCrescentRate ? 
                Random.Range(toughnessDetails.crescentPowerupRate - 3, toughnessDetails.crescentPowerupRate + 3) : 
                toughnessDetails.crescentPowerupRate;

            await UniTask.Delay(waitDuration * 1000);
        }
    }

    //----------------------------------------------------------------------------------
    IPoolableObject SpawnPooledObject(string _objName, float _xRange)
    {
        Vector3 insPos, direction;
        GetRandomPointAndDirection(out insPos, out direction);

        var tempPooled = ObjectPoolManager.instance.GetPooledObject(_objName);

        tempPooled.Spawn(insPos, direction);
        return tempPooled;
    }

    public void GetRandomPointAndDirection(out Vector3 insPos, out Vector3 direction)
    {
        int random = Random.Range(0, instantiationPoints.Length);
        int opposite = (random <= 3) ? (random + 4) : (random - 4);

        if (opposite > 0 && opposite < instantiationPoints.Length - 1)
        {
            if (Random.Range(0, 2) == 0) opposite++;
            else opposite--;
        }

        insPos = instantiationPoints[random].position;
        direction = instantiationPoints[opposite].position - insPos;
    }
}