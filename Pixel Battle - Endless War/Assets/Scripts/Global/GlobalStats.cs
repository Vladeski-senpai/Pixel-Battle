public static class GlobalStats
{
    public static int GoldEarned { get; private set; }
    public static int GemsEarned { get; private set; }
    public static int EnemiesKilled { get; private set; }
    public static int UnitsSummoned { get; private set; }

    public static void SaveStatistics()
    {
        GlobalData.SetInt("Gold", GlobalData.GetInt("Gold") + GoldEarned); // Записываем новое кол-во золота
        GlobalData.SetInt("Gems", GlobalData.GetInt("Gems") + GemsEarned); // Записываем новое кол-во гемов
        GlobalData.SetInt("StatsGoldEarned", GlobalData.GetInt("StatsGoldEarned") + GoldEarned); // Добавляем в статистику Полученного золота
        GlobalData.SetInt("StatsGemsCollected", GlobalData.GetInt("StatsGemsCollected") + GemsEarned); // Добавляем в статистику Полученных гемов
        GlobalData.SetInt("StatsKilled", GlobalData.GetInt("StatsKilled") + EnemiesKilled); // Добавляем в статистику Убитых противников
        GlobalData.SetInt("StatsUnitsSummoned", GlobalData.GetInt("StatsUnitsSummoned") + UnitsSummoned); // Добавляем в статистику Созданных юнитов

        PlayerLevelManager.player_level_manager.SaveStatistics(); // Сохраняем статистику опыта и уровня игрока
        ClearStatistics();
    }

    // Очищаем поля после сохранения
    private static void ClearStatistics()
    {
        GoldEarned = 0;
        GemsEarned = 0;
        EnemiesKilled = 0;
        UnitsSummoned = 0;
    }

    /// <summary>
    /// Добавляем золото, которое запишем в конце раунда
    /// </summary>
    public static void AddGold(int amount)
    {
        GoldEarned += amount;
    }

    /// <summary>
    /// Добавляем гемы, которые запишем в конце раунда
    /// </summary>
    public static void AddGems(int amount)
    {
        GemsEarned += amount;
    }

    /// <summary>
    /// ВНИМАНИЕ! Только те данные, которые должны сохранится после окончания матча!
    /// </summary>
    /// <param name="code">Enemies Killed, Units Summoned</param>
    public static void AddToStats(string code)
    {
        switch (code)
        {
            case "Enemies Killed": EnemiesKilled++; break;
            case "Units Summoned": UnitsSummoned++; break;
        }
    }

    // Добавляем золото мгновенно
    public static void AddInstantGold(int value)
    {
        GlobalData.SetInt("Gold", GlobalData.GetInt("Gold") + value); // Записываем новое золото
        SetStats("StatsGoldEarned", value); // Добавляем золото в статистику
    }

    // Добавляем гемы мгновенно
    public static void AddInstantGems(int value)
    {
        GlobalData.SetInt("Gems", GlobalData.GetInt("Gems") + value); // Записываем новое золото
        SetStats("StatsGemsCollected", value); // Добавляем золото в статистику
    }

    /// <summary>
    /// Мгновенно записываем статистику.
    /// </summary>
    public static void SetStats(string name, int value)
    {
        GlobalData.SetInt(name, GlobalData.GetInt(name) + value);
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
