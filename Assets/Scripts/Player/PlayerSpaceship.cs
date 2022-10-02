using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerSpaceship : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotSpeed = 10f;
    [SerializeField] float fireGap = 0.2f;
    [SerializeField] private float armorDuration = 5f;
    [SerializeField] private float crescentWeaponDuration = 10f;
    [SerializeField] private int damageAmount = 10;

    [Header("Child References")]
    [SerializeField] private Transform spaceShipBulletFirePos;
    [SerializeField] private GameObject spaceshipArmor;
    [SerializeField] private GameObject crescenmoonFirer;

    [Header("Assets References")]
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Bullet crescentBulletPrefab;
    [SerializeField] private Bullet shockWaveBulletPrefab;

    [Header("RunTime Uses")]
    [SerializeField] WeaponType weaponType;

    public static Action onCoinCollected;
    public static Action onPlayerSpaceshipDied;
    public static Action<int> onPlayerHealthChanged;

    private float xClampLimit = 18f;
    private float yClampLimit = 12f;

    private float horizontal;
    private float vertical;

    float lastFiredTime;

    int healthAmount = 100;

    private bool isArmorActive = false;

    private Rigidbody rigidbodyComp;
    private Transform shipMesh;
    //-----------------------------------------------------------------------------------------------------
    private void Start()
    {
        rigidbodyComp = GetComponent<Rigidbody>();
        shipMesh = transform.GetChild(0);

        healthAmount = 100;
        onPlayerHealthChanged?.Invoke(100);
    }


    private void FixedUpdate()
    {
        ControlSpaceship();
        ClampSpaceshipPositon();
    }

    private void Update()
    {
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

            if (tempCollectable.itsGameObject.GetComponent<Armor>() != null)
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
                other.GetComponent<IPoolableObject>().itsGameObject.GetComponent<Asteroid>().DestroyAndInstantiateTwoPieces(0, false);
            }
        }
    }

    //-----------------------------------------------------------------------------------------------------
    private void ControlSpaceship()
    {
        Quaternion targetRot = Quaternion.Euler(vertical * rotSpeed, -horizontal * rotSpeed, transform.rotation.eulerAngles.z - horizontal * rotSpeed);

        rigidbodyComp.MoveRotation(Quaternion.Lerp(transform.rotation, targetRot, rotSpeed));
        rigidbodyComp.velocity = transform.up * moveSpeed * vertical;
    }

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
            if (Time.time > (lastFiredTime + fireGap))
            {
                for (int i = 0; i < 3; i++)
                {
                    FireAsPerWeaponType();
                }
                lastFiredTime = Time.time;
            }
        }
    }

    private void FireAsPerWeaponType()
    {
        Vector3 dir = spaceShipBulletFirePos.position - transform.position;

        switch (weaponType)
        {
            case WeaponType.BASIC_BULLET:
                for (int i = 0; i < 3; i++)
                {
                    Fire(i, dir, bulletPrefab.name);
                }
                break;

            case WeaponType.TRI_ANGLED_BULLET:
                for (int i = 0; i < 3; i++)
                {
                    Fire(i, dir, bulletPrefab.name);
                    Vector3 angledDir = Quaternion.AngleAxis(30, transform.forward) * dir;

                    Fire(i, angledDir, bulletPrefab.name);
                    angledDir = Quaternion.AngleAxis(30, -transform.forward) * dir;
                    Fire(i, angledDir, bulletPrefab.name);
                }
                break;

            case WeaponType.CRESCENT_MOON_BULLET:
                Fire(0, dir, crescentBulletPrefab.name);
                break;

            case WeaponType.SHOCK_WAVE_BULLET:
                Fire(0, dir, shockWaveBulletPrefab.name);
                break;

            default:
                break;
        }
    }

    private void Fire(int i, Vector3 _direction, string _bulletName)
    {
        Vector3 firePos = spaceShipBulletFirePos.position;
        firePos += (_direction) * i * 0.6f;
        firePos.z = 0;

        ObjectPooler_Sonu.instance.GetPooledObject(_bulletName).Spawn(firePos, _direction);
    }

    void TakeDamage()
    {
        if (healthAmount > 0)
        {
            healthAmount -= damageAmount;
        }
        else
        {
            healthAmount = 0;
            DiePlayerSpaceship();
        }

        onPlayerHealthChanged?.Invoke(healthAmount);
    }

    void DiePlayerSpaceship()
    {
        onPlayerSpaceshipDied?.Invoke();
        gameObject.SetActive(false);
    }
    //---------------------------------------------------------------------------------------------
    void ActivateSpaceshipArmor()
    {
        spaceshipArmor.SetActive(true);
        isArmorActive = true;
        Invoke(nameof(DeactivateArmorAfterTime), armorDuration);
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
        Invoke(nameof(DeactivateCrescentMoonWeapon), crescentWeaponDuration);
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