using UnityEngine;

// КЛАСС ОТВЕЧАЮЩИЙ ЗА ВЫБОР ЮНИТОВ
public class EnemyUnitsSelector : MonoBehaviour
{
    [Header("Regular Units Store")]
    public GameObject[] RegularUnits; // Обычный тип юнитов

    [Header("Strong Units Store")]
    public GameObject[] StrongUnits; // Сильный тип юнитов

    [Header("Arena Bonus Units")]
    public GameObject[] A_BonusUnits;

    // Бонусные типы юнитов
    [Header("Summer Field Bonus Units")]
    [Space]
    public GameObject[] SF_BonusUnits;

    [Header("Winter Field Bonus Units")]
    public GameObject[] WF_BonusUnits;

    [Header("Desert Bonus Units")]
    public GameObject[] D_BonusUnits;

    [Header("Dark Forest Bonus Units")]
    public GameObject[] DF_BonusUnits;

    public int MapType { get; set; } // Типы карт: 0 - Summer Field, 1 - Winter Field, 2 - Desert, 3 - Dark Forest

    // Выбираем Обычного юнита
    public GameObject GetRegularUnit(string unit_name)
    {
        // Ищем префаб юнита по его имени
        for (int i = 0; i < RegularUnits.Length; i++)
        {
            if (RegularUnits[i].name == unit_name)
                return RegularUnits[i];
        }
        return null;
    }

    // Выбираем Обычного юнита
    public GameObject GetStrongUnit(string unit_name)
    {
        // Ищем префаб юнита по его имени
        for (int i = 0; i < StrongUnits.Length; i++)
        {
            if (StrongUnits[i].name == unit_name)
                return StrongUnits[i];
        }
        return null;
    }

    // Выбираем Обычного юнита
    public GameObject GetBonusUnit()
    {
        switch (MapType)
        {
            case 0: return SF_BonusUnits[Random.Range(0, SF_BonusUnits.Length)]; // Map: Summer Field      
            case 1: return WF_BonusUnits[Random.Range(0, WF_BonusUnits.Length)]; // Map: Winter Field
            case 2: return D_BonusUnits[Random.Range(0, D_BonusUnits.Length)]; // Map: Desert           
            case 3: return DF_BonusUnits[Random.Range(0, DF_BonusUnits.Length)]; // Map: Dark Forest
            case 4: return A_BonusUnits[Random.Range(0, A_BonusUnits.Length)]; // Map: Arena

        }

        Debug.LogError("WTF");
        return null;
    }
}

/*
for (int i = 0; i < SF_BonusUnits.Length; i++)
                {
                    if (SF_BonusUnits[i].name == unit_name)
                        return SF_BonusUnits[i];
                }*/
