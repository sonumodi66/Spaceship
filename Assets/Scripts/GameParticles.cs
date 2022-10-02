using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameParticles : MonoBehaviour, IPoolableObject
{
    [SerializeField] private float deactivateAfterTime = 1f; 
    public GameObject itsGameObject => this.gameObject;

    public void Spawn(Vector3 _spawnPos, Vector3 _movingDirection)
    {
        transform.position = _spawnPos;
        gameObject.SetActive(true);

        Invoke(nameof(Reset), deactivateAfterTime);
    }

    public void Reset()
    {
        ObjectPooler_Sonu.instance.ResetBackPooledObject(gameObject);
        gameObject.SetActive(false);
    }
}
