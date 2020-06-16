using UnityEngine;
using UnityEngine.UI;

public class DefaultGameController : MonoBehaviour
{
    #region Public Fields
    public static DefaultGameController default_controller;
    public GameObject[] spawn_buttons;
    public Transform[] lane; // Координаты точек спавна
    public Sprite[] units_avatars; // Спрайты юнитов для "слотов"

    [Space]
    public DefaultManaButton mana_button; // Кнопка апгрейда маныи
    public Animator 
        camera_anim,
        damage_indicator,
        fade_effect; // Анимация эффекта вспышк

    public Slider
        ally_slider,
        enemy_slider;

    public Text
        txt_ally_hp,
        txt_enemy_hp,
        txt_boost_price, // Текст цены апгрейда маны
        txt_current_mana; // Текст текущей маны

    public AudioClip
        victory_sfx,
        defeat_sfx;
    #endregion

    #region Get/Set Fields
    public string ChoosedUnit { get; private set; } // Выбранный юнит
    public float CurrentMana { get; private set; } // Текущая мана
    public float CurrentRoundTime { get; private set; } // Текущее время раунда
    public float UpgradeCost { get; private set; } // Цена апгрейда маны
    public int AllyHealth { get; private set; } // Здоровье союзной базы
    #endregion

    #region Private Fields
    private DefaultAllySpawnManager spawn_manager;
    private DefaultUnitButton unit_button;
    private AudioManager audio_manager;
    private AudioSource audio_s;

    private float
        unit_cost, // Стоимость юнита в мане
        mana_regen_speed = 10.95f,
        mana_regen_bonus;

    private int enemy_health = 100; // Здоровье вражеской базы

    private bool isGameFinished; // Закончена ли игра
    #endregion

    private void Awake()
    {
        default_controller = this;
        spawn_manager = transform.GetChild(0).GetComponent<DefaultAllySpawnManager>();
        UpgradeCost = 10;
        AllyHealth = 100;
        audio_s = GetComponent<AudioSource>();
        ClassicDifficultSystem.map_lvl = GlobalData.GetInt("CurrentLevel"); // Записываем текущий уровень карты в скрипт сложности
    }

    private void Start()
    {
        audio_manager = AudioManager.instance;
    }

    private void Update()
    {
        if (!isGameFinished)
        {
            CurrentRoundTime += Time.deltaTime; // Считаем время раунда

            // Считаем ману
            CurrentMana = Mathf.Lerp(CurrentMana, CurrentMana + 1, (mana_regen_speed + (mana_regen_speed * mana_regen_bonus)) * Time.deltaTime);
            txt_current_mana.text = ((int)CurrentMana).ToString();
        }
    }

    // Наносим урон базам
    public void DoDamage(int damage, bool isAllyBase)
    {
        // Если игра не закончена
        if (!isGameFinished)
        {
            camera_anim.SetTrigger("shake"); // Запускаем дёрганье камеры

            // Если наносим урон союзной базе
            if (isAllyBase)
            {
                // Если союзная база жива, наносим урон
                if (AllyHealth > 0) AllyHealth -= damage;

                // Если союзная база убита, проигрываем
                if (AllyHealth <= 0)
                {
                    if (audio_manager.IsOn()) audio_s.PlayOneShot(defeat_sfx);
                    AllyHealth = 0;
                    isGameFinished = true;
                    GetComponent<DefaultRewardSystem>().CalculateReward(false, CurrentRoundTime); // Считаем награду
                }

                damage_indicator.SetTrigger("enemy"); // Индикатор урон по краям карты
                txt_ally_hp.text = AllyHealth.ToString(); // Меняем текст с хп
                ally_slider.value = AllyHealth; // Меняем значение слайдера с хп
            }

            // Если наносим урон вражеской базе
            else
            {
                // Если вражеская база жива, наносим урон
                if (enemy_health > 0) enemy_health -= damage;

                // Если вражеская база убита, выигрываем
                if (enemy_health <= 0)
                {
                    if (audio_manager.IsOn()) audio_s.PlayOneShot(victory_sfx);
                    enemy_health = 0;
                    isGameFinished = true;
                    GetComponent<DefaultRewardSystem>().CalculateReward(true, CurrentRoundTime); // Считаем награду
                }

                damage_indicator.SetTrigger("ally"); // Индикатор урон по краям карты
                txt_enemy_hp.text = enemy_health.ToString(); // Меняем текст хп
                enemy_slider.value = enemy_health; // Меняем значение слайдера с хп
            }
        }

        // Если игра закончена
        else
        {
            if (enabled == true)
            {
                GetComponent<ClassicGenerator>().StopAllCoroutines(); // Отключаем спавн вражеских юнитов
                enabled = false;
            }
        }
    }

    // Подготовить юнита для создания
    public void PrepareAllyUnit(string unit_name, int unit_cost, DefaultUnitButton unit_button)
    {
        ChoosedUnit = unit_name; // Записываем имя выбранного юнита
        this.unit_cost = unit_cost; // Записываем манакост выбранного юнита

        // Отключаем предыдущую обводку
        if (this.unit_button != null)
            this.unit_button.outline.SetActive(false);
        this.unit_button = unit_button; // Записываем новую кнопку выбранного юнита

        spawn_manager.PrepareAllyUnit(unit_name);
    }

    // Создаём союзного юнита
    public void CreateAllyyUnit(byte lane_id)
    {
        CurrentMana -= unit_cost; // Отнимаем ману за создание юнита
        unit_button.PlayAnim(); // Анимируем увеличение кнопки выбранного юнита
        mana_button.AnimateText(unit_cost); // Запускаем анимацию текста затраченной маны
        spawn_manager.SpawnUnit(lane[lane_id - 1].position);

        GlobalStats.AddToStats("Units Summoned");

        // Звук спавна
        if (audio_manager.IsOn()) audio_s.Play();

        // Если маны не хватает на создание юнита, отключаем кнопки спавна
        if (CurrentMana < unit_cost)
            SpawnButtonsCondition(false);
    }

    /// <summary>
    /// Включаем/выключаем кнопки спавна
    /// </summary>
    /// <param name="enable">true - включаем, false - выключаем</param>
    public void SpawnButtonsCondition(bool enable)
    {
        for (int i = 0; i < 3; i++)
        {
            spawn_buttons[i].SetActive(enable);
        }
    }

    // Прокачиваем ману
    public void BoostMana()
    {
        CurrentMana -= UpgradeCost; // Отнимаем стоимость апгрейда от текущей маны
        UpgradeCost *= 2; // Увеличиваем стоимость апгрейда
        txt_boost_price.text = "BOOST " + UpgradeCost;
        mana_regen_bonus += 0.27f; //0.24
        fade_effect.SetTrigger("activate"); // Эффект вспышки

        // Если текущей маны меньше чем нужно для создания юнита, выключаем кнопки спавна
        if (CurrentMana < unit_cost) SpawnButtonsCondition(false);
    }
}
