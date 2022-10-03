using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bullet Data", menuName = "Spaceship/Create Bullet Data", order = 1)]
public class BulletData : ScriptableObject
{
    public WeaponType bulletType;

    [Range(2, 20)]
    public float movingSpeed = 10f;

    [Range(1, 5)]
    public int oneShotBulletsCount = 3;

    [Header("For Tri-Angled Bullet Only")]
    [Range(10, 90)]
    public int angleValue = 30;
}