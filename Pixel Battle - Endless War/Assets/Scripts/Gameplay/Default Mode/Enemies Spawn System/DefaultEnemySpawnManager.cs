using UnityEngine;

public class DefaultEnemySpawnManager : MonoBehaviour
{
    public Transform units_trashcan; // Мусорка для юнитов

    [HideInInspector]
    public float[]
        RegularUnitStats, // Базовые характеристики Обычных юнитов
        StrongUnitStats, // Базовые характеристики Сильных юнитов
        BonusUnitStats; // Базовые характеристики Бонусных юнитов

    #region Private Fields
    private EnemyUnitsSelector units_selector; // Для выбора префабов юнитов
    private EnemyStatsRecorder stats_recorder; // Для записи характеристик юнитов
    private GameObject // Префабы юнитов
        regular_prefab,
        strong_prefab,
        bonus_prefab;

    private UnitManager // Для записи характеристик 
        regular_manager,
        strong_manager,
        bonus_manager;

    private string
        regular_unit, // Выбранный Обычный юнит
        strong_unit, // Выбранный Сильный юнит
        bonus_unit; // Выбранный Бонусный юнит
    #endregion

    private void Awake ()
    {
        units_selector = GetComponent<EnemyUnitsSelector>(); // Кэшируем скрипт
        stats_recorder = GetComponent<EnemyStatsRecorder>(); // Кэшируем скрипт
    }

    // Подготавливаем Обычного юнита перед созданием
    public void PrepareRegularUnit(string unit_name, Vector2 spawn_position)
    {
        // Если юнит отличается от предыдущего
        if (unit_name != regular_unit)
        {
            regular_unit = unit_name; // Записываем имя юнита для проверки
            regular_prefab = units_selector.GetRegularUnit(regular_unit); // Записываем префаб юнита
            stats_recorder.SetRegularUnitStats(regular_unit); // Записываем характеристики юнита
        }

        SpawnRegularUnit(spawn_position); // Создаём юнита
    }

    // Подготавливаем Сильного юнита перед созданием
    public void PrepareStrongUnit(string unit_name, Vector2 spawn_position)
    {
        // Если юнит отличается от предыдущего
        if (unit_name != strong_unit)
        {
            strong_unit = unit_name; // Записываем имя юнита для проверки
            strong_prefab = units_selector.GetStrongUnit(strong_unit); // Записываем префаб юнита
            stats_recorder.SetStrongUnitStats(strong_unit); // Записываем характеристики юнита
        }

        SpawnStrongUnit(spawn_position); // Создаём юнита
    }

    // Подготавливаем Бонусного юнита перед созданием
    public void PrepareBonusUnit(string unit_name, Vector2 spawn_position)
    {
        // Если юнит отличается от предыдущего
        if (unit_name != bonus_unit)
        {
            bonus_unit = unit_name; // Записываем имя юнита для проверки
            bonus_prefab = units_selector.GetBonusUnit(bonus_unit); // Записываем префаб юнита
            stats_recorder.SetBonusUnitStats(bonus_unit); // Записываем характеристики юнита
        }

        SpawnBonusUnit(spawn_position); // Создаём юнита
    }

    //  ==========================================================================================================

    // Создаём Обычного юнита
    private void SpawnRegularUnit(Vector2 spawn_position)
    {
        regular_prefab = Instantiate(regular_prefab, spawn_position, Quaternion.identity, units_trashcan) as GameObject;
        regular_prefab.name = regular_unit;
        regular_manager = regular_prefab.GetComponent<UnitManager>(); // Кэшируем скрипт
        regular_manager.health = RegularUnitStats[0];
        regular_manager.damage = RegularUnitStats[1];
        regular_manager.move_speed = RegularUnitStats[2];
        regular_manager.attack_speed = RegularUnitStats[3];
    }

    // Создаём Сильного юнита
    private void SpawnStrongUnit(Vector2 spawn_position)
    {
        strong_prefab = Instantiate(strong_prefab, spawn_position, Quaternion.identity, units_trashcan) as GameObject;
        strong_prefab.name = strong_unit;
        strong_manager = strong_prefab.GetComponent<UnitManager>(); // Кэшируем скрипт
        strong_manager.health = StrongUnitStats[0];
        strong_manager.damage = StrongUnitStats[1];
        strong_manager.move_speed = StrongUnitStats[2];
        strong_manager.attack_speed = StrongUnitStats[3];
    }

    // Создаём Бонусного юнита
    private void SpawnBonusUnit(Vector2 spawn_position)
    {
        bonus_prefab = Instantiate(bonus_prefab, spawn_position, Quaternion.identity, units_trashcan) as GameObject;
        bonus_prefab.name = bonus_unit;
        bonus_manager = bonus_prefab.GetComponent<UnitManager>(); // Кэшируем скрипт
        bonus_manager.health = BonusUnitStats[0];
        bonus_manager.damage = BonusUnitStats[1];
        bonus_manager.move_speed = BonusUnitStats[2];
        bonus_manager.attack_speed = BonusUnitStats[3];
    }
}