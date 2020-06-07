using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefaultRewardSystem : MonoBehaviour
{
    public AudioSource music_controller;
    public GameObject
        gems_obj,
        end_game_menu,
        next_button, // Кнопка для следующего уровня (выводит в инвентарь)
        repeat_button; // Кнопка повторая раунда (при проигрыше)

    public Text
        txt_title,
        txt_enemies_killed,
        txt_health,
        txt_gold,
        txt_gems,
        txt_round_time,
        txt_compliment;

    private DefaultGameController game_controller;
    private GameObject
        victory_sound, // Префаб с победными звуками
        defeat_sound; // Префаб с проигрышными звуками

    private string language;
    private int 
        enemies_killed_stats, // Сколько было убито на начало раунда
        start_gold; // Золото на начало раунда

    private void Awake()
    {
        game_controller = GetComponent<DefaultGameController>();
        victory_sound = transform.GetChild(0).gameObject;
        defeat_sound = transform.GetChild(1).gameObject;
        language = GlobalData.GetString("Language");
        enemies_killed_stats = GlobalData.GetInt("StatsKilled");
        start_gold = GlobalData.GetInt("Gold");
    }

    public void CalculateReward(bool won, float round_time)
    {
        // Останавливаем мызуку
        //music_controller.Stop();!!!!!!!!!!!!!!!!!!

        // Если игрок выиграл
        if (won)
        {
            // Победная музыка
            victory_sound.SetActive(true);

            float reward_time_bonus;
            int gem_chance;

            // Шанс выпадения гема
            if (round_time < 100)
                gem_chance = 5; // 10%
            else if (round_time > 100 && round_time < 180)
                gem_chance = 15; // 20%
            else
                gem_chance = 20; // 30%

            // Считаем бонусную награду от времени победы
            if (round_time < 70)
                reward_time_bonus = 0.5f; // 50% reward
            else if (round_time > 70 && round_time < 80)
                reward_time_bonus = 0.35f; // 35% reward
            else if (round_time > 80 && round_time < 90)
                reward_time_bonus = 0.20f; // 20% reward
            else if (round_time > 90 && round_time < 100)
                reward_time_bonus = 0.1f; // 10% reward
            else
                reward_time_bonus = 0;

            // Финальная награда
            float reward = Random.Range(200, 500) * (1 + reward_time_bonus);

            // Записываем новое значение золота
            GlobalStats.AddGold((int)reward);

            // Прибавляем победу к статистике Побед
            GlobalStats.SetStats("StatsVictories", 1);

            // Открываем новый уровень
            int current_lvl = GlobalData.GetInt("CurrentLevel");
            int max_lvl = GlobalData.GetInt("MaxLevel");

            if (current_lvl == max_lvl)
            {
                max_lvl++;
                GlobalData.SetInt("MaxLevel", max_lvl);
                GlobalData.SetInt("CurrentLevel", max_lvl);

                if (current_lvl == 1)
                {
                    GlobalData.SetInt("Archer", 1); // Активируем (покупаем) юнита Warrior
                }
            }

            // Даём гем только если пройденный уровень равен последним двум открытым
            if (current_lvl >= max_lvl - 3)
            {
                // Если текущий уровень кратен 15, 100% даём гем
                if (current_lvl % 15 == 0)
                    gem_chance = 100;

                // Если гем выпал
                if (Random.Range(0, 99) < gem_chance)
                {
                    GlobalStats.AddGems(1);
                    gems_obj.SetActive(true); // Активируем текстуру с текстом полученных гемов

                    txt_gems.text = GlobalTranslateSystem.TranslateStatsText("Gems received") + ":  1";
                }
            }

            // Вызываем последнее меню
            StartCoroutine(Timer("EndGameWon", 1.5f, current_lvl));

            next_button.SetActive(true);

            // Убиваем всех вражеских юнитов, не давай опыта и золота
            GameObject[] enemy_units = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemy_units)
            {
                //enemy.GetComponent<UnitManager>().DeathByDestroyer();!!!!!!!!!!!!!!!!!!!!!!
            }
        }

        // Если проиграли
        else
        {
            // Проигрышная музыка
            defeat_sound.SetActive(true);

            // Прибавляем поражение к статистике Поражений
            GlobalStats.SetStats("StatsDefeats", 1);

            // Вызываем последнее меню
            StartCoroutine(Timer("EndGameLost", 1.5f, 0));
        }

        GlobalStats.SaveStatistics(); // СОХРАНЯЕМ СТАТИСТИКУ ЗА РАУНД И ТД.
    }

    // Таймер для различных задач
    private IEnumerator Timer(string code, float time, float current_lvl)
    {
        yield return new WaitForSeconds(time);

        switch (code)
        {
            case "EndGameWon":
                // Записываем финальную статистику в конце раунда
                end_game_menu.SetActive(true);

                // Если язык русский
                if (language == "ru")
                {
                    txt_title.text = "Уровень " + current_lvl + " пройден!"; // Текст "Уровень № пройден!"
                    txt_enemies_killed.text = "Противников убито:  " + (GlobalData.GetInt("StatsKilled") - enemies_killed_stats); // Сколько убили за раунд
                    txt_health.text = "Здоровья осталось:  " + game_controller.AllyHealth + "%"; // Сколько здоровья осталось
                    txt_gold.text = "Золота получено:  " + (GlobalData.GetInt("Gold") - start_gold); // Сколько золота заработали
                    txt_round_time.text = "Время:  " + Mathf.Round(game_controller.CurrentRoundTime) + "с";

                    // Случайный комплимент
                    switch (Random.Range(0, 8))
                    {
                        case 0:
                            txt_compliment.text = "Отлично!";
                            break;

                        case 1:
                            txt_compliment.text = "Молодец!";
                            break;

                        case 2:
                            txt_compliment.text = "Прекрасно!";
                            break;

                        case 3:
                            txt_compliment.text = "Хорошая битва!";
                            break;

                        case 4:
                            txt_compliment.text = "Превосходно!";
                            break;

                        case 5:
                            txt_compliment.text = "Хорошая игра!";
                            break;

                        case 6:
                            txt_compliment.text = "Замечательно!";
                            break;

                        case 7:
                            txt_compliment.text = "Славная битва!";
                            break;
                    }
                }
                // Если язык английский
                else
                {
                    txt_title.text = "Level " + current_lvl + " complete!"; // Текст "Уровень № пройден!"
                    txt_enemies_killed.text = "Enemies defeated:  " + (GlobalData.GetInt("StatsKilled") - enemies_killed_stats); // Сколько убили за раунд
                    txt_health.text = "Health remainig:  " + game_controller.AllyHealth + "%"; // Сколько здоровья осталось
                    txt_gold.text = "Gold earned:  " + (GlobalData.GetInt("Gold") - start_gold); // Сколько золота заработали
                    txt_round_time.text = "Time:  " + Mathf.Round(game_controller.CurrentRoundTime) + "s";

                    // Случайный комплимент
                    switch (Random.Range(0, 7))
                    {
                        case 0:
                            txt_compliment.text = "Well done!";
                            break;

                        case 1:
                            txt_compliment.text = "Nice!";
                            break;

                        case 2:
                            txt_compliment.text = "Awesome!";
                            break;

                        case 3:
                            txt_compliment.text = "Good job!";
                            break;

                        case 4:
                            txt_compliment.text = "Good fight!";
                            break;

                        case 5:
                            txt_compliment.text = "Well played!";
                            break;

                        case 6:
                            txt_compliment.text = "Wonderful!";
                            break;
                    }
                }
                break;

            case "EndGameLost":
                // Финальные титры если проиграл
                end_game_menu.SetActive(true);

                txt_title.transform.localPosition = new Vector2(0, txt_title.transform.localPosition.y - 140);

                // Если язык русский
                if (language == "ru")
                    txt_title.text = "Поражение.";
                // Если язык английский
                else
                    txt_title.text = "Game over.";

                txt_enemies_killed.text = "";
                txt_health.text = "";
                txt_gold.text = "";
                txt_round_time.text = "";
                txt_compliment.text = "";

                repeat_button.SetActive(true);
                break;
        }
    }
}
