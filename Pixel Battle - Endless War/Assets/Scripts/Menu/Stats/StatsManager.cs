using UnityEngine;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour
{
    public Text
        arena_record,
        enemies_defeated,
        units_summoned,
        victories,
        gold_earned,
        gems_earned,
        units_unlocked,
        defeats;

    private void Start()
    {
        enemies_defeated.text = Translate("Enemies defeated", "StatsKilled"); // Убито противников
        units_summoned.text = Translate("Units summoned", "StatsUnitsSummoned"); // Призвано юнитов
        victories.text = Translate("Victories", "StatsVictories"); // Побед
        gold_earned.text = Translate("Gold earned", "StatsGoldEarned"); // Золота получено
        gems_earned.text = Translate("Gems collected", "StatsGemsCollected"); // Гемов получено
        units_unlocked.text = Translate("Units unlocked", "StatsUnitsUnlocked"); // Юнитов открыто
        defeats.text = Translate("Defeats", "StatsDefeats"); // Поражений
        arena_record.text = Translate("Arena Record", "MaxWave"); // Рекорд Арены
    }

    private string Translate(string text, string data)
    {
        return GlobalTranslateSystem.TranslateShortText(text) + ":  "
            + GlobalData.GetInt(data);
    }
}
