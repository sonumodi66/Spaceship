using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour, IPoolableObject, IResetable
{
    [Header("Assets References")] [SerializeField]
    private GameParticles breakingSmoke;

    [SerializeField] private GameParticles breakingSmall;

    private PlayerSpaceship playerSpaceship;
    private Rigidbody rigidbodyComp;

    private float lastCheckedAt;
    private float recheckAfter = 5f;
    private bool isStartedMoving;

    private bool canGivePoint;
    private string[] asteroidNames = {"Asteroid 1", "Asteroid 2", "Asteroid 3"};

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
        if (other.GetComponent<Bullet>() != null)
        {
            if (!canGivePoint)
            {
                for (int i = 0; i < 2; i++)
                {
                    DestroyAndInstantiateTwoPieces(i, true);
                }
            }
            else
            {
                ObjectPooler_Sonu.instance.GetPooledObject(breakingSmall.name).Spawn(transform.position, Vector3.zero);
                Reset();
                GameManager.instance.IncrementPoints();

                Reset();
            }

            other.GetComponent<IPoolableObject>().Reset();
        }
    }


    //------------------------------------------------------------------------------
    public void SetAsSmallBronkenAsteroid(int _num)
    {
        if (rigidbodyComp == null) rigidbodyComp = GetComponent<Rigidbody>();

        float direction = 1f;
        if (_num == 0)
            direction = 1f;
        else
            direction = -1f;

        rigidbodyComp.velocity = new Vector3(direction * Random.Range(0.1f, 0.4f), -1f, 0f) * Random.Range(2f, 5f);

        transform.localScale = transform.localScale / 2f;
        canGivePoint = true;
    }

    private void CheckIfNeedsToBeReset()
    {
        if (!isStartedMoving)
            return;

        if (Time.time < (lastCheckedAt + recheckAfter))
            return;

        float distane = Vector3.Distance(playerSpaceship.transform.position, transform.position);
        bool isDown = transform.position.y < playerSpaceship.transform.position.y;

        bool isReset = Time.time > (lastCheckedAt + recheckAfter);

        if (isReset && isDown && (distane > 4f))
        {
            Reset();
        }

        lastCheckedAt = Time.time;
    }

    public void DestroyAndInstantiateTwoPieces(int i, bool _isInstantiateSmallPieces)
    {
        if (_isInstantiateSmallPieces)
        {
            tempAsteroid =
                ObjectPooler_Sonu.instance.GetPooledObject(
                    asteroidNames[Random.Range(0, asteroidNames.Length)]);
            tempAsteroid.Spawn(transform.position, Vector3.zero);
            tempAsteroid.itsGameObject.GetComponent<Asteroid>().SetAsSmallBronkenAsteroid(i);
        }

        ObjectPooler_Sonu.instance.GetPooledObject(breakingSmoke.name).Spawn(transform.position, movingDir);
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

        ObjectPooler_Sonu.instance.ResetBackPooledObject(gameObject);
        isStartedMoving = false;
        canGivePoint = false;
    }
}