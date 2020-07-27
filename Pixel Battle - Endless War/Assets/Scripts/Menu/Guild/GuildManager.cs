using UnityEngine;
using UnityEngine.UI;

public class GuildManager : MonoBehaviour
{
    #region Public Fields
    public static GuildManager guild_manager;

    [Header("Unit Stats")]
    [Space]
    public GameObject menu; // Меню покупки/апгрейда
    public GameObject
        menu_gold_obj,
        menu_gems_obj,
        purchase_button_obj;

    public Text
        txt_gold_cost, // Стоимость покупки в золоте
        txt_current_gems, // Текущее кол-во гемов
        txt_gems_cost; // Стоимость апгрейда

    [Header("Unit Info")]
    [Space]
    public Text txt_menu_title;
    public Text
        txt_unit_name,
        txt_unit_info,
        txt_unit_perks,
        txt_unit_stats;

    [HideInInspector]
    public MenuGoldAndGemsInfo money_info; // Текст золота и гемов в меню
    
    public string ChoosedUnit { get; private set; }
    public string Language { get; private set; }
    public int PlayerLevel { get; private set; }

    [HideInInspector]
    public int unit_lvl;
    #endregion

    #region Private Fields
    private GuildUnitInfo unit_info;
    private GuildUnitButton unit_button;
    private GameObject big_avatar; // Аватар юнита в меню покупки/апгрейда
    private AudioSource audio_s;
    private GuildTakeUnitStats units_stats;

    private bool isOn;
    #endregion

    private void Awake()
    {
        guild_manager = this;
        money_info = GetComponent<MenuGoldAndGemsInfo>();
        unit_info = GetComponent<GuildUnitInfo>();
        audio_s = GetComponent<AudioSource>();
        units_stats = GetComponent<GuildTakeUnitStats>();

        Language = GlobalData.GetString("Language");
        PlayerLevel = GlobalData.GetInt("PlayerLvl");

        if (GlobalData.GetInt("Sound") != 0) isOn = true;
    }

    public void BuyOrUpgradeUnit()
    {
        // Прокачиваем
        if (unit_lvl > 0)
        {
            int gems = GlobalData.GetInt("Gems");
            GlobalData.SetInt("Gems", gems - unit_lvl);
        }

        // Покупаем
        else
        {
            int gold = GlobalData.GetInt("Gold");
            GlobalData.SetInt("Gold", gold - GetUnitGoldCost());

            // Добавляем в статистику +1 разблокированный юнит
            GlobalStats.SetStats("StatsUnitsUnlocked", 1);
        }

        unit_lvl++; // +1 уровень
        GlobalData.SetInt(ChoosedUnit, unit_lvl); // Устанавливаем новый уровень юнита

        UpdateInfo();
        unit_button.UpdateText();
        money_info.UpdateInfo();
    }

    // Обновляем информацию о юните
    private void UpdateInfo()
    {
        if (unit_lvl > 0)
        {
            txt_menu_title.text = GlobalTranslateSystem.TranslateShortText("Upgrade") + ":";
            txt_unit_name.text = ChoosedUnit + "\n" + "Lvl " + unit_lvl;
            txt_current_gems.text = GlobalData.GetInt("Gems") + " -";
            txt_gems_cost.text = unit_lvl.ToString(); // Стоимость апгрейда

            menu_gold_obj.SetActive(false);
            menu_gems_obj.SetActive(true);
        }
        else
        {
            txt_menu_title.text = GlobalTranslateSystem.TranslateShortText("Purchase") + ":";
            txt_unit_name.text = ChoosedUnit + "\n" + GlobalTranslateSystem.TranslateShortText("Locked") + ".";
            txt_gold_cost.text = GetUnitGoldCost().ToString(); // Стоимость покупки

            menu_gold_obj.SetActive(true);
            menu_gems_obj.SetActive(false);
        }

        txt_unit_info.text = unit_info.GetUnitHistory(); // История юнита
        txt_unit_perks.text = unit_info.GetUnitPerks(); // Перки юнита

        txt_unit_stats.text = "HP  " + GetStats(true, unit_lvl) + "  ->  " + GetStats(true, unit_lvl + 1) +
            "\n" + "DMG  " + GetStats(false, unit_lvl) + "  ->  " + GetStats(false, unit_lvl + 1);
    }

    // Возвраем статы юнита
    private int GetStats(bool isHP, int lvl)
    {
        if (isHP)
            return (int)ClassicDifficultSystem.CalculateAllyStatsGuild(units_stats.GetUnitHP(ChoosedUnit), lvl);
        else
            return (int)ClassicDifficultSystem.CalculateAllyStatsGuild(units_stats.GetUnitDMG(ChoosedUnit), lvl);
    }

    // Открываем меню и записываем выбранного юнита
    public void OpenMenu(string unit_name, int unit_lvl, bool isUnlocked, GameObject big_avatar, GuildUnitButton unit_button)
    {
        // Если новый юнит
        if (unit_name != ChoosedUnit)
        {
            ChoosedUnit = unit_name;
            this.big_avatar = big_avatar;
            this.unit_button = unit_button;
        }

        if (isUnlocked) purchase_button_obj.SetActive(true);
        else purchase_button_obj.SetActive(false);

        // Звук нажатия кнопки
        if (isOn) audio_s.Play();

        this.unit_lvl = unit_lvl;
        menu.SetActive(true); // Включаем меню
        this.big_avatar.SetActive(true); // Включаем аватар юнита
        UpdateInfo(); // Обновляем информацию о юните
    }

    // Закрываем меню
    public void CloseMenu()
    {
        big_avatar.SetActive(false);
        menu.SetActive(false);
    }

    // Возвращаем стоимость выбранного юнита в золоте
    public int GetUnitGoldCost()
    {
        return GetUnitGoldCost(ChoosedUnit);
    }
    // Возвращаем стоимость указанного юнита в золоте
    public int GetUnitGoldCost(string code)
    {
        switch (code)
        {
            case "Archer": return 1500;
            case "Thief": return 2500;
            case "Knight": return 3500;
            case "Ninja": return 5500;
            case "Paladin": return 9500;
            case "Necromancer": return 7500;
            case "Elf Maiden": return 8500;
            case "Gunslinger": return 9500;
            case "Dark Knight": return 11500;
            case "Steel Bat": return 9000;
            case "Tinker": return 9500;
            case "Shieldman": return 10000;
            case "Megumi": return 11000;

            default: return 0;
        }
    }

    // Возвращаем нужный уровень для покупки юнита
    public int GetUnitRequiredLvl(string code)
    {
        switch (code)
        {
            case "Warrior": return 0;
            case "Archer": return 5;
            case "Thief": return 8;
            case "Knight": return 11;
            case "Ninja": return 15;
            case "Shieldman": return 17;
            case "Paladin": return 19;
            case "Elf Maiden": return 22;
            case "Necromancer": return 24;
            case "Tinker": return 26;
            case "Steel Bat": return 28;
            case "Gunslinger": return 30;
            case "Dark Knight": return 33;

            case "Megumi": return 100;

            default: return 0;
        }
    }
}
