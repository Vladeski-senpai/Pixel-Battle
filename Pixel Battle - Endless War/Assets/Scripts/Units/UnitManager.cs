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
    public bool IsInfectedByZombie { get; set; } // Заражён ли вирусом "Обычного Зомби"
    public bool IsInfectedByFrozenZombie { get; set; } // Заражён ли вирусом "Замороженного Зомби"
    public bool IsInfectedBySpider { get; set; } // Заражён ли вирусом "Гигантского Паука"
    public bool IsDead { get; private set; } // Мёртв ли юнит
    #endregion

    #region Private Fields
    private UnitFightManager fight_manager;
    private AudioManager audio_manager;
    private UnitManager enemy;
    private Rigidbody2D rb;
    private AudioSource audio_s;
    private AudioClip clip;
    private Vector2 direction = Vector2.left;
    private GameObject 
        unit_wrapper, // Отключаем объект юнита в момент "смерти"
        shell_prefab; // Префаб снаряда

    private List<UnitEvasionEffect> body_parts = new List<UnitEvasionEffect>();

    private int 
        ninja_throws = 3, // Кол-во сюрикенов ниндзи
        gunslinger_shot; // Номер выстрела юнита Gunslinger, 0 - левое оружие, 1 - правое оружие

    private bool
        necroSlow, // Замедлен ли юнит Necromancer
        canBleed, // Может ли истекать кровью
        canPoisoned, // Может ли отравиться
        canInfected, // Может ли быть инфецирован (зомби/паук)
        havePerks,
        isAlly,
        isMelee;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audio_s = GetComponent<AudioSource>();
        fight_manager = GetComponent<UnitFightManager>();
        unit_wrapper = transform.GetChild(0).gameObject;
        animator = unit_wrapper.GetComponent<Animator>();
        IsDead = false;

        if (CompareTag("Ally"))
        {
            isAlly = true;
            direction = Vector2.right; // Направление движения
        }
    }

    private void Start()
    {
        audio_manager = AudioManager.instance; // Кэшируем скрипт
        particles_manager = ParticlesManager.instance; // Кэшируем скрипт

        PrepareBasicStats(); // Устанавливаем базовые статы юнита
        CalculateStats(); // Пересчитываем статы
        clip = audio_s.clip;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            AddEffect("Evasion");

        if (Input.GetKeyDown(KeyCode.P))
            AddEffect("Poisoned");

        if (Input.GetKeyDown(KeyCode.B))
            AddEffect("Bleeding");

        if (enemy != null && !fight_manager.attackCD)
        {
            // Если не застанен
            if (!fight_manager.isStunned)
            {
                // Звук удара/выстрела
                if (audio_manager.PlayHitSound()) audio_s.PlayOneShot(clip);

                fight_manager.attackCD = true;
                animator.SetBool("attacking", true);
                StartCoroutine(fight_manager.AttackCD(AttackSpeed)); // Запускаем откат кулдауна атаки
            }
        }

        // Проверяем не умер ли юнит
        if (Health <= 0) Death();
    }

    // Движение юнита
    private void FixedUpdate()
    {
        if (enemy == null && !fight_manager.isStunned)
        {
            rb.MovePosition((Vector2)transform.position + (direction * MoveSpeed * Time.deltaTime));
        }
        // Некромансер всегда двигается
        else if (isAlly && !fight_manager.isStunned && UnitClass == "Necromancer")
        {
            rb.MovePosition((Vector2)transform.position + (direction * MoveSpeed * Time.deltaTime));
        }
    }

    // Начинаем атаку (из аниматора)
    public void StartAttack()
    {
        if (isMelee)
        {
            Attack(false, enemy); // Проводим атаку
        }
        else
        {
            SpawnShell(); // Создаём снаряд
        }
    }

    // Проводим атаку
    public void Attack(bool isShell, UnitManager enemy)
    {
        if (enemy != null)
        {
            // Проверяем "защитные" перки
            float final_damage = enemy.CheckDefensivePerks(Damage, isShell, this);

            // Если атаку не заблокировали
            if (final_damage > 0)
            {
                // Если есть "атакующие" перки, проверяем их и наносим урон
                if (havePerks)
                {
                    enemy.DoDamage(fight_manager.CheckAttackingPerks(final_damage, isShell, enemy));
                }
                // Если "атакующих" перков нет, просто наносим возвращённый урон
                else
                {
                    enemy.DoDamage(final_damage);
                }

                // Создаём частицы попадания снаряда
                if (isShell) particles_manager.SpawnParticles("Shell Hit",
                    enemy.transform.position.x + Random.Range(-0.25f, 0.2f), enemy.transform.position.y + Random.Range(-0.05f, 0.45f));
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

    // Создаём снаряд
    public void SpawnShell()
    {
        if (isAlly && UnitClass == "Necromancer")
        {
            AdditionalUnitsSpawner.instance.SpawnUnit("Undead", transform.position.x - 1, transform.position.y);
            AdditionalUnitsSpawner.instance.SpawnUnit("Undead", transform.position.x + 2, transform.position.y);
        }
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
        else
        {
            GameObject temp = Instantiate(shell_prefab, fire_points[0].position, Quaternion.identity);
            temp.GetComponent<ShellManager>().SetStats(isAlly, UnitClass, this);

            // Если есть перки, проверяем уникальные для снаряда
            if (havePerks) fight_manager.CheckShellPerks(temp.GetComponent<ShellManager>());
        }
    }

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
                    if (stun_particles != null && !fight_manager.isStunned)
                    {
                        fight_manager.isStunned = true; // Указываем что юнит "оглушён"
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
                    if (canBleed && fight_manager.isBleeding == 0)
                    {
                        fight_manager.isBleeding = 20; // Кол-во тиков

                        // Отнимаем 20% от максимального здоровья за ~4 секунды, урон идёт каждые 0.2 секунды (20 = 4 сек * 5 тиков/сек)
                        StartCoroutine(fight_manager.EffectsTimer(code, 0.2f, MaxHealth * 0.2f / 20));

                        StartCoroutine(fight_manager.ChangeSpeed("Slow MS", time, value)); // Замедляем скорость передвижения на 5% на 4.5 секунды
                        StartCoroutine(fight_manager.ChangeSpeed("Slow AS", time, value)); // Замедляем скорость атаки на 5% на 4.5 секунды
                    }
                    break;

                // Добавляем эффект "отравления"
                case "Poisoned":
                    // Если юнит может "отравиться" и сейчас "отравления" нет, включаем эффект
                    if (canPoisoned && fight_manager.isPoisoned == 0)
                    {
                        fight_manager.isPoisoned = 25; // Кол-во тиков

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

                // После смерти превращаемся в "Обычного Зомби"
                case "Zombie Virus":
                    if (canInfected)
                    {
                        IsInfectedByZombie = true;
                        ChangeColor(code); // Меняем цвет на зеленоватый
                        StartCoroutine(fight_manager.EffectsTimer(code, time)); // Отключаем эффект через время
                        StartCoroutine(fight_manager.ChangeSpeed("Slow MS", time, value)); // Замедляем скорость передвижения
                        StartCoroutine(fight_manager.ChangeSpeed("Slow AS", time, value)); // Замедляем скорость атаки
                    }
                    break;

                // После смерти превращаемся в "Обычного Зомби"
                case "Frozen Zombie Virus":
                    if (canInfected)
                    {
                        IsInfectedByFrozenZombie = true;
                        StartCoroutine(fight_manager.EffectsTimer(code, time)); // Отключаем эффект через время
                        StartCoroutine(fight_manager.ChangeSpeed("Slow MS", time, value)); // Замедляем скорость передвижения
                        StartCoroutine(fight_manager.ChangeSpeed("Slow AS", time, value)); // Замедляем скорость атаки
                        ChangeColor(code); // Меняем цвет на зелено-голубоватый
                    }
                    break;

                // После смерти превращаемся в "Обычного Зомби"
                case "Spider Virus":
                    if (canInfected)
                    {
                        IsInfectedBySpider = true;
                        StartCoroutine(fight_manager.EffectsTimer(code, time)); // Отключаем эффект через время
                        StartCoroutine(fight_manager.ChangeSpeed("Slow MS", time, value)); // Замедляем скорость передвижения
                        StartCoroutine(fight_manager.ChangeSpeed("Slow AS", time, value)); // Замедляем скорость атаки
                        ChangeColor(code); // Меняем цвет на фиолетовый
                    }
                    break;
            }
        }
    }
    #endregion

    // Устанавливаем базовые статы юнита
    private void PrepareBasicStats()
    {
        UnitClass = UnitData.unit_class;
        isMelee = UnitData.isMelee;
        canBleed = UnitData.canBleed;
        canPoisoned = UnitData.canPoisoned;
        canInfected = UnitData.canInfected;

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

        // Если дальний бой, присваиваем снаряд
        if (!isMelee)
        {
            shell_prefab = UnitData.shell_prefab;

            // Турель не двигается
            if (UnitClass == "Turret")
            {
                BasicMS = 0;
                MoveSpeed = 0;
            }
        }

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

            // Если юнит тинкер, добавляем распознавание косаний
            if (UnitClass == "Tinker")
            {
                turret = unit_wrapper.transform.GetChild(1).gameObject;
                gameObject.AddComponent<UnitTouchEvents>().unit_class = UnitClass;
            }

        }
        else
        {
            Damage = ClassicDifficultSystem.CalculateEnemyStats(Damage) * 0.7f;
            MaxHealth = ClassicDifficultSystem.CalculateEnemyStats(MaxHealth);
            Health = MaxHealth;
        }
    }

    // Меняем цвет юнита ОПТИМИЗИРОВАТЬ!!!!!!!!!!!!!!!!!!
    public void ChangeColor(string code)
    {
        byte r = 255, g = 255, b = 255;

        switch (code)
        {
            case "Zombie Virus":
                r = 186;
                b = 173;
                break;

            case "Frozen Zombie Virus":
                r = 173;
                b = 217;
                break;

            case "Spider Virus":
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

    // [ DEATH ] ==========================================================

    // Убиваем юнита (от рук другого юнита/снаряда)
    public void Death()
    {
        if (unit_wrapper.activeSelf)
        {
            // Если вражеский юнит
            if (!isAlly)
            {
                int gold, xp;

                switch (UnitClass)
                {
                    case "Yeti":
                    case "Ogre":
                    case "Viper":
                    case "Minotaur":
                    case "Giant Spider":
                        gold = Random.Range(2, 6);
                        xp = Random.Range(8, 15);
                        break;

                    default:
                        gold = Random.Range(1, 4);
                        xp = Random.Range(6, 11);
                        break;
                }
                        
                GlobalStats.AddGold(gold); // Прибавляем золото
                GlobalStats.AddToStats("Enemies Killed"); // +1 убитый юнит в статистику
                PlayerLevelManager.player_level_manager.AddXP(xp); // Прибавляем опыт
                particles_manager.SpawnCoin(gold, transform.position.x - 0.1f, transform.position.y + 0.75f); // Частица монетки
            }
            else
            {
                string virus_type = ""; // Тип вируса: zmb - Зомби, frzmb - Зимний Зомби, spr - Гигансткий паук

                if (IsInfectedByZombie)
                    virus_type = "zmb";
                else if (IsInfectedByFrozenZombie)
                    virus_type = "frzmb";
                else if (IsInfectedBySpider)
                    virus_type = "spr";

                // Если есть вирус, создаём "паразитов"
                if (virus_type != "") AdditionalUnitsSpawner.instance.SpawnUnit(virus_type, transform.position.x, transform.position.y);
            }

            // Создаём частицы "смерти"
            particles_manager.SpawnParticles(isAlly + " Death", transform.position.x, transform.position.y + 0.35f);

            DisableUnit(); // Отключаем и уничтожаем юнита
        }
    }

    // Убиваем юнита без записи в статистику и без эффектов (вирусы и тп.)
    public void Destroy()
    {
        if (unit_wrapper.activeSelf)
        {
            // Создаём частицы "смерти"
            particles_manager.SpawnParticles(isAlly + " Death", transform.position.x, transform.position.y + 0.35f);

            DisableUnit(); // Отключаем и уничтожаем юнита
        }
    }

    // Убиваем юнита без частиц смерти, только частицы крови
    public void Execution()
    {
        if (unit_wrapper.activeSelf)
            DisableUnit(); // Отключаем и уничтожаем юнита
    }

    // Отключаем юнита и через время уничтожаем
    private void DisableUnit()
    {
        MoveSpeed = 0;
        unit_wrapper.SetActive(false); // Отключаем дочерний объект "содержащий" юнита
        fight_manager.StopAllCoroutines();
        fight_manager.isDead = true;
        fight_manager.enabled = false;
        IsDead = true;
        GetComponent<BoxCollider2D>().enabled = false; // Отключаем хит-бокс

        Destroy(gameObject, 4);
    }

    // [ TRIGGERS ] ==========================================================

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
}
