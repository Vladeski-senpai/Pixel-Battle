public static class GlobalStats
{
    public static int GoldEarned { get; private set; }
    public static int GemsEarned { get; private set; }
    public static int EnemiesKilled { get; private set; }
    public static int UnitsSummoned { get; private set; }

    private static int stats;


    public static void SaveStatistics()
    {
        GoldEarned += GlobalData.GetInt("Gold");
        GemsEarned += GlobalData.GetInt("Gems");

        GlobalData.SetInt("Gold", GoldEarned); // Записываем новое кол-во золота
        GlobalData.SetInt("Gems", GemsEarned); // Записываем новое кол-во гемов

        // Добавляем в статистику Полученного золота
        stats = GlobalData.GetInt("StatsGoldEarned");
        GlobalData.SetInt("StatsGoldEarned", stats + GoldEarned);

        // Добавляем в статистику Полученных гемов
        stats = GlobalData.GetInt("StatsGemsCollected");
        GlobalData.SetInt("StatsGemsCollected", stats + GoldEarned);

        // Добавляем в статистику Убитых противников
        stats = GlobalData.GetInt("StatsKilled");
        GlobalData.SetInt("StatsKilled", stats + EnemiesKilled);

        // Добавляем в статистику Созданных юнитов
        stats = GlobalData.GetInt("StatsUnitsSummoned");
        GlobalData.SetInt("StatsUnitsSummoned", stats + UnitsSummoned);
    }

    public static void AddGold(int amount)
    {
        GoldEarned += amount;
    }

    public static void AddGems(int amount)
    {
        GemsEarned += amount;
    }

    /// <summary>
    /// Мгновенно записываем статистику.
    /// </summary>
    public static void SetStats(string name, int value)
    {
        stats = GlobalData.GetInt(name);
        GlobalData.SetInt(name, stats + value);
    }

    /// <summary>
    /// ВНИМАНИЕ! Только те данные, которые должны сохранится после окончания матча!
    /// </summary>
    public static void AddToStats(string code, int amount)
    {
        switch (code)
        {
            case "Enemies Killed": EnemiesKilled += amount; break;
            case "Units Summoned": UnitsSummoned += amount; break;
        }
    }
}

/*    Названия переменных где хранятся данные:
 *      FirstLaunch (int) - первый ли запуск игры (0 - да)
 *      Gold (int) - золото
 *      Gems (int) - гемы
 *      Music (int) - включена ли музыка (1 - да)
 *      Sound (int) - включены ли звуки (1 - да)
 *      CurrentLevel (int) - текущий уровень
 *      MaxLevel (int) - кол-во открытых уровней
 *      EnemyDivider (float) - сложность противников (0.15, чем больше число, тем сложнее)
 *      Slot1-6 (string) - юниты в 1-6 слотах
 *      *unit_name* (int) - уровень каждого юнита
 *      StatsKilled (int) - убито противников
 *      StatsGoldEarned (int) - заработано золота
 *      StatsVictories (int) - побед
 *      StatsDefeats (int) - поражений
 *      StatsGemsCollected (int) - заработано гемов
 *      StatsUnitsUnlocked (int) - открыто юнитов
 *      StatsUnitsSummoned (int) - создано юнитов
 *      Language (string) - язык игры (en, ru)
 *      PlayerLvl (int) - уровень игрока
 *      GameMode (int) - режим игры: 0 - Защита башни, 1 - Классический, 2 - Арена
 *      MaxWave (int) - максимальная волна в режиме Арена
*/
