using UnityEngine;

// КЛАСС ОТВЕЧАЮЩИЙ ЗА ВЫБОР ЮНИТОВ
public class EnemyUnitsSelector : MonoBehaviour
{
    [Header("Regular Units Store")]
    public GameObject[] RegularUnits; // Обычный тип юнитов

    [Header("Strong Units Store")]
    public GameObject[] StrongUnits; // Сильный тип юнитов

    [Header("Bonus Units Store")]
    public GameObject[] BonusUnits; // Бонусный тип юнитов


    private int unit_id; // id выбранного юнита

    // Выбираем Обычного юнита
    public GameObject GetRegularUnit(string unit_name)
    {
        // Ищем префаб юнита по его имени
        for (int i = 0; i < RegularUnits.Length; i++)
        {
            if (RegularUnits[i].name == unit_name)
                unit_id = i;
        }
        return RegularUnits[unit_id];
    }

    // Выбираем Обычного юнита
    public GameObject GetStrongUnit(string unit_name)
    {
        // Ищем префаб юнита по его имени
        for (int i = 0; i < StrongUnits.Length; i++)
        {
            if (StrongUnits[i].name == unit_name)
                unit_id = i;
        }
        return StrongUnits[unit_id];
    }

    // Выбираем Обычного юнита
    public GameObject GetBonusUnit(string unit_name)
    {
        // Ищем префаб юнита по его имени
        for (int i = 0; i < BonusUnits.Length; i++)
        {
            if (BonusUnits[i].name == unit_name)
                unit_id = i;
        }
        return BonusUnits[unit_id];
    }
}
