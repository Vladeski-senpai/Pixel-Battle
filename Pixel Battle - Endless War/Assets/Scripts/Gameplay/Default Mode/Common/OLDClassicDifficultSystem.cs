using UnityEngine;

public static class OLDClassicDifficultSystem
{
    public static PlayerLevelManager player_level_manager;

    #region Enemies
    private static float
        enemy_exponent = 1.21f, // Экспонента роста статов 1.215
        enemy_divider = 0.15f; // Делитель результата вычислений с экспонентой
    #endregion

    #region Player
    private const float player_exponent = 1.1f; // Экспонента роста уровня игрока
    private const int player_basic_xp = 100; // Базовое кол-во опыта игрока (на 1 лвл)
    public static int map_lvl; // Уровень врагов (текущий уровень карты, устанавливаем в начале раунда)
    #endregion

    #region Allies
    public const float
        player_bonus_exponent = 0.02f, // Экспонента бонуса для юнитов от уровня игрока
        ally_divider = 0.115f,
        ally_exponent = 1.1f, // Экспонента роста союзного юнита для прокачивания
        map_exponent = 1.05f;
    #endregion

    // Возвращаем новое значение в зависимости от уровня игрока, экспоненты бонуса юнитам от уровня игрока, уровня юнита, экспоненты юнита
    public static float CalculateAllyStats(float value, int unit_lvl)
    {
        if (map_lvl > 1)
        {
            return (value * Mathf.Pow(map_lvl + 6, map_exponent) * ally_divider + (CalculatePlayerXP() * player_bonus_exponent))
                * Mathf.Pow(unit_lvl, ally_exponent);
        }
        else
        {
            return (value * Mathf.Pow(map_lvl + 6, 1.1f) * ally_divider + (CalculatePlayerXP() * player_bonus_exponent))
                * Mathf.Pow(unit_lvl, ally_exponent);
        }
    }

    // Возвращаем новое значение в зависимости от уровня *вражеского юнита* для режима - Classic
    public static float CalculateEnemyStats(float value)
    {
        return value * Mathf.Pow(map_lvl + 3, enemy_exponent) * enemy_divider;
    }

    // Возвращаем значения опыта *игрока* нужного для перехода на следующий уровень
    private static float CalculatePlayerXP()
    {
        return player_basic_xp * Mathf.Pow(player_level_manager.player_lvl, player_exponent);
    }

    // FOR GUILD MENU

    // Возвращаем новое значение в зависимости от уровня игрока, экспоненты бонуса юнитам от уровня игрока, уровня юнита, экспоненты юнита
    public static float CalculateAllyStatsGuild(float value, int unit_lvl)
    {
        return (value * Mathf.Pow(7, map_exponent) * ally_divider +
            (player_basic_xp * Mathf.Pow(1, player_exponent) * player_bonus_exponent))
            * Mathf.Pow(unit_lvl, ally_exponent);
    }
}
