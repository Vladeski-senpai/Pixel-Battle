using UnityEngine;

public class DefaultAllySpawnManager : MonoBehaviour
{
    public Transform units_trashcan; // Мусорка для юнитов

    [Header("Ally Units Store")]
    public GameObject[] AllyUnits; // Обычный тип юнитов

    private GameObject unit_prefab;
    private string choosed_unit;


    // Подготавливаем и создаём юнита
    public void PrepareAllyUnit(string unit_name)
    {
        choosed_unit = unit_name; // Записываем имя юнита для проверки
    }

    // Создаём союзного юнита
    public void SpawnUnit(Vector2 spawn_position)
    {
        unit_prefab = Instantiate(GetAllyUnit(), spawn_position, Quaternion.identity, units_trashcan) as GameObject;
        unit_prefab.name = choosed_unit;
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
