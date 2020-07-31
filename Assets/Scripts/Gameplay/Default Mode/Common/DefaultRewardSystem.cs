using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DefaultRewardSystem : MonoBehaviour
{
    public GameObject
        music_obj,
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
        start_lvl,
        enemies_killed_stats, // Кол-во убитых вражеских юнитов за раунд
        earned_gold_stats; // Кол-во заработанного золота за раунд

    private void Awake()
    {
        game_controller = GetComponent<DefaultGameController>();
        victory_sound = transform.GetChild(0).gameObject;
        defeat_sound = transform.GetChild(1).gameObject;
        language = GlobalData.GetString("Language");
        start_lvl = GlobalData.GetInt("CurrentLevel");
    }

    // Считаем награду за матч
    public void CalculateReward(bool won, float round_time)
    {
        enemies_killed_stats = GlobalStats.EnemiesKilled; // Берём статистику убитых противников за раунд
        music_obj.GetComponent<AudioSource>().Stop();

        // Если игрок выиграл
        if (won)
        {
            CheckLevel(round_time); // Открываем новый лвл (если можно) и проверяем гемы
            CalcGold(round_time); // Считаем награду за раунд (золото) !!! НЕ СЧИТАТЬ ПРИ ИГРЕ НА АРЕНЕ !!!!!

            victory_sound.SetActive(true); // Звук при победе
            earned_gold_stats = GlobalStats.GoldEarned; // Статистика заработанного золота за раунд

            GameObject[] enemy_units = GameObject.FindGameObjectsWithTag("Enemy"); // Ищем всех противников на карте

            // Убиваем всех вражеских юнитов, не давая опыта и золота
            foreach (GameObject enemy in enemy_units)
            {
                // Если юнит жив
                if (enemy != null && !enemy.GetComponent<UnitManager>().IsDead)
                    enemy.GetComponent<UnitManager>().Destroy();
            }

            GlobalStats.SetStats("StatsVictories", 1); // Прибавляем победу к статистике Побед
            GlobalStats.SaveStatistics(); // Сохраняем статистику за раунд и очищаем её после этого

            // Вызываем последнее меню
            StartCoroutine(Timer("EndGameWon", 1.5f));
        }

        // Если проиграли
        else
        {
            defeat_sound.SetActive(true); // Звук при проигрыше
            earned_gold_stats = GlobalStats.GoldEarned; // Статистика заработанного золота за раунд

            GlobalStats.SetStats("StatsDefeats", 1); // Прибавляем поражение к статистике Поражений
            GlobalStats.SaveStatistics(); // Сохраняем статистику за раунд и очищаем её после этого

            // Вызываем последнее меню
            StartCoroutine(Timer("EndGameLost", 1.5f));
        }
    }

    // Проверяем можем ли открыть новый уровень и проверяем гемы
    private void CheckLevel(float round_time)
    { 
        int
            current_lvl = GlobalData.GetInt("CurrentLevel"),
            max_lvl = GlobalData.GetInt("MaxLevel"),
            gem_chance = CheckGemsChance(round_time); // Записываем шанс выпадения гема

        // Открываем новый уровень
        if (current_lvl == max_lvl)
        {
            max_lvl++;
            GlobalData.SetInt("MaxLevel", max_lvl);
            GlobalData.SetInt("CurrentLevel", max_lvl);
        }

        // Даём гем только если пройденный уровень равен последним трём открытым
        if (current_lvl >= max_lvl - 3)
        {
            // Если гем выпал
            if (Random.Range(0, 99) < gem_chance)
            {
                GlobalStats.AddGems(1);
                gems_obj.SetActive(true); // Активируем текстуру с текстом полученных гемов

                txt_gems.text = GlobalTranslateSystem.TranslateStatsText("Gems received") + ":  1";
            }
        }
    }

    // Считаем выигрышное золото
    private void CalcGold(float round_time)
    {
        float reward_time_bonus;

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
    }

    // Проверяем шансы выпадения гемов
    private int CheckGemsChance(float round_time)
    {
        // Шанс выпадения гема
        if (round_time < 100)
            return 5; // 10%
        else if (round_time > 100 && round_time < 180)
            return 10; // 20%
        else
            return 15; // 30%
    }

    // Таймер для различных задач
    private IEnumerator Timer(string code, float time)
    {
        yield return new WaitForSeconds(time);

        end_game_menu.SetActive(true);

        switch (code)
        {
            case "EndGameWon":  
                next_button.SetActive(true); // Переход в Инвентарь

                // Если язык русский
                if (language == "ru")
                {
                    txt_title.text = "Уровень " + start_lvl + " пройден!"; // Текст "Уровень № пройден!"
                    txt_enemies_killed.text = "Противников убито:  " + enemies_killed_stats; // Сколько убили за раунд
                    txt_health.text = "Здоровья осталось:  " + game_controller.AllyHealth + "%"; // Сколько здоровья осталось
                    txt_gold.text = "Золота получено:  " + earned_gold_stats; // Сколько золота заработали
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
                    // Если обычный режим игры
                    if (GlobalData.GetInt("GameMode") != 4)
                    {
                        txt_title.text = "Level " + start_lvl + " complete!"; // Текст "Уровень № пройден!"
                        txt_enemies_killed.text = "Enemies defeated:  " + enemies_killed_stats; // Сколько убили за раунд
                        txt_health.text = "Health remainig:  " + game_controller.AllyHealth + "%"; // Сколько здоровья осталось
                        txt_gold.text = "Gold earned:  " + earned_gold_stats; // Сколько золота заработали
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
                }
                break;

            case "EndGameLost":
                repeat_button.SetActive(true); // Загрузка этой же локации

                // Если Обычный режим
                if (GlobalData.GetInt("GameMode") != 1)
                {
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
                }

                // Если режим игры Арена
                else
                {
                    if (language == "ru")
                    {
                        txt_title.text = "Поражение.";
                        txt_enemies_killed.text = "Противников убито:  " + enemies_killed_stats; // Сколько убили за раунд
                        txt_gold.text = "Золота получено:  " + earned_gold_stats; // Сколько золота заработали
                        txt_round_time.text = "Время:  " + Mathf.Round(game_controller.CurrentRoundTime) + "с";
                        txt_health.text = "Волны:  " + ArenaManager.instance.wave_num;

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
                    else
                    {
                        txt_title.text = "Game over.";
                        txt_enemies_killed.text = "Enemies defeated:  " + enemies_killed_stats; // Сколько убили за раунд
                        txt_gold.text = "Gold earned:  " + earned_gold_stats; // Сколько золота заработали
                        txt_round_time.text = "Time:  " + Mathf.Round(game_controller.CurrentRoundTime) + "s";
                        txt_health.text = "Waves:  " + ArenaManager.instance.wave_num;

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
                }
                break;
        }
    }
}
