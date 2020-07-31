using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitFightManager))]
public class UnitManager : MonoBehaviour
{
    #region Public Fields
    public UnitData UnitData;
    public GameObject stun_particles; // Частицы стана

    public Transform[] fire_points;

    [HideInInspector]
    public ParticlesManager particles_manager;

    [HideInInspector]
    public Animator animator;

    [HideInInspector]
    public GameObject turret; // Спрайт турели у тинкера

    [HideInInspector]
    public Rigidbody2D rb;
    #endregion

    #region Get/Set Fields
    public string UnitClass { get; private set; }
    public float MaxHealth { get; private set; }
    public float Health { get; set; }
    public float Damage { get; set; }
    public float BasicMS { get; set; } // Базовая скорость передвижения
    public float MoveSpeed { get; set; }
    public float BasicAS { get; set; } // Базовая скорость атаки
    public float AttackSpeed { get; set; }
    public int VirusType { get; set; } // Заражён ли вирусом Зомби/Паука (0 - нет), типы: 1 - обычный, - 2 зимний, 3 - пустынный, 4 - тёмный; Паук - 5
    public bool IsDead { get; private set; } // Мёртв ли юнит
    public bool CanBePushed { get; private set; } // Может ли быть толкнутым
    #endregion

    #region Private Fields
    private UnitFightManager fight_manager;
    private AudioManager audio_manager;
    private UnitManager enemy;
    private AudioSource audio_s;
    private AudioClip clip;
    private CameraShake cam_shake;
    private GameObject
        unit_wrapper, // Отключаем объект юнита в момент "смерти"
        shell_prefab; // Префаб снаряда

    private List<UnitEvasionEffect> body_parts = new List<UnitEvasionEffect>();

    private float
        slow_strength, // Время замедления времени при ударах
        punch_strength, // Сила толчка
        newX;

    private float[] death_particles_colors; // Цвета частиц смерти (rgb x3)

    private int
        dp_size, // Размер частиц смерти (0 - обычные, 1 - большие)
        direction = -1, // Направление движения
        turrets = 1, // Кол-во турелей
        slow_frames, // Кол-во кадров замедления времени при ударах
        punch_chance, // Шанс толчка
        ninja_throws = 3, // Кол-во сюрикенов ниндзи
        gunslinger_shot; // Номер выстрела юнита Gunslinger, 0 - левое оружие, 1 - правое оружие


    private bool
        necroSlow, // Замедлен ли юнит Necromancer
        canAttack = true, // Может ли атаковать
        canBleed, // Может ли истекать кровью
        canBePoisoned, // Может ли отравиться
        canBeInfected, // Может ли быть инфецирован (зомби/паук)
        canAlwaysMove, // Может ли всегда двигаться (кроме когда оглушён)
        shakeHits, // Трясётся ли камера при ударах
        shakeShoot, // Трясётся ли камера В НАЧАЛЕ удара
        havePerks,
        isAlly,
        isMelee;
    #endregion

    private void Awake()
    {
        audio_s = GetComponent<AudioSource>();
        fight_manager = GetComponent<UnitFightManager>();
        unit_wrapper = transform.GetChild(0).gameObject;
        animator = unit_wrapper.GetComponent<Animator>();
        IsDead = false;

        if (CompareTag("Ally"))
        {
            isAlly = true;
            direction = 1; // Направление движения
        }
    }

    private void Start()
    {
        audio_manager = AudioManager.instance; // Кэшируем скрипт
        particles_manager = ParticlesManager.instance; // Кэшируем скрипт
        cam_shake = CameraShake.instance;
        rb = GetComponent<Rigidbody2D>();

        PrepareBasicStats(); // Устанавливаем базовые статы юнита
        CalculateStats(); // Пересчитываем статы
        clip = audio_s.clip;
    }

    private void Update()
    {
        if (enemy != null && !fight_manager.attackCD && !enemy.IsDead && canAttack)
        {
            // Если не застанен
            if (!fight_manager.IsStunned)
            {
                if (audio_manager.PlayHitSound()) audio_s.PlayOneShot(clip); // Звук удара/выстрела

                // Если жив
                if (!IsDead) animator.SetBool("attacking", true);

                fight_manager.attackCD = true;
                StartCoroutine(fight_manager.AttackCD(AttackSpeed)); // Запускаем откат кулдауна атаки
            }
        }

        if (enemy == null && !fight_manager.IsStunned) Movement(); // Двигаемся когда врага нет и не оглушены
        else if (enemy != null && canAlwaysMove && !fight_manager.IsStunned) Movement(); // Двигаемся всегда, кроме когда оглушены

        // Проверяем не умер ли юнит
        if (Health <= 0) Death();
        else if (Health > MaxHealth) Health = MaxHealth;
    }

    // Движение юнита
    private void Movement()
    {
        newX = Mathf.MoveTowards(transform.position.x, transform.position.x + direction, MoveSpeed * Time.deltaTime);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    #region Battle
    // Начинаем атаку (из аниматора)
    public void StartAttack()
    {
        if (shakeShoot) cam_shake.SmallShake(); // Трясём камеру

        if (isMelee) Attack(false, enemy); // Проводим атаку
        else SpawnShell(); // Создаём снаряд
    }

    // Проводим атаку
    public void Attack(bool isShell, UnitManager enemy)
    {
        if (enemy != null && !enemy.IsDead)
        {
            // Проверяем "защитные" перки
            float final_damage = enemy.CheckDefensivePerks(Damage, isShell, this);

            // Если атаку не заблокировали
            if (final_damage > 0)
            {
                // Трясём камеру при ударах
                if (shakeHits) cam_shake.SmallShake();

                // Если есть "атакующие" перки, проверяем их и наносим урон
                if (havePerks) enemy.DoDamage(fight_manager.CheckAttackingPerks(final_damage, isShell, enemy));
                else enemy.DoDamage(final_damage); // Если "атакующих" перков нет, просто наносим возвращённый урон

                // Толкаем противника
                if (punch_strength != 0 && Random.Range(0, 99) < punch_chance && enemy.CanBePushed)
                {
                    enemy.rb.AddForce(Vector2.right * 
                        Random.Range(punch_strength - 1.2f, punch_strength + 1.2f) * direction, ForceMode2D.Impulse);
                }

                // Замедляем время на доли секунд при нанесении урона
                if (Time.timeScale == 1)
                {
                    Time.timeScale = slow_strength;
                    StartCoroutine(ResumeTime());
                }

                // Создаём частицы попадания снаряда
                if (isShell) particles_manager.SpawnParticles("Shell Hit",
                    enemy.transform.position.x + Random.Range(-0.25f, 0.2f),
                    enemy.transform.position.y + Random.Range(-0.05f, 0.45f));
            }
            else
            {
                //Debug.Log("Hit/shell was blocked/evaded by - " + enemy + ", from enemy - " + name);
            }
        }
    }

    // Проверяем защитные перки "после начала атаки"
    public float CheckDefensivePerks(float damage, bool isShell, UnitManager attacking_unit)
    {
        // Если есть перки у текущего юнита
        if (havePerks)
        {
            // Если урон от ЮНИТА, записываем его
            if (!isShell) fight_manager.attacking_unit = attacking_unit;

            // Проверяем "защитные" перки и наносим возращённый урон
            return fight_manager.CheckDefensivePerks(damage, isShell);
        }

        // Если перков нет, возвращаем урон
        else return damage;
    }

    // Получаем урон (от другого юнита/снаряда)
    public void DoDamage(float damage)
    {
        Health -= damage;
    }

    // Возобновляем время после замедления
    private IEnumerator ResumeTime()
    {
        for (int i = 0; i < slow_frames; i++)
        {
            yield return null;
        }

        Time.timeScale = 1;
    }

    // Создаём снаряд
    public void SpawnShell()
    {
        if (isAlly && UnitClass == "Necromancer")
        {
            AdditionalUnitsSpawner.instance.SpawnUnit("Undead", transform.position.x - 1, transform.position.y);
            AdditionalUnitsSpawner.instance.SpawnUnit("Undead", transform.position.x + 2, transform.position.y);
        }

        // Если стрелок
        else if (isAlly && UnitClass == "Gunslinger")
        {

            GameObject temp = Instantiate(shell_prefab, fire_points[gunslinger_shot].position, Quaternion.identity);
            temp.GetComponent<ShellManager>().SetStats(isAlly, UnitClass, this);

            gunslinger_shot++;
            if (gunslinger_shot > 1)
            {
                gunslinger_shot = 0;

                // Звук удара/выстрела
                if (audio_manager.PlayHitSound()) audio_s.PlayOneShot(clip);
            }

            // Если есть перки, проверяем уникальные для снаряда
            if (havePerks) fight_manager.CheckShellPerks(temp.GetComponent<ShellManager>());
        }

        // Если юнит ниндзя, создаём сюрикены
        else if (isAlly && UnitClass == "Ninja")
        {
            GameObject temp;
            for (int i = 0; i < 3; i++)
            {
                temp = Instantiate(shell_prefab, fire_points[gunslinger_shot].position, Quaternion.identity);
                temp.GetComponent<ShellManager>().SetStats(isAlly, UnitClass, this);
                temp.AddComponent<ShurikenSpinAnimation>().shuriken_id = i;
            }

            ninja_throws--;
            if (ninja_throws == 0)
            {
                isMelee = true;
                enemy = null;
                audio_s.volume = 1;
                unit_wrapper.transform.GetChild(0).gameObject.SetActive(false); // Отключаем дальний детектор
                unit_wrapper.transform.GetChild(1).gameObject.SetActive(false); // Отключаем сюрикен
                animator.SetBool("range", false);
            }
        }

        else if (!isAlly && UnitClass == "Dark Slime")
        {
            GameObject temp;
            for (int i = 0; i < 3; i++)
            {
                temp = Instantiate(shell_prefab, fire_points[i].position, Quaternion.identity);
                temp.GetComponent<ShellManager>().SetStats(isAlly, UnitClass, this);
                temp.GetComponent<ShellManager>().spike_id = i + 1;
            }
        }
        else
        {
            GameObject temp = Instantiate(shell_prefab, fire_points[0].position, Quaternion.identity);
            temp.GetComponent<ShellManager>().SetStats(isAlly, UnitClass, this);

            // Если есть перки, проверяем уникальные для снаряда
            if (havePerks) fight_manager.CheckShellPerks(temp.GetComponent<ShellManager>());
        }
    }

    // Способности по вызову нажатия
    public void TouchAbilities()
    {
        if (UnitClass == "Tinker" && turrets > 0)
        {
            turrets--;
            turret.SetActive(false); // Отключаем спрайт турели тинкера
            AdditionalUnitsSpawner.instance.SpawnUnit("Turret", transform.position.x - 0.217f, transform.position.y + 0.12f);
        }
        else if (UnitClass == "Shieldman")
        {
            animator.SetBool("block", canAttack);
            canAlwaysMove = canAttack;
            canAttack = !canAttack;
            CanBePushed = canAttack;

            if (!canAttack) StartCoroutine(fight_manager.ChangeSpeed("Slow MS", 0.75f));
            else StartCoroutine(fight_manager.ChangeSpeed("Boost MS", 0.75f));
        }
    }
    #endregion

    #region Add Effects
    /// <summary>
    /// Добавляем эффект
    /// </summary>
    /// <param name="code">Название эффекта</param>
    public void AddEffect(string code)
    {
        AddEffect(code, 0, 0);
    }

    /// <summary>
    /// Добавляем эффект
    /// </summary>
    /// <param name="code">Название эффекта</param>
    /// <param name="time">Длительность эффекта</param>
    public void AddEffect(string code, float time)
    {
        AddEffect(code, time, 0);
    }

    /// <summary>
    /// Добавляем эффект
    /// </summary>
    /// <param name="code">Название эффекта</param>
    /// <param name="time">Длительность эффекта</param>
    /// <param name="value">Значение эффекта</param>
    public void AddEffect(string code, float time, float value)
    {
        // Если юнит жив
        if (!IsDead)
        {
            switch (code)
            {
                // Добавляем эффект "оглушения"
                case "Stunned":
                    // Если юнит может быть оглушаемым и не оглушён, оглушаем его
                    if (stun_particles != null && !fight_manager.IsStunned)
                    {
                        CameraShake.instance.SmallShake(); // Трясём камеру
                        fight_manager.IsStunned = true; // Указываем что юнит "оглушён"
                        animator.SetBool("stunned", true); // Включаем анимацию "оглушения"
                        stun_particles.SetActive(true); // Включаем частицы "оглушения"
                        StartCoroutine(fight_manager.EffectsTimer(code, time)); // Запускаем таймер отключения "оглушения"
                    }
                    break;

                // Добавляем эффект "уклонения"
                case "Evasion":
                    // Если части тела не записаны (при первом запуске)
                    if (body_parts.Count == 0)
                    {
                        // Проверяем все дочерние объекты
                        for (int i = 0; i < unit_wrapper.transform.childCount; i++)
                        {
                            // Если объект - часть тела, то добавляем скрипт и кэшируем его
                            if (unit_wrapper.transform.GetChild(i).CompareTag("BodyPart"))
                            {
                                unit_wrapper.transform.GetChild(i).gameObject.AddComponent<UnitEvasionEffect>().isNew = true;
                                body_parts.Add(unit_wrapper.transform.GetChild(i).GetComponent<UnitEvasionEffect>());
                            }
                        }
                    }

                    // Создаём спрайт и включаем эффект исчезновения
                    for (int i = 0; i < body_parts.Count; i++)
                    {
                        body_parts[i].SpawnSprite();
                    }
                    break;

                // Добавляем эффект "кровотечения"
                case "Bleeding":
                    // Если юнит может "кровоточить" и сейчас "кровотечения" нет, включаем эффект
                    if (canBleed && fight_manager.IsBleeding == 0)
                    {
                        fight_manager.IsBleeding = 20; // Кол-во тиков

                        // Отнимаем 20% от максимального здоровья за ~4 секунды, урон идёт каждые 0.2 секунды (20 = 4 сек * 5 тиков/сек)
                        StartCoroutine(fight_manager.EffectsTimer(code, 0.2f, MaxHealth * 0.2f / 20));

                        StartCoroutine(fight_manager.ChangeSpeed("Slow MS", time, value)); // Замедляем скорость передвижения на 5% на 4.5 секунды
                        StartCoroutine(fight_manager.ChangeSpeed("Slow AS", time, value)); // Замедляем скорость атаки на 5% на 4.5 секунды
                    }
                    break;

                // Добавляем эффект "отравления"
                case "Poisoned":
                    // Если юнит может "отравиться" и сейчас "отравления" нет, включаем эффект
                    if (canBePoisoned && fight_manager.IsPoisoned == 0)
                    {
                        fight_manager.IsPoisoned = 25; // Кол-во тиков

                        // Отнимаем 25% от максимального здоровья за ~5 секунд, урон идёт каждые 0.2 секунды (25 = 5 сек * 5 тиков/сек)
                        StartCoroutine(fight_manager.EffectsTimer(code, 0.2f, MaxHealth * 0.25f / 25));

                        StartCoroutine(fight_manager.ChangeSpeed("Slow MS", time, value)); // Замедляем скорость передвижения на 10% на 5.5 секунд
                        StartCoroutine(fight_manager.ChangeSpeed("Slow AS", time, value)); // Замедляем скорость атаки на 10% на 5.5 секунд
                    }
                    break;

                // Замедляем скорость атаки и передвижения
                case "Sticky Web":
                case "Freezing":
                    ChangeColor(code); // Меняем цвет на голубоватый (только для Frozen Bite)
                    StartCoroutine(fight_manager.EffectsTimer(code, time)); // Меняем цвет на начальный через время
                    StartCoroutine(fight_manager.ChangeSpeed("Slow MS", time, value)); // Замедляем скорость передвижения
                    StartCoroutine(fight_manager.ChangeSpeed("Slow AS", time, value)); // Замедляем скорость атаки
                    break;

                // После смерти превращаемся в "Зомби" / "Гигантского паука"
                case "Virus":
                    if (canBeInfected)
                    {
                        ChangeColor(VirusType.ToString()); // Меняем цвет на зеленоватый
                        StartCoroutine(fight_manager.EffectsTimer(code, time)); // Отключаем эффект через время
                        StartCoroutine(fight_manager.ChangeSpeed("Slow MS", time, value)); // Замедляем скорость передвижения
                        StartCoroutine(fight_manager.ChangeSpeed("Slow AS", time, value)); // Замедляем скорость атаки
                    }
                    break;
            }
        }
    }
    #endregion

    #region Other
    // Устанавливаем базовые статы юнита
    private void PrepareBasicStats()
    {
        UnitClass = UnitData.unit_class;
        isMelee = UnitData.isMelee;
        CanBePushed = UnitData.canBePushed;
        dp_size = UnitData.death_particles_size;
        shakeHits = UnitData.shakeHits;
        shakeShoot = UnitData.shakeShoot;
        canBleed = UnitData.canBleed;
        canBePoisoned = UnitData.canBePoisoned;
        canBeInfected = UnitData.canBeInfected;
        canAlwaysMove = UnitData.canAlwaysMove;

        audio_s.clip = UnitData.hit_sfx; // Звук удара/выстрела
        audio_s.volume = UnitData.hit_sfx_volume;
        audio_s.pitch = UnitData.hit_sfx_pitch;

        MaxHealth = UnitData.health;
        Health = MaxHealth;
        Damage = UnitData.damage;
        BasicMS = Random.Range(UnitData.move_speed - 0.05f, UnitData.move_speed + 0.05f); // Небольшой разброс
        MoveSpeed = BasicMS;
        BasicAS = UnitData.attack_speed;
        AttackSpeed = BasicAS;

        slow_strength = UnitData.slow_strength;
        slow_frames = UnitData.slow_frames;
        punch_strength = UnitData.punch_strength;
        punch_chance = UnitData.punch_chance;

        death_particles_colors = UnitData.GetColors();  // Записываем цвета частиц смерти

        // Если дальний бой, присваиваем снаряд
        if (!isMelee)
        {
            shell_prefab = UnitData.shell_prefab;

            // Турель не двигается
            if (UnitClass == "Turret")
            {
                BasicMS = 0;
                MoveSpeed = 0;
                Invoke("Destroy", 40);

                GlobalData.SetInt(UnitClass, GlobalData.GetInt("Tinker"));

                // Звук спавна
                if (audio_manager.IsOn())
                    audio_s.PlayOneShot(UnitData.spawn_sfx);
            }
        }

        if (isAlly)
        {
            if (UnitClass == "Undead")
                GlobalData.SetInt(UnitClass, GlobalData.GetInt("Necromancer"));

            transform.position = new Vector3(transform.position.x, transform.position.y, Random.Range(1, 30));
        }
        else transform.position = new Vector3(transform.position.x, transform.position.y, Random.Range(-50, -1));

        // Если может менять линии
        if (UnitData.canSwapLanes) gameObject.AddComponent<UnitLaneSwap>().MoveSpeed = MoveSpeed;

        // Если противник, создаём компонент звука
        if (!isAlly && audio_manager.PlaySpawnSound())
        {
            unit_wrapper.AddComponent<AudioSource>().playOnAwake = false;
            AudioSource spawn_sfx = unit_wrapper.GetComponent<AudioSource>();
            spawn_sfx.clip = UnitData.spawn_sfx;
            spawn_sfx.volume = UnitData.spawn_sfx_volume;
            spawn_sfx.pitch = UnitData.spawn_sfx_pitch;
            spawn_sfx.Play();
        }

        // Если есть перки, записываем их
        if (UnitData.Perks.Count > 0)
        {
            havePerks = true;

            foreach (Perks perk in UnitData.Perks)
            {
                fight_manager.PerksNames.Add(perk.PerkName);
                fight_manager.PerksChances.Add(perk.PerkChance);
                fight_manager.PerksDurations.Add(perk.PerkDuration);
                fight_manager.PerksValues.Add(perk.PerkValue);
            }
        }
    }

    // Пересчитываем статы на основе текущего уровня и тд.
    private void CalculateStats()
    {
        if (isAlly)
        {
            int unit_lvl = GlobalData.GetInt(UnitClass);
            Damage = ClassicDifficultSystem.CalculateAllyStats(Damage, unit_lvl);
            MaxHealth = ClassicDifficultSystem.CalculateAllyStats(MaxHealth, unit_lvl);
            Health = MaxHealth;

            // Если юнит тинкер, записываем турель
            if (UnitClass == "Tinker") turret = unit_wrapper.transform.GetChild(1).gameObject;
        }
        else
        {
            if (ArenaManager.instance.isArenaOn)
            {
                //value* Mathf.Pow(wave_num + 10, 1.01f) * 0.085f;
                Damage *= (Mathf.Pow(ArenaManager.instance.wave_num + 10, 1.01f) * 0.095f) * 0.7f;
                MaxHealth *= Mathf.Pow(ArenaManager.instance.wave_num + 10, 1.01f) * 0.095f;
                Health = MaxHealth;
            }
            else
            {
                Damage = ClassicDifficultSystem.CalculateEnemyStats(Damage) * 0.7f;
                MaxHealth = ClassicDifficultSystem.CalculateEnemyStats(MaxHealth);
                Health = MaxHealth;
            }
        }
    }

    // Меняем цвет юнита ОПТИМИЗИРОВАТЬ!!!!!!!!!!!!!!!!!!
    public void ChangeColor(string code)
    {
        byte r = 255, g = 255, b = 255;

        switch (code)
        {
            case "1": // Обычный зомби
                r = 186;
                b = 173;
                break;

            case "2": // Вирус Зимнего зомби
                r = 173;
                b = 217;
                break;

            case "3": // Пустынный зомби
                r = 248;
                b = 173;
                break;

            case "4": // Тёмный зомби
                r = 225;
                g = 182;
                break;

            case "5": // Вирус Гигантского паука
                g = 172;
                b = 177;
                break;

            case "Freezing":
                r = 173;
                b = 249;
                break;
        }

        // Проверяем все дочерние объекты
        for (int i = 0; i < unit_wrapper.transform.childCount; i++)
        {
            // Если объект - часть тела, то добавляем скрипт и кэшируем его
            if (unit_wrapper.transform.GetChild(i).CompareTag("BodyPart"))
            {
                unit_wrapper.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color32(r, g, b, 255);
            }
        }
    }
    #endregion

    #region Death's
    // Убиваем юнита (от рук другого юнита/снаряда)
    public void Death()
    {
        if (!IsDead)
        {
            // Частицы смерти
            particles_manager.DeathParticles(dp_size, death_particles_colors, transform.position.x, transform.position.y + 0.7f);

            // Если вражеский юнит
            if (!isAlly)
            {
                cam_shake.SmallShake(); // Дёргаем камеру
                int gold, xp;

                switch (UnitClass)
                {
                    case "Yeti":
                    case "Ogre":
                    case "Viper":
                    case "Minotaur":
                    case "Giant Spider":
                        if (ClassicGenerator.instance.map_type != 4)
                        {
                            gold = Random.Range(2, 6);
                            xp = Random.Range(9, 15);
                        }
                        else
                        {
                            gold = Random.Range(4, 8);
                            xp = Random.Range(2, 8);
                        }
                        break;

                    default:
                        // Обычный режим
                        if (ClassicGenerator.instance.map_type != 4)
                        {
                            gold = Random.Range(1, 4);
                            xp = Random.Range(6, 11);
                        }
                        // Арена
                        else
                        {
                            gold = Random.Range(3, 6);
                            xp = Random.Range(1, 5);
                        }
                        break;
                }
                        
                GlobalStats.AddGold(gold); // Прибавляем золото
                GlobalStats.AddToStats("Enemies Killed"); // +1 убитый юнит в статистику
                PlayerLevelManager.player_level_manager.AddXP(xp); // Прибавляем опыт
                particles_manager.SpawnCoin(gold, transform.position.x - 0.1f, transform.position.y + 0.75f); // Частица монетки
            }
            else
            {
                // Если есть вирус, создаём "паразитов"
                if (VirusType > 0)
                {
                    AdditionalUnitsSpawner.instance.SpawnParasite(VirusType, transform.position.x, transform.position.y);
                    particles_manager.SpawnParticles("Death", transform.position.x, transform.position.y + 0.35f);
                }
            }

            DisableUnit(); // Отключаем и уничтожаем юнита
        }
    }

    // Убиваем юнита без записи в статистику и без эффектов (вирусы и тп.)
    public void Destroy()
    {
        if (!IsDead)
        {
            // Создаём частицы "смерти"
            particles_manager.SpawnParticles("Death", transform.position.x, transform.position.y + 0.35f);

            DisableUnit(); // Отключаем и уничтожаем юнита
        }
    }

    // Убиваем юнита без частиц смерти, только частицы крови
    public void Execution()
    {
        if (!IsDead)
            DisableUnit(); // Отключаем и уничтожаем юнита
    }

    // Отключаем юнита и через время уничтожаем
    private void DisableUnit()
    {
        MoveSpeed = 0;
        audio_manager.CheckDeathSound(isAlly); // Звук смерти юнита
        fight_manager.StopAllCoroutines();
        fight_manager.IsDead = true;
        fight_manager.enabled = false;
        IsDead = true;
        GetComponent<BoxCollider2D>().enabled = false; // Отключаем хит-бокс

        Destroy(unit_wrapper);
        Destroy(gameObject, 4);
    }
    #endregion

    #region Triggers
    private void OnTriggerEnter2D(Collider2D col)
    {
        // Если союзный юнит обнаружил вражеского юнита
        if (isAlly && col.CompareTag("Enemy"))
        {
            enemy = col.GetComponent<UnitManager>();

            if (UnitClass == "Necromancer" && !necroSlow)
            {
                necroSlow = true;
                StartCoroutine(fight_manager.ChangeSpeed("Slow MS", 0.45f));
            }
        }

        // Если вражеский юнит обнаружил союзного юнита
        else if (!isAlly && col.CompareTag("Ally"))
        {
            enemy = col.GetComponent<UnitManager>();
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        // Если союзный юнит обнаружил вражеского юнита
        if (isAlly && col.CompareTag("Enemy"))
        {
            enemy = col.GetComponent<UnitManager>();
        }

        // Если вражеский юнит обнаружил союзного юнита
        else if (!isAlly && col.CompareTag("Ally"))
        {
            enemy = col.GetComponent<UnitManager>();
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        // Если союзный юнит обнаружил вражеского юнита
        if (isAlly && col.CompareTag("Enemy"))
        {
            enemy = null;

            if (UnitClass == "Necromancer" && necroSlow)
            {
                necroSlow = false;
                StartCoroutine(fight_manager.ChangeSpeed("Boost MS", 0.45f));
            }
        }

        // Если вражеский юнит обнаружил союзного юнита
        else if (!isAlly && col.CompareTag("Ally"))
        {
            enemy = null;
        }
    }
    #endregion
}
