using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "UnitData")]
public class UnitData : ScriptableObject
{
    public string unit_class;
    public bool isMelee = true;
    public bool canBleed = true; // Может ли истекать кровью
    public bool canPoisoned = true; // Может ли отравиться
    public bool canInfected = true; // Может ли заразиться вирусом зомби/паука

    public GameObject shell_prefab;

    public AudioClip hit_sfx;
    public float hit_sfx_volume = 1;
    public float hit_sfx_pitch = 1;

    public AudioClip spawn_sfx;
    public float spawn_sfx_volume = 1;
    public float spawn_sfx_pitch = 1;

    public float health;
    public float damage;
    public float move_speed;
    public float attack_speed;

    public List<Perks> Perks = new List<Perks>();

    public void SetClass()
    {
        unit_class = name;
    }
}

[System.Serializable]
public class Perks
{
    public string PerkName;
    public float PerkChance;
    public float PerkDuration;
    public float PerkValue;
}
