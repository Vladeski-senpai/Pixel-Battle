using UnityEngine;

public class GuildTakeUnitStats : MonoBehaviour
{
    public UnitData[] units_data;

    // Берём здоровье выбранного юнита
    public float GetUnitHP(string unit_name)
    {
        for (int i = 0; i < units_data.Length; i++)
        {
            if (units_data[i].name == unit_name)
                return units_data[i].health;
        }

        return 0;
    }


    // Берём урон выбранного юнита
    public float GetUnitDMG(string unit_name)
    {
        for (int i = 0; i < units_data.Length; i++)
        {
            if (units_data[i].name == unit_name)
                return units_data[i].damage;
        }

        return 0;
    }
}
