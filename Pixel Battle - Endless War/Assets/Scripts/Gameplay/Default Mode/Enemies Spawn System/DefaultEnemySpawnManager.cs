using UnityEngine;

public class DefaultEnemySpawnManager : MonoBehaviour
{
    public Transform units_trashcan; // Мусорка для юнитов

    #region Private Fields
    private EnemyUnitsSelector units_selector; // Для выбора префабов юнитов
    private GameObject // Префабы юнитов
        regular_prefab,
        strong_prefab,
        bonus_prefab;

    private string
        regular_unit, // Выбранный Обычный юнит
        strong_unit, // Выбранный Сильный юнит
        bonus_unit; // Выбранный Бонусный юнит
    #endregion

    private void Awake ()
    {
        units_selector = GetComponent<EnemyUnitsSelector>(); // Кэшируем скрипт
    }

    // Создаём Обычного юнита
    public void SpawnRegularUnit(string unit_name, Vector2 spawn_position)
    {
        // Если юнит отличается от предыдущего
        if (unit_name != regular_unit)
        {
            regular_unit = unit_name; // Записываем имя юнита для проверки
            regular_prefab = units_selector.GetRegularUnit(regular_unit); // Записываем префаб юнита
        }

        Instantiate(regular_prefab, spawn_position, Quaternion.identity, units_trashcan);
    }

    // Создаём Сильного юнита
    public void SpawnStrongUnit(string unit_name, Vector2 spawn_position)
    {
        // Если юнит отличается от предыдущего
        if (unit_name != strong_unit)
        {
            strong_unit = unit_name; // Записываем имя юнита для проверки
            strong_prefab = units_selector.GetStrongUnit(strong_unit); // Записываем префаб юнита
        }

        Instantiate(strong_prefab, spawn_position, Quaternion.identity, units_trashcan);
    }

    // Создаём Бонусного юнита
    public void SpawnBonusUnit(string unit_name, Vector2 spawn_position)
    {
        // Если юнит отличается от предыдущего
        if (unit_name != bonus_unit)
        {
            bonus_unit = unit_name; // Записываем имя юнита для проверки
            bonus_prefab = units_selector.GetBonusUnit(bonus_unit); // Записываем префаб юнита
        }

        Instantiate(bonus_prefab, spawn_position, Quaternion.identity, units_trashcan);
    }
}