using UnityEngine;
using UnityEngine.UI;

public class PlayerLvlInfo : MonoBehaviour
{
    public Slider slider;

    public Text
        txt_player_lvl,
        txt_player_xp;

    private int
        player_lvl,
        current_xp, // Текущее кол-во опыта
        new_xp; // Кол-во опыта для нового уровня

    private void Start()
    {
        player_lvl = GlobalData.GetInt("PlayerLvl");
        current_xp = GlobalData.GetInt("PlayerXP");
        new_xp = (int)CalculatePlayerXP();
        slider.maxValue = new_xp;
        slider.value = current_xp;
        txt_player_lvl.text = "Player Level: " + player_lvl;
        txt_player_xp.text = "XP " + current_xp + " / " + new_xp;
    }

    // Возвращаем значения опыта *игрока* нужного для перехода на следующий уровень
    public float CalculatePlayerXP()
    {
        return 100 * Mathf.Pow(player_lvl, 1.1f); // player_basix_xp = 100, player_exponent = 1.1
    }
}
