using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour, IPoolableObject, IResetable
{
    [Header("Number of Pieces to Break into")] [Range(2, 4)]
    [SerializeField] int breakingCount = 2;

    [Header("Assets References")]
    [SerializeField]
    private GameParticles breakingSmoke;

    [SerializeField] private GameParticles breakingSmall;

    private PlayerSpaceship playerSpaceship;
    private Rigidbody rigidbodyComp;

    private bool isStartedMoving;

    private bool canGivePoint;
    private string[] asteroidNames = { "Asteroid 1", "Asteroid 2", "Asteroid 3" };

    private IPoolableObject tempAsteroid;
    Vector3 movingDir;
    //-------------------------------------------------------------------------------------
    private void Start()
    {
        playerSpaceship = GameManager.instance.PlayerSpaceship;
    }

    //-------------------------------------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BaseBullet>() != null)
        {
            if (!canGivePoint)
            {
                for (int i = 0; i < breakingCount; i++)
                {
                    DestroyAndInstantiatSmallPieces(true);
                }
            }
            else
            {
                ObjectPoolManager.instance.GetPooledObject(breakingSmall.name).Spawn(transform.position, Vector3.zero);
                Reset();
                GameManager.instance.IncrementPoints();

                Reset();
            }

            other.GetComponent<IPoolableObject>().Reset();
        }
    }


    //------------------------------------------------------------------------------
    /// <summary>
    /// Set as per small pieces of the asteroids
    /// </summary>
    void SetAsSmallBronkenAsteroid()
    {
        if (rigidbodyComp == null) rigidbodyComp = GetComponent<Rigidbody>();


        rigidbodyComp.velocity = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0f) * Random.Range(2f, 5f);

        transform.localScale = transform.localScale / 2f;
        canGivePoint = true;
    }

    public void DestroyAndInstantiatSmallPieces(bool _isInstantiateSmallPieces)
    {
        if (_isInstantiateSmallPieces)
        {
            tempAsteroid =
                ObjectPoolManager.instance.GetPooledObject(
                    asteroidNames[Random.Range(0, asteroidNames.Length)]);
            tempAsteroid.Spawn(transform.position, Vector3.zero);
            tempAsteroid.itsGameObject.GetComponent<Asteroid>().SetAsSmallBronkenAsteroid();
        }

        ObjectPoolManager.instance.GetPooledObject(breakingSmoke.name).Spawn(transform.position, movingDir);
        Reset();
    }

    //-------------------------------------------------------------------------------
    public GameObject itsGameObject => this.gameObject;

    public void Spawn(Vector3 _spawnPos, Vector3 _movingDirection)
    {
        transform.position = _spawnPos;
        gameObject.SetActive(true);

        transform.localScale = Vector3.one * Random.Range(0.3f, 0.7f);

        if (rigidbodyComp == null) rigidbodyComp = GetComponent<Rigidbody>();

        rigidbodyComp.angularVelocity = Random.insideUnitSphere * Random.Range(0.2f, 1f);
        movingDir = _movingDirection * Random.Range(0.5f, 1f);
        movingDir.z = 0f;
        rigidbodyComp.velocity = movingDir * Random.Range(0.05f, 0.3f);

        isStartedMoving = true;
    }

    public void Reset()
    {
        rigidbodyComp.angularVelocity = Vector3.zero;
        rigidbodyComp.velocity = Vector3.zero;

        ObjectPoolManager.instance.ResetBackPooledObject(gameObject);
        isStartedMoving = false;
        canGivePoint = false;
    }
}