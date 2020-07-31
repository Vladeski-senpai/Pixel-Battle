using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelManager : MonoBehaviour
{
    public static PlayerLevelManager player_level_manager;

    public Slider slider;
    public Text txt_player_lvl;

    public int player_lvl;

    private int
        current_xp, // Текущее кол-во опыта
        new_xp; // Кол-во опыта для нового уровня

    private void Awake()
    {
        player_level_manager = this;
        player_lvl = GlobalData.GetInt("PlayerLvl");
        current_xp = GlobalData.GetInt("PlayerXP");
        new_xp = (int)CalculatePlayerXP();

        ClassicDifficultSystem.player_level_manager = this; // Кэшируем скрипт
    }
    
    private void Start()
    {
        slider.maxValue = new_xp;
        slider.value = current_xp;
        txt_player_lvl.text = "Player Level: " + player_lvl;
    }

    /// <summary>
    /// Добавляем опыт, который запишем в конце раунда
    /// </summary>
    public void AddXP(int amount)
    {
        current_xp += amount; // Прибавляем к старому кол-ву опыта новое значение  

        CheckNewLvl(); // Проверяем можем ли получить новый уровень
    }

    // Проверяем можем ли получить новый уровень
    private void CheckNewLvl()
    {
        // Если текущего опыта больше/равно нужному для нового уровня
        if (current_xp >= new_xp)
        {
            player_lvl++; // Прибавляем 1 уровень игрока
            current_xp = 0; // Обнуляем опыт
            new_xp = (int)CalculatePlayerXP(); // Обновляем кол-во опыта для нового уровня
            slider.maxValue = new_xp;
            txt_player_lvl.text = "Player Level:  " + player_lvl; // Обновляем текст с уровнем игрока

            NewLvlAnimation.new_lvl_animation.Enable(); // Запускаем анимацию нового уровня
        }

        slider.value = current_xp;
    }

    // Сохраняем статистику опыта и уровня игрока
    public void SaveStatistics()
    {
        GlobalData.SetInt("PlayerLvl", player_lvl); // Записываем уровень игрока
        GlobalData.SetInt("PlayerXP", current_xp); // Записываем опыт игрока
    }

    // Возвращаем значения опыта *игрока* нужного для перехода на следующий уровень
    public float CalculatePlayerXP()
    {
        return 100 * Mathf.Pow(player_lvl, 1.4f); // player_basix_xp = 100, player_exponent = 1.15
    }
}
