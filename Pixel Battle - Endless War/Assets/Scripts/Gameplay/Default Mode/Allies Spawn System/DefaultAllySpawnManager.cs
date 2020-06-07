using UnityEngine;

public class DefaultAllySpawnManager : MonoBehaviour
{
    public Transform units_trashcan; // Мусорка для юнитов

    [Header("Ally Units Store")]
    public GameObject[] AllyUnits; // Обычный тип юнитов

    [HideInInspector]
    public float[] AllyUnitStats; // Базовые характеристики союзных юнитов

    private DefaultAllyStatsRecorder stats_recorder;
    private UnitManager ally_manager;  // Для записи характеристик 
    private GameObject unit_prefab;
    private string choosed_unit;

    private void Awake()
    {
        stats_recorder = GetComponent<DefaultAllyStatsRecorder>();
    }

    // Подготавливаем и создаём юнита
    public void PrepareAllyUnit(string unit_name)
    {
        choosed_unit = unit_name; // Записываем имя юнита для проверки
        stats_recorder.SetAllyUnitStats(choosed_unit);
    }

    // Создаём союзного юнита
    public void SpawnUnit(Vector2 spawn_position)
    {
        unit_prefab = Instantiate(GetAllyUnit(), spawn_position, Quaternion.identity, units_trashcan) as GameObject;
        unit_prefab.name = choosed_unit;
        ally_manager = unit_prefab.GetComponent<UnitManager>(); // Кэшируем скрипт
        ally_manager.health = AllyUnitStats[0];
        ally_manager.damage = AllyUnitStats[1];
        ally_manager.move_speed = AllyUnitStats[2];
        ally_manager.attack_speed = AllyUnitStats[3];
    }

    // Берём нужный префаб юнита
    private GameObject GetAllyUnit()
    {
        // Ищем префаб юнита по его имени
        for (int i = 0; i < AllyUnits.Length; i++)
        {
            if (AllyUnits[i].name == choosed_unit)
                return AllyUnits[i];
        }

        return null;
    }
}
