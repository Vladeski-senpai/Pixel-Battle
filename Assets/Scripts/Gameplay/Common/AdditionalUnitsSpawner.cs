using UnityEngine;

public class AdditionalUnitsSpawner : MonoBehaviour
{
    public static AdditionalUnitsSpawner instance;

    public GameObject
        undead,
        turret,
        zombie,
        frozen_zombie,
        desert_zombie,
        dark_zombie,
        spiderling;

    [Space]
    public Transform units_trashcan;

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Создаём юнитов
    /// </summary>
    /// <param name="code">Код юнита: Undead, Turret</param>
    public void SpawnUnit(string code, float posX, float posY)
    {
        Instantiate(GetUnitPref(code), new Vector2(posX, posY), Quaternion.identity, units_trashcan); // Создаём остальных юнитов
    }

    /// <summary>
    /// Создаём паразитов (зомби, паучки)
    /// </summary>
    /// <param name="code">Код юнита: 1 - зомби, 2 - зимний зомби, 3 - пустынный зомби, 4 - тёмный зомби, 5 - паучок</param>
    public void SpawnParasite(int code, float posX, float posY)
    {
        // Создаём паучков
        if (code == 5)
        {
            for (int i = 0; i < Random.Range(1, 4); i++)
            {
                Instantiate(GetParasitePref(code), new Vector2(posX + Random.Range(-0.17f, 0.23f), posY), Quaternion.identity, units_trashcan);
            }
        }
        else Instantiate(GetParasitePref(code), new Vector2(posX, posY), Quaternion.identity, units_trashcan); // Создаём остальных юнитов
    }

    // Возвращаем объект паразитов
    private GameObject GetParasitePref(int code)
    {
        switch (code)
        {
            case 1: return zombie; // Обычный зомби
            case 2: return frozen_zombie; // Зимний зомби
            case 3: return desert_zombie; // Пустынный зомби
            case 4: return dark_zombie; // Тёмный зомби
            case 5: return spiderling; // Паучок

            default: return null;
        }
    }

    // Возвращает объект юнитов
    private GameObject GetUnitPref(string code)
    {
        switch (code)
        {
            case "Undead": return undead;
            case "Turret": return turret;

            default: return null;
        }
    }
}
