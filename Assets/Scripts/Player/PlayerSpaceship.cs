using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerSpaceship : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotIntensity = 10f;
    [SerializeField] private float rotSpeed = 10f;
    [SerializeField] private float xPosLimit = 3.3f;
    [SerializeField] private float armorDuration = 5f;

    [Header("Child References")] [SerializeField]
    private Transform spaceShipBulletFirePos;
    [SerializeField] private GameObject spaceshipArmor;

    [Header("Assets References")] [Space(20)] [SerializeField]
    private Bullet bulletPrefab;

    public static Action onCoinCollected;

    private Rigidbody rigidbodyComp;

    private float horizontal;
    private float vertical;

    private bool isArmorActive = true;

    private Transform shipMesh;
    //-----------------------------------------------------------------------------------------------------
    private void Start()
    {
        rigidbodyComp = GetComponent<Rigidbody>();
        shipMesh = transform.GetChild(0);
    }


    private void FixedUpdate()
    {
    }

    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        vertical = Mathf.Clamp(vertical, -0.1f, 1f);

        // ClampSpaceshipPositon();
        ControlSpaceship();
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
        }

        if (other.GetComponent<Asteroid>() != null)
        {
            if (!isArmorActive)
            {
                this.gameObject.SetActive(false);
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
        rigidbodyComp.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.Euler(vertical * rotIntensity,  -horizontal * rotIntensity * rotSpeed, transform.rotation.eulerAngles.z - horizontal), rotSpeed));
        rigidbodyComp.velocity = transform.up * moveSpeed * vertical;
        
        // float verticalSpeed = (vertical == 0) ? 0.1f : vertical;
        // transform.Translate(new Vector3(horizontal, verticalSpeed, 0f) * 10f * Time.deltaTime);
        // transform.rotation = Quaternion.Lerp(transform.rotation,
        //     Quaternion.Euler(vertical * rotIntensity, horizontal * rotIntensity * -1.5f,
        //         -horizontal * rotIntensity * 1.5f), rotSpeed);
        // rigidbodyComp.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.Euler(vertical * rotIntensity, horizontal * rotIntensity, -horizontal * rotIntensity), rotSpeed));


        // Vector3 targetPos = transform.position + new Vector3(horizontal, vertical, 0f) * moveSpeed * Time.deltaTime;
        // rigidbodyComp.MovePosition(targetPos);
        // rigidbodyComp.AddForce(new Vector3(horizontal, vertical, 0f) * moveSpeed * Time.deltaTime);
        // rigidbodyComp.velocity = new Vector3(horizontal, vertical, 0f) * moveSpeed * Time.deltaTime;
        // rigidbodyComp.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.Euler(vertical * rotIntensity, horizontal * rotIntensity, -horizontal * rotIntensity), rotSpeed));
        // rigidbodyComp.position = Vector3.Lerp(transform.position, transform.position + 
        // new Vector3(horizontal, vertical, 0f) * 10f * Time.deltaTime, 1f);
    }

    private void ClampSpaceshipPositon()
    {
        float clampedXpos = Mathf.Clamp(transform.position.x, -xPosLimit, xPosLimit);
        transform.position = new Vector3(clampedXpos, transform.position.y, 0f);
    }

    void FireBullet()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ObjectPooler_Sonu.instance.GetPooledObject(bulletPrefab.name).Spawn(spaceShipBulletFirePos.position);
        }
    }

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
}