using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolableObject
{
    GameObject itsGameObject { get;}

    void Spawn(Vector3 _spawnPos);
    void Reset();
}

public interface ICollectable
{
    GameObject itsGameObject { get;}
    void Collect();
}

public interface IResetable
{
    void Reset();
}
