using UnityEngine;

public static class ClassicDifficultSystem
{
    public static PlayerLevelManager player_level_manager;

    #region Enemies
    private static float
        enemy_exponent = 1.15f, // Экспонента роста статов
        enemy_divider = 0.16f; // Делитель результата вычислений с экспонентой
    #endregion

    #region Player
    private const float player_exponent = 1.4f; // Экспонента роста уровня игрока
    private const int player_basic_xp = 100; // Базовое кол-во опыта игрока (на 1 лвл)
    public static int map_lvl; // Уровень врагов (текущий уровень карты, устанавливаем в начале раунда)
    #endregion

    #region Allies
    public const float
        player_bonus_exponent = 0.02f, // Экспонента бонуса для юнитов от уровня игрока
        ally_divider = 0.5f,
        ally_exponent = 1.1f, // Экспонента роста союзного юнита для прокачивания
        map_exponent = 1.05f;
    #endregion

    // Возвращаем новое значение в зависимости от уровня игрока, экспоненты бонуса юнитам от уровня игрока, уровня юнита, экспоненты юнита
    public static float CalculateAllyStats(float value, int unit_lvl)
    {
        if (unit_lvl > 1)
            return value + (value * Mathf.Pow(unit_lvl - 1, ally_exponent) * ally_divider);
        else
            return value;
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
        if (unit_lvl > 1)
            return value + (value * Mathf.Pow(unit_lvl - 1, ally_exponent) * ally_divider);
        else
            return value;
    }
}
