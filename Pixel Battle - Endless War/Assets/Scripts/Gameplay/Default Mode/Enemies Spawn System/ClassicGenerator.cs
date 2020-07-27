using System.Collections;
using UnityEngine;

public class ClassicGenerator : MonoBehaviour
{
    public static ClassicGenerator instance;

    [Header("Фон карты")]
    public SpriteRenderer background;
    public GameObject
        clouds_obj,
        snow_obj;

    public GameObject[] backgrounds; // Фоны карты: 0 - Summer Field, 1 - Winter Field, 2 - Desert, 3 - Dark Forest

    [Header("Точки спавна")]
    [Space]
    public Transform top_lane;
    public Transform
        middle_lane,
        bottom_lane;

    public MusicManager music_manager;

    [HideInInspector]
    public int map_type; // Тип карты: 0 - Summer Field, 1 - Winter Field, 2 - Desert, 3 - Dark Forest, 4 - Arena

    private DefaultEnemySpawnManager units_spawner; // Для спавна вражеских юнитов
    private DefaultGameController game_controller;

    private string army_type; // Тип армии (Goblins, Skeletons, Spider, Snowpeople, Snakes)

    private float
        spawn_time, // Время между появлением юнитов
        current_round_time,
        min_delay = 3.6f, // Минимальная задержка перед появлением юнитов
        max_delay = 4.5f; // // Максимальная задержка перед появлением юнитов

    private int
        strong_unit_chance = 5, // Шанс появления Сильного юнита
        bonus_unit_chance = 10; // Шанс появления Бонусного юнита

    private int
        first_lane_chance = 34, // Шансы cоздания юнита на первой линии
        second_lane_chance = 50, // Шансы cоздания юнита на второй линии
        third_lane_chance = 50; // Шансы создания юнита на третей линии (только для вычислений анти-спама)

    private bool delayCD, antiSpamCD;

    private void Awake()
    {
        instance = this;
        units_spawner = transform.GetChild(1).GetComponent<DefaultEnemySpawnManager>(); // Кэшируем скрипт
        game_controller = GetComponent<DefaultGameController>(); // Кэшируем скрипт
    }

    private void Start()
    {
        MapSettings();

        // Если не Арена
        if (map_type != 4)
        {
            StartCoroutine(AntiSpamCheck());
            StartCoroutine(ClassicSpawner("Regular", 3)); // Обычные юниты
            StartCoroutine(ClassicSpawner("Strong", 3)); // Сильные юниты
            StartCoroutine(ClassicSpawner("Bonus", 3)); // Бонусные юниты
        }

        // Если Арена
        else
        {
            spawn_time = SpawnTime();
            StartCoroutine(ArenaTimer("Regular", spawn_time)); // Запускаем генератор Обычных юнитов
            StartCoroutine(ArenaTimer("Strong", spawn_time)); // Запускаем генератор Сильных юнитов
            StartCoroutine(ArenaTimer("Bonus", spawn_time)); // Запускаем генератор Бонусных юнитов
        }

        StartCoroutine(ChangeArmyType(30));
    }

    private void Update()
    {
        current_round_time += Time.deltaTime; // Здесь считаем время раунда

        if (map_type == 4) CalcSpawnDelay(); // Только для арены
    }

    /// <summary>
    /// Добавляем шанс спавна врагов на линию, где был создан союзный юнит
    /// </summary>
    /// <param name="lane_num">Номер линии, где был создан союзный юнит</param>
    public void AddSpawnChance(byte lane_num)
    {
        if (map_type != 4)
        {
            bool check = false;

            switch (lane_num)
            {
                case 1:
                    // Не прибавляем если значение меньше числа
                    if (first_lane_chance < 46)
                    {
                        first_lane_chance += 2; // Прибавляем 2% к шансу появления на первой линии
                        second_lane_chance -= 1;
                        third_lane_chance -= 1;
                    }
                    else check = true;
                    break;

                case 2:
                    // Не прибавляем если значение меньше числа
                    if (second_lane_chance < 62)
                    {
                        first_lane_chance -= 1;
                        second_lane_chance += 2; // Прибавляем 2% к шансу появления на второй линии
                        third_lane_chance -= 1;
                    }
                    else check = true;
                    break;

                case 3:
                    // Не прибавляем если значение меньше числа
                    if (third_lane_chance < 62)
                    {
                        first_lane_chance -= 1; // Отнимаем 2% шанса появления на первой линии
                        second_lane_chance -= 1; // Отнимаем 2% шанса появления на второй линии
                        third_lane_chance += 2;
                    }
                    else check = true;
                    break;
            }

            // Через 12 секунд возвращаем прибавленный и отнятый проценты
            if (!check) StartCoroutine(LaneChanceTimer(lane_num));
        }
    }

    // Считаем сложность
    private void CalcDifficulty()
    {
        if (strong_unit_chance < 35)
            strong_unit_chance++;

        if (bonus_unit_chance < 65)
            bonus_unit_chance += 2;
    }

    // Вычисляем время раунда
    private void CalcSpawnDelay()
    {
        if (!delayCD && (int)current_round_time % 20 == 0)
        {
            min_delay = (float)System.Math.Round(min_delay, 2); // Округляем до 1 знака после запятой
            max_delay = (float)System.Math.Round(max_delay, 2); // Тоже что и выше
            delayCD = true; // Кулдаун вычисления
            CalcDifficulty(); // Меняем шансы появления Сильных и Бонусных юнитов

            if (max_delay > 2.3 && max_delay > min_delay + 0.5f)
                max_delay -= 0.2f;
            else if (min_delay > 1.8f)
                min_delay -= 0.2f;

            // Запускаем отключение кулдауна
            if (min_delay != 2)
                StartCoroutine(DelayCD(18));
        }
    }

    // Различные настройки карты
    private void MapSettings()
    {
        backgrounds[4].SetActive(false); // Отключаем арену

        // Выбираем тип карты: 0 - Summer Field, 1 - Winter Field, 2 - Desert, 3 - Dark Forest, 4 - Arena
        map_type = Random.Range(0, 4);

        if (GlobalData.GetInt("GameMode") == 1) map_type = 4;

        backgrounds[map_type].SetActive(true); // Включаем нужный фон
        units_spawner.GetComponent<EnemyUnitsSelector>().MapType = map_type; // Устанавливаем тип карты для выбора юнитов

        // Запускаем музыку
        if (GlobalData.GetInt("Music") != 0)
        {
            if (map_type == 4) music_manager.PlayMusic("Arena");
            else music_manager.PlayMusic("Default");
        }

        // Включаем снег на зимней карте
        if (map_type == 1) snow_obj.SetActive(true);

        // Включаем локальные для карты объекты
        switch (map_type)
        {
            case 0:
            case 1:
            case 2:
                clouds_obj.SetActive(true); // Включаем облака

                // Звук ветра
                if (AudioManager.instance.IsOn())
                    clouds_obj.GetComponent<AudioSource>().Play();
                break;
        }
    }

    // Создаём Сильного юнита для анти-спама
    private void CreateAntiSpamUnit(int lane_num)
    {
        StartCoroutine(AntiSpamCD());

        switch (lane_num)
        {
            case 1: units_spawner.SpawnStrongUnit(GetRandomStrongUnit(), top_lane.position); break;
            case 2: units_spawner.SpawnStrongUnit(GetRandomStrongUnit(), middle_lane.position); break;
            case 3: units_spawner.SpawnStrongUnit(GetRandomStrongUnit(), bottom_lane.position); break;
        }
    }

    // Отправляем запрос на создание юнитов
    private void SpawnUnit(string unit_type)
    {
        switch (unit_type)
        {
            case "Regular": units_spawner.SpawnRegularUnit(GetRandomRegularUnit(), ChooseLane()); break;
            case "Strong": units_spawner.SpawnStrongUnit(GetRandomStrongUnit(), ChooseLane()); break;
            case "Bonus": units_spawner.SpawnBonusUnit(ChooseLane()); break;
        }
    }

    // Выбираем Обычного юнита случайного типа (ближник/дальник)
    private string GetRandomRegularUnit()
    {
        int rand = RandomChance();

        switch (army_type)
        {
            case "Goblins": if (rand < 25) return "Goblin Range"; else return "Goblin Melee";
            case "Skeletons": if (rand < 25) return "Skeleton Range"; else return "Skeleton Melee";
            case "Spiders": if (rand < 25) return "Spider Range"; else return "Spider Melee";
            case "Snowpeople": if (rand < 25) return "Snowguy Range"; else return "Snowguy Melee";
            case "Snakes": if (rand < 25) return "Snake Range"; else return "Snake Melee";

            default: return "";
        }
    }

    // Выбираем случайного Сильного юнита
    private string GetRandomStrongUnit()
    {
        // Если не Арена
        if (map_type != 4)
        {
            switch (Random.Range(0, 4))
            {
                case 3:
                    if (map_type != 1) return "Viper";
                    else return "Yeti"; // Map - Winter Field

                case 0: return "Ogre";
                case 1: return "Minotaur";
                case 2: return "Giant Spider";

                default: return "";
            }
        }

        // Если Арена
        else
        {
            switch (Random.Range(0, 5))
            {
                case 0: return "Ogre";
                case 1: return "Minotaur";
                case 2: return "Giant Spider";
                case 3: return "Viper";
                case 4: return "Yeti";

                default: return "";
            }
        }
    }

    // Метод генерации врагов
    private void ArenaSpawner(string unit_type)
    {
        switch (unit_type)
        {
            // Обычный тип юнитов
            case "Regular":
                spawn_time = SpawnTime(); // Случайное время появления Обычного юнита
                SpawnUnit(unit_type); // Создаём Обычного юнита
                break;

            // Сильный тип юнитов
            case "Strong":
                spawn_time = SpawnTime(); // Случайное время появления Сильного юнита

                // Создаём Сильного юнита
                if (RandomChance() < strong_unit_chance)
                    SpawnUnit(unit_type); // Создаём Сильного юнита
                break;

            // Бонусный тип юнитов
            case "Bonus":
                spawn_time = SpawnTime(); // Случайное время появления Бонусного юнита

                // Создаём Бонусного юнита
                if (RandomChance() < bonus_unit_chance)
                    SpawnUnit(unit_type); // Создаём Бонусного юнита
                break;
        }

        // Вызываем спавн юнитов
        StartCoroutine(ArenaTimer(unit_type, spawn_time));
    }

    private IEnumerator ArenaTimer(string unit_type, float time)
    {
        yield return new WaitForSeconds(time);

        ArenaSpawner(unit_type);
    }

    /// <summary>
    /// Куратина создающая вражеских юнита
    /// </summary>
    /// <param name="unit_type">Тип юнита: Regular, Strong, Bonus</param>
    /// <param name="time">Кулдаун создания</param>
    private IEnumerator ClassicSpawner(string unit_type, float time)
    {
        yield return new WaitForSeconds(time);

        /*  ======================================
         *  Late стадия игры, после 180-й секунды.
        */
        if (game_controller.CurrentRoundTime > 180)
        {
            switch (unit_type)
            {
                case "Regular":
                    spawn_time = Random.Range(2.5f, 3.4f);
                    SpawnUnit(unit_type);
                    break;

                case "Strong":
                    // Создаём Сильного юнита с шансом 1 к 5
                    if (Random.Range(0, 5) == 4)
                        SpawnUnit(unit_type);
                    spawn_time = Random.Range(2.6f, 3.4f);
                    break;

                case "Bonus":
                    // Создаём Бонусного юнита с шансом 1 к 3
                    if (Random.Range(0, 3) == 2)
                        SpawnUnit(unit_type);
                    spawn_time = Random.Range(2.6f, 3.2f);
                    break;
            }
        }
        /*  =============================================
         *  Middle стадия игры, с 80-ой до 180-й секунды.
        */
        else if (game_controller.CurrentRoundTime > 80)
        {
            switch (unit_type)
            {
                case "Regular":
                    spawn_time = Random.Range(2.9f, 3.7f);
                    SpawnUnit(unit_type);
                    break;

                case "Strong":
                    // Создаём Сильного юнита с шансом 1 к 6
                    if (Random.Range(0, 6) == 5)
                        SpawnUnit(unit_type);
                    spawn_time = Random.Range(3.2f, 3.7f);
                    break;

                case "Bonus":
                    // Создаём Бонусного юнита с шансом 1 к 4
                    if (Random.Range(0, 3) == 2)
                        SpawnUnit(unit_type);
                    spawn_time = Random.Range(2.9f, 3.5f);
                    break;
            }
        }
        /*  ====================================
         *  Early стадия игры, первые 80 секунд. 
        */
        else
        {
            switch (unit_type)
            {
                case "Regular":
                    spawn_time = Random.Range(3.5f, 4.1f);
                    SpawnUnit(unit_type);
                    break;

                case "Strong":
                    // Если время больше 40-а секунд, создаём Сильного юнита с шансом 1 к 8
                    if (game_controller.CurrentRoundTime > 20 && Random.Range(0, 8) == 7)
                        SpawnUnit(unit_type);
                    spawn_time = Random.Range(3.5f, 4.1f);
                    break;

                case "Bonus":
                    // Если время больше 20-и секунд, создаём Бонусного юнита с шансом 1 к 4
                    if (game_controller.CurrentRoundTime > 20 && Random.Range(0, 4) == 3)
                        SpawnUnit(unit_type);
                    spawn_time = Random.Range(3.5f, 4f);
                    break;
            }
        }

        StartCoroutine(ClassicSpawner(unit_type, spawn_time));
    }

    private IEnumerator DelayCD(float time)
    {
        yield return new WaitForSeconds(time);

        delayCD = false;
    }

    // Меняем тип армии
    private IEnumerator ChangeArmyType(float time)
    {
        string prev_type = army_type;

        switch (map_type)
        {
            // Если карта "Summer Field", "Desert", "Arena"
            case 0:
            case 2:
            case 4:
                do
                {
                    switch (Random.Range(0, 4))
                    {
                        case 0: army_type = "Goblins"; break;
                        case 1: army_type = "Skeletons"; break;
                        case 2: army_type = "Spiders"; break;
                        case 3: army_type = "Snakes"; break;
                    }
                    break;
                }
                while (army_type == prev_type); // Предотвращаем выбор предыдущей армии
                break;

            // Если карта "Winter Field"
            case 1:
                do
                {
                    switch (Random.Range(0, 3))
                    {
                        case 0: army_type = "Goblins"; break;
                        case 1: army_type = "Skeletons"; break;
                        case 2: army_type = "Snowpeople"; break;
                    }
                }
                while (army_type == prev_type); // Предотвращаем выбор предыдущей армии
                break;

            // Если карта "Dark Forest"
            case 3:
                do
                {
                    switch (Random.Range(0, 3))
                    {
                        case 0: army_type = "Skeletons"; break;
                        case 1: army_type = "Spiders"; break;
                        case 2: army_type = "Snakes"; break;
                    }
                    break;
                }
                while (army_type == prev_type); // Предотвращаем выбор предыдущей армии
                break;

        }

        yield return new WaitForSeconds(time);
        StartCoroutine(ChangeArmyType(time)); // Запускаем заново
    }

    private Vector2 ChooseLane()
    {
        // ПЕРВАЯ ЛИНИЯ (ВЕРХНЯЯ)
        if (RandomChance() < first_lane_chance)
        {
            return top_lane.position;
        }
        // ВТОРАЯ ЛИНИЯ (СРЕДНЯЯ)
        else if (RandomChance() < second_lane_chance)
        {
            return middle_lane.position;
        }
        // ТРЕТЬЯ ЛИНИЯ (НИЖНЯЯ)
        else
        {
            return bottom_lane.position;
        }
    }

    // Возвращаем рандомное время появления
    private float SpawnTime()
    {
        return Random.Range(min_delay, max_delay);
    }

    // Возвращаем случайное число (шанс срабатывания перков)  RandomChance() < your_chance 
    private int RandomChance()
    {
        return Random.Range(0, 99);
    }

    // Кулдаун между созданием юнитов для анти-спама
    private IEnumerator AntiSpamCD()
    {
        antiSpamCD = true;

        yield return new WaitForSeconds(10);

        antiSpamCD = false;
    }

    // Возвращаем % шанса спавна на линия к стандартным значениям
    private IEnumerator LaneChanceTimer(byte lane_num)
    {
        yield return new WaitForSeconds(12);

        switch (lane_num)
        {
            case 1:
                first_lane_chance -= 2; // Отнимаем 2% шанса появления на первой линии
                second_lane_chance += 1;
                third_lane_chance += 1;
                break;

            case 2:
                first_lane_chance += 1;
                second_lane_chance -= 2; // Отнимаем 2% шанса появления на второй линии
                third_lane_chance += 1;
                break;

            case 3:
                first_lane_chance += 1; // Прибавляем 2% шанса появления на первой линии
                second_lane_chance += 1; // Прибавляем 2% шанса появления на второй линии
                third_lane_chance -= 2;
                break;
        }
    }

    // Проверяем на анти-спам
    private IEnumerator AntiSpamCheck()
    {
        yield return new WaitForSeconds(0.3f);

        if (!antiSpamCD)
        {
            // Создаём Сильного юнита на первой линии
            if (first_lane_chance > 42) CreateAntiSpamUnit(1);

            // Создаём Сильного юнита на второй линии
            if (second_lane_chance > 58) CreateAntiSpamUnit(2);

            // Создаём Сильного юнита на третей линии
            if (third_lane_chance > 58) CreateAntiSpamUnit(3);
        }

        Debug.Log("First lane chance = " + first_lane_chance + "%");
        Debug.Log("Second lane chance = " + second_lane_chance + "%");
        Debug.Log("Third lane chance = " + third_lane_chance + "%");

        StartCoroutine(AntiSpamCheck());
    }
}
