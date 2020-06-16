using UnityEngine;
using UnityEngine.UI;

public class GuildUnitButton : MonoBehaviour
{
    public GameObject big_avatar; // Аватар в меню Покупки/Улучшений

    private GuildManager guild_manager;
    private GameObject
        gold_obj,
        gems_obj;

    private Text
        txt_unit_info,
        txt_gold_cost,
        txt_gem_cost;

    private int unit_lvl;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
        txt_unit_info = transform.GetChild(1).GetComponent<Text>();
        gold_obj = transform.GetChild(2).gameObject; // Объект золота
        txt_gold_cost = gold_obj.transform.GetChild(0).GetComponent<Text>(); // Текст золота
        gems_obj = transform.GetChild(3).gameObject; // Объект гемов
        txt_gem_cost = gems_obj.transform.GetChild(0).GetComponent<Text>(); // Текст гемов
    }

    private void Start()
    {
        guild_manager = GuildManager.guild_manager;
        UpdateText();
    }

    private void TaskOnClick()
    {
        guild_manager.OpenMenu(name.Substring(3), unit_lvl, big_avatar, this);
    }

    // Обновляем инфу о юните
    public void UpdateText()
    {
        unit_lvl = GlobalData.GetInt(name.Substring(3));

        if (unit_lvl > 0)
        {
            txt_unit_info.text = name.Substring(3) + ",  Lvl " + unit_lvl;
            txt_gem_cost.text = unit_lvl.ToString(); // Стоимость в гемах
            gold_obj.SetActive(false); // Отключаем объект золота
            gems_obj.SetActive(true); // Включаем объект гемов
        }
        else
        {
            if (name.Substring(3) == "Archer") GetComponent<Button>().interactable = false;
            txt_unit_info.text = name.Substring(3) + ",  Locked.";
            txt_gold_cost.text = guild_manager.GetUnitGoldCost(name.Substring(3)).ToString(); // Стоимость юнита в золоте
            gold_obj.SetActive(true); // Включаем объект золота
            gems_obj.SetActive(false); // Отключаем объект гемов
        }
    }
}
