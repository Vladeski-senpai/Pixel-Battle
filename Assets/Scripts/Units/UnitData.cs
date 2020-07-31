using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "UnitData")]
public class UnitData : ScriptableObject
{
    public string unit_class;
    public int death_particles_size; // Размер частиц смерти (0 - обычные, 1 - большие)
    public bool isMelee = true;
    public bool canBePushed = true; // Может ли быть толкнутым
    public bool shakeHits = false; // Трясётся ли камера при ударах
    public bool shakeShoot = false; // Трясётся ли камера В НАЧАЛЕ атаки
    public bool canBleed = true; // Может ли истекать кровью
    public bool canBePoisoned = true; // Может ли отравиться
    public bool canBeInfected = true; // Может ли заразиться вирусом зомби/паука
    public bool canAlwaysMove = false; // Может ли двигаться всегда (кроме состояния оглушения)
    public bool canSwapLanes = false; // Может ли менять линии

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
    public float slow_strength = 0.3f; // На сколько замедляем время при ударах
    public int slow_frames = 2; // На сколько кадров замедляем время
    public float punch_strength = 5;
    public int punch_chance = 25; // 25%

    // Цвета пикселей при смерти
    public Color dp_color1;
    public Color dp_color2;
    public Color dp_color3;

    public List<Perks> Perks = new List<Perks>();

    public float[] GetColors()
    {
        float[] colors = new float[]
        {
            dp_color1.r, dp_color1.g, dp_color1.b,
            dp_color2.r, dp_color2.g, dp_color2.b,
            dp_color3.r, dp_color3.g, dp_color3.b
        };

        return colors;
    }

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
