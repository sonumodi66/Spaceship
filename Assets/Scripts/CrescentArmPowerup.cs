using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrescentArmPowerup : MonoBehaviour, IPoolableObject, ICollectable
{
    public GameObject itsGameObject => this.gameObject;

    bool canMove;
    Vector3 direction;
    float moveSpeed;

    private void Update()
    {
        if (canMove)
            transform.Translate(direction * Time.deltaTime * moveSpeed);
    }

    public void Spawn(Vector3 _spawnPos, Vector3 _movingDirection)
    {
        transform.position = _spawnPos;
        direction = _movingDirection;
        gameObject.SetActive(true);
        moveSpeed = Random.Range(0.05f, 0.15f);
        canMove = true;
    }

    public void Reset()
    {
        ObjectPooler_Sonu.instance.ResetBackPooledObject(gameObject);
        canMove = false;
        gameObject.SetActive(false);
    }

    public void Collect()
    {
        Reset();
    }
}
