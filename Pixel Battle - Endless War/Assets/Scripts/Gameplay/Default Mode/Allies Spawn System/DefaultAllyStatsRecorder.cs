using UnityEngine;

/* =============================================
 *     ЗДЕСЬ ХРАНИМ ВСЮ СТАТУ И ПЕРКИ ЮНИТОВ
 * =============================================
*/
public class DefaultAllyStatsRecorder : MonoBehaviour
{
    private DefaultAllySpawnManager spawn_manager;

    private float[] AllyUnitStats; // Базовые характеристики союзных юнитов

    private void Awake()
    {
        spawn_manager = GetComponent<DefaultAllySpawnManager>();
        AllyUnitStats = new float[4];
    }

    // Устанавливаем статы Обычных юнитов
    public void SetAllyUnitStats(string unit_name)
    {
        float hp = 0, dmg = 0, move_s = 0, attack_s = 0;

        // Записываем базовые характеристики
        switch (unit_name)
        {
            case "Warrior": hp = 40; dmg = 14; move_s = 1.58f; attack_s = 0.6f; break;
            case "Knight": hp = 60; dmg = 20; move_s = 1.45f; attack_s = 0.75f; break;
            case "Thief": hp = 30; dmg = 9; move_s = 1.8f; attack_s = 0.6f; break;
            case "Paladin": hp = 60; dmg = 18; move_s = 1.35f; attack_s = 1.1f; break;
            case "Ninja": hp = 36; dmg = 12; move_s = 1.65f; attack_s = 0.7f; break;
            case "Dark Knight": hp = 90; dmg = 26; move_s = 1.3f; attack_s = 1.3f; break;
            case "Archer": hp = 27; dmg = 9; move_s = 1.6f; attack_s = 0.75f; break;
            case "Elf Maiden": hp = 36; dmg = 10; move_s = 1.7f; attack_s = 0.65f; break;
            case "Gunslinger": hp = 30; dmg = 10; move_s = 1.55f; attack_s = 1.1f; break;
            case "Witch": hp = 30; dmg = 7; move_s = 1.3f; attack_s = 0.9f; break;
            case "Necromancer": hp = 36; dmg = 13; move_s = 1.3f; attack_s = 3.5f; break;
            case "Undead": hp = 36; dmg = 13; move_s = 1.53f; attack_s = 0.8f; break;
            case "Steel Bat": hp = 50; dmg = 11; move_s = 1.6f; attack_s = 0.9f; break;
            case "Tinker": hp = 30; dmg = 12; move_s = 1.55f; attack_s = 0.7f; break;
        }

        RecordAllyUnitStats(hp, dmg, move_s, attack_s); // Записываем в массив
        spawn_manager.AllyUnitStats = AllyUnitStats; // Записываем статы в Spawn Manager
    }

    /// <summary>
    /// Записываем базовые характеристики союзного юнита
    /// </summary>
    /// <param name="health">Здоровье юнита</param>
    /// <param name="damage">Урон юнита</param>
    /// <param name="move_speed">Скорость передвижения юнита</param>
    /// <param name="attack_speed">Скорость атаки юнита</param>
    private void RecordAllyUnitStats(float health, float damage, float move_speed, float attack_speed)
    {
        AllyUnitStats[0] = health;
        AllyUnitStats[1] = damage;
        AllyUnitStats[2] = move_speed;
        AllyUnitStats[3] = attack_speed;
    }
}
