using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpaceship : MonoBehaviour
{
    [SerializeField] private SpaceshipData spaceShipData;

    [Header("Child References")]
    [SerializeField] private Transform spaceShipBulletFirePos;
    [SerializeField] private GameObject spaceshipArmor;
    [SerializeField] private GameObject crescenmoonFirer;

    [Header("Assets References")]
    [SerializeField] private BaseBullet basicBulletPrefab;
    [SerializeField] private BaseBullet triAngledBulletPrefab;
    [SerializeField] private BaseBullet crescentBulletPrefab;
    [SerializeField] private BaseBullet shockWaveBulletPrefab;

    [Header("RunTime Uses")]
    [SerializeField] WeaponType weaponType;
    [SerializeField] bool canPlayerDie;

    public static Action onCoinCollected;
    public static Action onPlayerSpaceshipDied;
    public static Action<int> onPlayerHealthChanged;

    private float moveSpeed = 10f;
    private float rotSpeed = 10f;
    private float fireGap = 0.2f;
    private float armorDuration = 5f;
    private float crescentWeaponDuration = 10f;
    private int damageAmount = 10;

    private float xClampLimit = 18f;
    private float yClampLimit = 12f;

    private float horizontal;
    private float vertical;

    float lastFiredTime;

    int healthAmount = 100;

    private bool isArmorActive = false;
    bool canPlayerBeControlled;

    private Rigidbody rigidbodyComp;
    private Transform shipMesh;
    //-----------------------------------------------------------------------------------------------------
    private void OnEnable()
    {
        GameCinematicController.onCinemeticCompleted += EnablePlayerToBeControlled;
    }
    private void OnDisable()
    {
        GameCinematicController.onCinemeticCompleted -= EnablePlayerToBeControlled;
    }

    private void Start()
    {
        rigidbodyComp = GetComponent<Rigidbody>();
        shipMesh = transform.GetChild(0);

        GetDataAndDetails();

        onPlayerHealthChanged?.Invoke(healthAmount);
    }


    private void FixedUpdate()
    {
        if (!canPlayerBeControlled) return;

        ControlSpaceship();
        ClampSpaceshipPositon();
    }

    private void Update()
    {
        if (!canPlayerBeControlled) return;

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        vertical = Mathf.Clamp(vertical, -0.1f, 1f);

        FireBullet();
    }

    //-----------------------------------------------------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        ICollectable tempCollectable;

        if (other.GetComponent<ICollectable>() != null)
        {
            tempCollectable = other.GetComponent<ICollectable>();
            tempCollectable.Collect();

            if (tempCollectable.itsGameObject.GetComponent<Coin>() != null)
                onCoinCollected?.Invoke();

            if (tempCollectable.itsGameObject.GetComponent<Armour>() != null)
                ActivateSpaceshipArmor();

            if (tempCollectable.itsGameObject.GetComponent<CrescentArmPowerup>() != null)
                ActivateCrescentSameWeapon();
        }

        if (other.GetComponent<Asteroid>() != null)
        {
            if (!isArmorActive)
            {
                TakeDamage();
                other.GetComponent<IPoolableObject>().Reset();
            }
            else
            {
                other.GetComponent<IPoolableObject>().itsGameObject.GetComponent<Asteroid>().DestroyAndInstantiatSmallPieces(false);
            }
        }
    }

    //-----------------------------------------------------------------------------------------------------
    void EnablePlayerToBeControlled()
    {
        canPlayerBeControlled = true;
    }
    void GetDataAndDetails()
    {
        healthAmount = spaceShipData.fullHealth;
        damageAmount = spaceShipData.spaceshipDamageRate;
    }
    //-----------------------------------------------------------------------------------------------------
    /// <summary>
    ///Controling of Player spaceship as per provided input
    /// </summary>
    private void ControlSpaceship()
    {
        Quaternion targetRot =
            Quaternion.Euler
            (
            vertical * spaceShipData.rotationSpeed,
            -horizontal * spaceShipData.rotationSpeed,
            transform.rotation.eulerAngles.z - horizontal * spaceShipData.rotationSpeed
            );

        rigidbodyComp.MoveRotation(Quaternion.Lerp(transform.rotation, targetRot, rotSpeed));
        rigidbodyComp.velocity = transform.up * spaceShipData.moveSpeed * vertical;
    }

    /// <summary>
    /// Clamp player movement limit as per defined values
    /// </summary>
    private void ClampSpaceshipPositon()
    {
        float clampedXpos = Mathf.Clamp(transform.position.x, -xClampLimit, xClampLimit);
        float clampedYpos = Mathf.Clamp(transform.position.y, -yClampLimit, yClampLimit);
        rigidbodyComp.position = new Vector3(clampedXpos, clampedYpos, 0f);
    }

    void FireBullet()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (Time.time > (lastFiredTime + spaceShipData.fireGap))
            {
                FireAsPerWeaponType();
                lastFiredTime = Time.time;
            }
        }
    }

    /// <summary>
    /// Fire bullet as per selected weapon type 
    /// </summary>
    private void FireAsPerWeaponType()
    {
        Vector3 dir = spaceShipBulletFirePos.position - transform.position;

        switch (weaponType)
        {
            case WeaponType.BASIC_BULLET:
                for (int i = 0; i < basicBulletPrefab.BulletData.oneShotBulletsCount; i++)
                    Fire(i, dir, basicBulletPrefab.name);
                break;

            case WeaponType.TRI_ANGLED_BULLET:
                for (int i = 0; i < triAngledBulletPrefab.BulletData.oneShotBulletsCount; i++)
                {
                    Fire(i, dir, triAngledBulletPrefab.name);
                    Vector3 angledDir = Quaternion.AngleAxis(triAngledBulletPrefab.BulletData.angleValue, transform.forward) * dir;

                    Fire(i, angledDir, triAngledBulletPrefab.name);
                    angledDir = Quaternion.AngleAxis(triAngledBulletPrefab.BulletData.angleValue, -transform.forward) * dir;
                    Fire(i, angledDir, triAngledBulletPrefab.name);
                }
                break;

            case WeaponType.CRESCENT_MOON_BULLET:
                for (int i = 0; i < crescentBulletPrefab.BulletData.oneShotBulletsCount; i++)
                    Fire(i, dir, crescentBulletPrefab.name);
                break;

            case WeaponType.SHOCK_WAVE_BULLET:
                for (int i = 0; i < shockWaveBulletPrefab.BulletData.oneShotBulletsCount; i++)
                    Fire(i, dir, shockWaveBulletPrefab.name);
                break;

            default:
                Debug.Log("No Bullet Data");
                break;
        }
    }

    private void Fire(int i, Vector3 _direction, string _bulletName)
    {
        Vector3 firePos = spaceShipBulletFirePos.position;
        firePos += (_direction) * i * 0.6f;
        firePos.z = 0;

        ObjectPoolManager.instance.GetPooledObject(_bulletName).Spawn(firePos, _direction);
    }

    /// <summary>
    /// Taking player damage and reducing health
    /// </summary>
    void TakeDamage()
    {
        if (healthAmount > 0)
        {
            healthAmount -= damageAmount;
        }

        if (healthAmount <= 0)
        {
            healthAmount = 0;
            DiePlayerSpaceship();
        }

        onPlayerHealthChanged?.Invoke(healthAmount);
    }

    void DiePlayerSpaceship()
    {
        if (canPlayerDie)
        {
            onPlayerSpaceshipDied?.Invoke();
            gameObject.SetActive(false);
        }
    }
    //---------------------------------------------------------------------------------------------
    void ActivateSpaceshipArmor()
    {
        spaceshipArmor.SetActive(true);
        isArmorActive = true;
        Invoke(nameof(DeactivateArmorAfterTime), spaceShipData.armorDuration);
    }

    void DeactivateArmorAfterTime()
    {
        if (gameObject.activeInHierarchy)
        {
            spaceshipArmor.SetActive(false);
            isArmorActive = false;
        }
    }

    void ActivateCrescentSameWeapon()
    {
        crescenmoonFirer.SetActive(true);
        weaponType = WeaponType.CRESCENT_MOON_BULLET;
        Invoke(nameof(DeactivateCrescentMoonWeapon), spaceShipData.crescentWeaponDuration);
    }

    void DeactivateCrescentMoonWeapon()
    {
        if (gameObject.activeInHierarchy)
        {
            weaponType = WeaponType.BASIC_BULLET;
            crescenmoonFirer.SetActive(false);
        }
    }
    //---------------------------------------------------------------------------------------------
    public void ChangeWeapon(int _weaponType)
    {
        if (weaponType == WeaponType.CRESCENT_MOON_BULLET)
            return;

        weaponType = (WeaponType)_weaponType;
    }

}