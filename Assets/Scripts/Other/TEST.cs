using UnityEngine;

public class TEST : MonoBehaviour
{
    [Header("Input Fields")]
    public int target_level;
    public float player_exponent = 1.1f;

    [Header("Ally Fields")]
    [Space]
    public int player_lvl;
    public int ally_health;
    public int unit_lvl;
    public float ally_exponent;
    public float ally_divider;

    [Header("Enemy Fields")]
    [Space]
    public int enemy_health;
    public int map_lvl;
    public float enemy_exponent = 1.15f;
    public float enemy_divider = 0.15f;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("From " + target_level + " to " + (target_level + 1) + " lvl, player needs " +
                (int)CalculatePlayerXP(target_level) + " xp." +
                "Total needs " + (int)CalcAllXP() + " xp and " + (int)CalcKills() + " kills.");
        }

        if (Input.GetKeyDown(KeyCode.E))
            Debug.Log("Enemy health is " + (int)CalculateEnemyStats(enemy_health) + " at " + map_lvl + " lvl");

        if (Input.GetKeyDown(KeyCode.A))
            Debug.Log("Ally health is " + (int)CalculateAllyStats(ally_health));

        if (Input.GetKeyDown(KeyCode.Equals)) target_level++;
        if (Input.GetKeyDown(KeyCode.Minus) && target_level > 1) target_level--;
    }

    // Возвращаем значения опыта *игрока* нужного для перехода на следующий уровень
    private float CalculatePlayerXP(int lvl)
    {
        return 100 * Mathf.Pow(lvl, player_exponent);
    }
    // Общая сумма опыта за все лвлы
    private float CalcAllXP()
    {
        float temp = 0;
        for (int i = 1; i < target_level + 1; i++)
        {
            temp += CalculatePlayerXP(i);
        }
        return temp;
    }
    // Общая сумма убийств нужных для получения текущего уровня
    private float CalcKills()
    {
        int xp_for_1_kill = 10;
        return CalcAllXP() / xp_for_1_kill;
    }


    // Возвращаем новое значение в зависимости от уровня *вражеского юнита* для режима - Classic
    public float CalculateEnemyStats(float value)
    {
        return value * Mathf.Pow(map_lvl + 3, enemy_exponent) * enemy_divider;
    }



    // Возвращаем новое значение в зависимости от уровня игрока, экспоненты бонуса юнитам от уровня игрока, уровня юнита, экспоненты юнита
    public float CalculateAllyStats(float value)
    {
        if (unit_lvl > 1)
            return value + (value * Mathf.Pow(unit_lvl - 1, ally_exponent) * ally_divider);
        else
            return value;
    }
}
