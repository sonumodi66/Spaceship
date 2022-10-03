using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spaceship Data", menuName = "Spaceship/Create Spaceship Data", order = 1)]
public class SpaceshipData : ScriptableObject
{
    public float moveSpeed = 10f;
    public float rotationSpeed = 10f;
    
    [Header("Weapon")]
    public float fireGap = 0.2f;
    public float armorDuration = 10f;
    public float crescentWeaponDuration = 10f;
    
    [Header("Spaceship Health")]
    public int fullHealth = 100;
    public int spaceshipDamageRate = 10;
}
