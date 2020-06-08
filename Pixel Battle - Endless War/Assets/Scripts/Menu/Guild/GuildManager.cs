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
        menu_gems_obj;

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

    [HideInInspector]
    public int unit_lvl;
    #endregion

    #region Private Fields
    private GuildUnitInfo unit_info;
    private GuildUnitButton unit_button;
    private GameObject big_avatar; // Аватар юнита в меню покупки/апгрейда
    #endregion

    private void Awake()
    {
        guild_manager = this;
        money_info = GetComponent<MenuGoldAndGemsInfo>();
        unit_info = GetComponent<GuildUnitInfo>();
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
            txt_menu_title.text = "Upgrade:";
            txt_unit_name.text = ChoosedUnit + "\n" + "Lvl " + unit_lvl;
            txt_current_gems.text = GlobalData.GetInt("Gems") + " -";
            txt_gems_cost.text = unit_lvl.ToString(); // Стоимость апгрейда

            menu_gold_obj.SetActive(false);
            menu_gems_obj.SetActive(true);
        }
        else
        {
            txt_menu_title.text = "Purchase:";
            txt_unit_name.text = ChoosedUnit + "\n" + "Locked.";
            txt_gold_cost.text = GetUnitGoldCost().ToString(); // Стоимость покупки

            menu_gold_obj.SetActive(true);
            menu_gems_obj.SetActive(false);
        }

        txt_unit_info.text = unit_info.GetUnitHistory(); // История юнита
        txt_unit_perks.text = unit_info.GetUnitPerks(); // Перки юнита
    }

    // Открываем меню и записываем выбранного юнита
    public void OpenMenu(string unit_name, int unit_lvl, GameObject big_avatar, GuildUnitButton unit_button)
    {
        // Если новый юнит
        if (unit_name != ChoosedUnit)
        {
            ChoosedUnit = unit_name;
            this.big_avatar = big_avatar;
            this.unit_button = unit_button;
        }

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
            case "Thief":
                return 2000;

            case "Knight":
                return 2000;

            case "Ninja":
                return 5000;

            case "Paladin":
                return 4500;

            case "Necromancer":
                return 7500;

            case "Elf Maiden":
                return 7500;

            case "Gunslinger":
                return 8000;

            case "Dark Knight":
                return 9500;

            case "Steel Bat":
                return 8000;

            case "Tinker":
                return 9500;

            default:
                return 0;
        }
    }
}
