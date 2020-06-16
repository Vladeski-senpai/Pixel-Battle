using System.Collections;
using UnityEngine;

public class ClassicGenerator : MonoBehaviour
{
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

    private DefaultEnemySpawnManager units_spawner; // Для спавна вражеских юнитов
    private DefaultGameController game_controller;

    private string
        army_type; // Тип армии (Goblins, Skeletons, Spider, Snowpeople, Snakes)

    private float spawn_time;
    private int map_type; // Тип карты: 0 - Summer Field, 1 - Winter Field, 2 - Desert

    private void Awake()
    {
        units_spawner = transform.GetChild(1).GetComponent<DefaultEnemySpawnManager>(); // Кэшируем скрипт
        game_controller = GetComponent<DefaultGameController>(); // Кэшируем скрипт
    }

    private void Start()
    {
        // Выбираем тип карты: 0 - Summer Field, 1 - Winter Field, 2 - Desert, 3 - Dark Forest
        map_type = Random.Range(0, 4);
        map_type = 3;
        //backgrounds[map_type].SetActive(true); // Включаем нужный фон

        // Включаем локальные для карты объекты
        switch (map_type)
        {
            case 0:
            case 1:
            case 2:
                clouds_obj.SetActive(true); // Включаем облака

                // Звук ветра
                if (GlobalData.GetInt("Sound") != 0)
                    clouds_obj.GetComponent<AudioSource>().Play();
                break;
        }

        // Включаем снег на зимней карте
        if (map_type == 1) snow_obj.SetActive(true);

        StartCoroutine(SpawnCD("Regular", 3)); // Обычные юниты
        StartCoroutine(SpawnCD("Strong", 3)); // Сильные юниты
        StartCoroutine(SpawnCD("Bonus", 3)); // Бонусные юниты
        StartCoroutine(ChangeArmyType(35));
    }

    // Отправляем запрос на создание юнитов
    private void SpawnUnit(string unit_type)
    {
        switch (unit_type)
        {
            case "Regular": units_spawner.SpawnRegularUnit(GetRandomRegularUnit(), ChooseLane()); break;
            case "Strong": units_spawner.SpawnStrongUnit(GetRandomStrongUnit(), ChooseLane()); break;
            case "Bonus": units_spawner.SpawnBonusUnit(GetRandomBonusUnit(), ChooseLane()); break;
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

            default: return "";
        }
    }

    // Выбираем случайного Сильного юнита
    private string GetRandomStrongUnit()
    {
        int rand = RandomChance();

        switch (Random.Range(0, 4))
        {
            case 0: return "Ogre";
            case 1: return "Minotaur";
            case 2: return "Giant Spider";
            case 3: return "Viper"; // Map - Winter Field, else return "Yeti";

            default: return "";
        }
    }

    // Выбираем случайного Бонусного юнита
    private string GetRandomBonusUnit()
    {
        switch (Random.Range(0, 6))
        {   
            // Зомби
            case 0:
                switch (map_type)
                {
                    case 1: return "Frozen Zombie"; // Winter Field
                    case 2: return "Desert Zombie"; // Desert
                    case 3: return "Dark Zombie"; // Dark Forest

                    default: return "Zombie"; // Summer Field
                }

            // Летучая мышь
            case 3:
                switch (map_type)
                {
                    case 1: return "Frozen Bat"; // Winter Field
                    case 2: return "Desert Bat"; // Desert
                    case 3: return "Dark Bat"; // Dark Forest

                    default: return "Bat"; // Summer Field
                }

            case 1: return "Troll";
            case 2: return "Mutant";
            case 4: return "Slime";
            case 5: return "Treant";

            default: return "";
        }
    }

    // Таймер для создания юнитов
    private IEnumerator SpawnCD(string unit_type, float time)
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
                    // Создаём Сильного юнита с шансом 1 к 3
                    if (Random.Range(0, 3) == 2)
                        SpawnUnit(unit_type);
                    spawn_time = Random.Range(2.6f, 3.4f);
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
                    // Создаём Сильного юнита с шансом 1 к 7
                    if (Random.Range(0, 7) == 6)
                        SpawnUnit(unit_type);
                    spawn_time = Random.Range(3.2f, 3.7f);
                    break;

                case "Bonus":
                    // Создаём Сильного юнита с шансом 1 к 4
                    if (Random.Range(0, 4) == 3)
                        SpawnUnit(unit_type);
                    spawn_time = Random.Range(2.9f, 3.7f);
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
                    if (game_controller.CurrentRoundTime > 40 && Random.Range(0, 8) == 7)
                        SpawnUnit(unit_type);
                    spawn_time = Random.Range(3.5f, 4.1f);
                    break;

                case "Bonus":
                    // Если время больше 25-и секунд, создаём Бонусного юнита с шансом 1 к 5
                    if (game_controller.CurrentRoundTime > 25 && Random.Range(0, 5) == 4)
                        SpawnUnit(unit_type);
                    spawn_time = Random.Range(3.5f, 4.1f);
                    break;
            }
        }

        StartCoroutine(SpawnCD(unit_type, spawn_time));
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
                    switch (Random.Range(0, 3))
                    {
                        case 0: army_type = "Goblins"; break;
                        case 1: army_type = "Skeletons"; break;
                        case 2: army_type = "Spiders"; break;
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
                    switch (Random.Range(0, 2))
                    {
                        case 0: army_type = "Skeletons"; break;
                        case 1: army_type = "Spiders"; break;
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
        if (RandomChance() < 34)
        {
            return top_lane.position;
        }
        else if (RandomChance() < 50)
        {
            return middle_lane.position;
        }
        else
        {
            return bottom_lane.position;
        }
    }

    // Возвращаем случайное число (шанс срабатывания перков)  RandomChance() < your_chance 
    private int RandomChance()
    {
        return Random.Range(0, 99);
    }
}
