using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstaclesManager : MonoBehaviour
{
    [Header("Assets References")] [SerializeField]
    private Asteroid[] asteroidPrefabs;

    [SerializeField] private Armor spaceshipArmorPrefab;

    private PlayerSpaceship playerSpaceship;

    private bool isGenerateAsteroids;
    private bool isGenerateArmor;
    
    
    private float farFromPlayer = 10f;
    private int asteroidInstInterval = 2;
    private int armorInstInterval = 15;

    private float asteroidXRange = 5f;
    private float armorXRange = 3f;

    //----------------------------------------------------------------------------------
    private void Start()
    {
        playerSpaceship = GameManager.instance.PlayerSpaceship;
        isGenerateAsteroids = true;
        isGenerateArmor = true;

        _ = GenerateAsteroidObstacles();
        _ = GenerateSpaceshipArmor();
    }
    //----------------------------------------------------------------------------------

    async UniTask GenerateAsteroidObstacles()
    {
        await UniTask.Delay(1000);
        
        while (isGenerateAsteroids)
        {
            SpawnPooledObject(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)].name, asteroidXRange);
            await UniTask.Delay(asteroidInstInterval * 1000);
        }
    }

    async UniTask GenerateSpaceshipArmor()
    {
        await UniTask.Delay(5000);
        while (isGenerateArmor)
        {
            SpawnPooledObject(spaceshipArmorPrefab.name, armorXRange);
            await UniTask.Delay(armorInstInterval * 1000);
        }
    }

    //----------------------------------------------------------------------------------
    IPoolableObject SpawnPooledObject(string _objName, float _xRange)
    {
        Vector3 insPos = new Vector3(Random.Range(-_xRange, _xRange),
            playerSpaceship.transform.position.y + farFromPlayer, 0f);

        var tempArmor = ObjectPooler_Sonu.instance.GetPooledObject(_objName);

        tempArmor.Spawn(insPos);
        return tempArmor;
    }
}