using UnityEngine;

public class AdditionalUnitsSpawner : MonoBehaviour
{
    public static AdditionalUnitsSpawner instance;

    public GameObject
        undead_pref,
        turret_pref,
        zombie_pref,
        frozen_zombie_pref,
        spiderling_pref;

    [Space]
    public Transform units_trashcan;

    private GameObject temp;

    private void Awake()
    {
        instance = this;
    }

    public void SpawnUnit(string code, float posX, float posY)
    {
        temp = GetUnitPref(code); // Записываем префаб юнита

        // Создаём паучков
        if (code == "spr")
        {
            for (int i = 0; i < Random.Range(1, 5); i++)
            {
                Instantiate(temp, new Vector2(posX + Random.Range(-0.15f, 0.15f), posY), Quaternion.identity, units_trashcan);
            }
        }
        else Instantiate(temp, new Vector2(posX, posY), Quaternion.identity, units_trashcan); // Создаём остальных юнитов
    }

    private GameObject GetUnitPref(string code)
    {
        switch (code)
        {
            case "Undead": return undead_pref;
            case "Turret": return turret_pref;
            case "zmb": return zombie_pref; // Зомби
            case "frzmb": return frozen_zombie_pref; // Зимний Зомби
            case "spr": return spiderling_pref; // Паучок

            default: return null;
        }
    }
}
