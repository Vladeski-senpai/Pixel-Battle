using UnityEngine;

/* =============================================
 *     ЗДЕСЬ ХРАНИМ ВСЮ СТАТУ И ПЕРКИ ЮНИТОВ
 * =============================================
*/
public class EnemyStatsRecorder : MonoBehaviour
{
    private DefaultEnemySpawnManager spawn_manager;

    private float[]
        RegularUnitStats, // Базовые характеристики Обычных юнитов
        StrongUnitStats, // Базовые характеристики Сильных юнитов
        BonusUnitStats; // Базовые характеристики Бонусных юнитов

    private void Awake()
    {
        spawn_manager = GetComponent<DefaultEnemySpawnManager>(); // Кэшируем скрипт
        RegularUnitStats = new float[4];
        StrongUnitStats = new float[4];
        BonusUnitStats = new float[4];
    }

    // Устанавливаем статы Обычных юнитов
    public void SetRegularUnitStats(string unit_name)
    {
        float hp = 0, dmg = 0, move_s = 0, attack_s = 0;

        // Записываем базовые характеристики
        switch (unit_name)
        {
            case "Goblin Melee": hp = 38; dmg = 13; move_s = 1.45f; attack_s = 0.8f; break;
            case "Goblin Range": hp = 26; dmg = 8; move_s = 1.55f; attack_s = 0.9f; break;
            case "Skeleton Melee": hp = 33; dmg = 12; move_s = 1.65f; attack_s = 0.75f; break;
            case "Skeleton Range": hp = 23; dmg = 8; move_s = 1.75f; attack_s = 0.8f; break;
            case "Spider Melee": hp = 33; dmg = 12; move_s = 1.6f; attack_s = 0.7f; break;
            case "Spider Range": hp = 23; dmg = 8; move_s = 1.5f; attack_s = 1.1f; break;
            case "Snowguy Melee": hp = 37; dmg = 13; move_s = 1.5f; attack_s = 1; break;
            case "Snowguy Range": hp = 26; dmg = 10; move_s = 1.55f; attack_s = 1.3f; break;
        }

        RecordRegularUnitStats(hp, dmg, move_s, attack_s); // Записываем в массив
        spawn_manager.RegularUnitStats = RegularUnitStats; // Записываем статы в Spawn Manager
    }

    // Устанавливаем статы Сильных юнитов
    public void SetStrongUnitStats(string unit_name)
    {
        float hp = 0, dmg = 0, move_s = 0, attack_s = 0;

        // Записываем базовые характеристики
        switch (unit_name)
        {
            case "Ogre": hp = 90; dmg = 18; move_s = 1.1f; attack_s = 1.15f; break;
            case "Minotaur": hp = 95; dmg = 21; move_s = 1.05f; attack_s = 1.15f; break;
            case "Giant Spider": hp = 85; dmg = 17; move_s = 1.25f; attack_s = 1.1f; break;
            case "Viper": hp = 80; dmg = 19; move_s = 1.35f; attack_s = 1; break;
            case "Yeti": hp = 100; dmg = 19; move_s = 1.1f; attack_s = 1.15f; break;
        }

        if (hp == 0) Debug.LogError("STATS ERROR ZERO HEALTH.");
        RecordStrongUnitStats(hp, dmg, move_s, attack_s); // Записываем в массив
        spawn_manager.StrongUnitStats = StrongUnitStats; // Записываем статы в Spawn Manager
    }

    // Устанавливаем статы Бонусных юнитов
    public void SetBonusUnitStats(string unit_name)
    {
        float hp = 0, dmg = 0, move_s = 0, attack_s = 0;

        // Записываем базовые характеристики
        switch (unit_name)
        {
            case "Mutant": hp = 26; dmg = 7; move_s = 1.55f; attack_s = 0.5f; break;
            case "Troll": hp = 36; dmg = 12; move_s = 1.85f; attack_s = 0.45f; break;
            case "Bat": hp = 20; dmg = 10; move_s = 1.7f; attack_s = 0.7f; break;
            case "FrozenZombie": case "Zombie": hp = 50; dmg = 11; move_s = 1.3f; attack_s = 1; break;
            case "Slime": hp = 35; dmg = 10; move_s = 1.35f; attack_s = 1; break;
            case "Treant": hp = 29; dmg = 10; move_s = 1.4f; attack_s = 1; break;

            // Незаконченные юниты
            case "SnakeMelee": hp = 38; dmg = 13; move_s = 1.45f; attack_s = 0.8f; break;
            case "SnakeRange": hp = 38; dmg = 13; move_s = 1.45f; attack_s = 0.8f; break;
            case "Warlock": hp = 38; dmg = 13; move_s = 1.45f; attack_s = 0.8f; break;
            case "Ballista": hp = 38; dmg = 13; move_s = 1.45f; attack_s = 0.8f; break;
            case "Pangolier": hp = 38; dmg = 13; move_s = 1.45f; attack_s = 0.8f; break;
        }

        RecordBonusUnitStats(hp, dmg, move_s, attack_s); // Записываем в массив
        spawn_manager.BonusUnitStats = BonusUnitStats; // Записываем статы в Spawn Manager
    }

    //  ==========================================================================================================

    /// <summary>
    /// Записываем базовые характеристики Обычного юнита
    /// </summary>
    /// <param name="health">Здоровье юнита</param>
    /// <param name="damage">Урон юнита</param>
    /// <param name="move_speed">Скорость передвижения юнита</param>
    /// <param name="attack_speed">Скорость атаки юнита</param>
    private void RecordRegularUnitStats(float health, float damage, float move_speed, float attack_speed)
    {
        RegularUnitStats[0] = health;
        RegularUnitStats[1] = damage;
        RegularUnitStats[2] = move_speed;
        RegularUnitStats[3] = attack_speed;
    }

    /// <summary>
    /// Записываем базовые характеристики Сильного юнита
    /// </summary>
    /// <param name="health">Здоровье юнита</param>
    /// <param name="damage">Урон юнита</param>
    /// <param name="move_speed">Скорость передвижения юнита</param>
    /// <param name="attack_speed">Скорость атаки юнита</param>
    private void RecordStrongUnitStats(float health, float damage, float move_speed, float attack_speed)
    {
        StrongUnitStats[0] = health;
        StrongUnitStats[1] = damage;
        StrongUnitStats[2] = move_speed;
        StrongUnitStats[3] = attack_speed;
    }

    /// <summary>
    /// Записываем базовые характеристики Бонусного юнита
    /// </summary>
    /// <param name="health">Здоровье юнита</param>
    /// <param name="damage">Урон юнита</param>
    /// <param name="move_speed">Скорость передвижения юнита</param>
    /// <param name="attack_speed">Скорость атаки юнита</param>
    private void RecordBonusUnitStats(float health, float damage, float move_speed, float attack_speed)
    {
        BonusUnitStats[0] = health;
        BonusUnitStats[1] = damage;
        BonusUnitStats[2] = move_speed;
        BonusUnitStats[3] = attack_speed;
    }
}
