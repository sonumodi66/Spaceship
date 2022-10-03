using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Toughness and Generation Data", menuName = "Spaceship/Create Toughness and Generation Data", order = 1)]
public class ToughnessAndGenerationDetails : ScriptableObject
{
    [Header("Toughness Increment Rate")]
    [Range(10, 100)]
    public int incrementRate = 20;

    [Header("Toughness Increment Time (In Secs)")]
    [Range(2, 20)]
    public int incrementTimeRate = 10;

    [Header("Crescent Bullet Powerup Generation Duration")]
    [Range(5, 20)]
    public int crescentPowerupRate = 10;
    public bool isRandmizeCrescentRate;

    [Header("Crescent Bullet Powerup Generation Duration")]
    [Range(5, 20)]
    public int armorGenerationRate = 10;
    public bool isRandmizeArmorRate;
}
