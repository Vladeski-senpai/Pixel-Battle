using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFightManager : MonoBehaviour
{
    [HideInInspector]
    public List<string> PerksNames = new List<string>(); // Здесь хранятся названия полученных перков

    [HideInInspector]
    public List<float>
        PerksChances = new List<float>(), // Здесь хранятся значения шансов полученных перков
        PerksDurations = new List<float>(), // Здесь хранятся длительности полученных перков
        PerksValues = new List<float>(); // Здесь хранятся различные значения от полученных перков

    public UnitManager attacking_unit { get; set; } // Вражеский юнит, который нас атакует

    public float isBleeding { get; set; } // Истекает ли кровью (>0 - да)
    public float isPoisoned { get; set; } // Отравлен ли (>0 - да)
    public bool attackCD { get; set; } // В кулдауне ли атака
    public bool isStunned { get; set; } // Застанен ли юнит
    public bool isDead { get; set; } // Мёртв ли юнит

    private UnitManager unit_manager;

    private float 
        ms_slow, // Замедление к скорости передвижения
        f_ms_slow, // Финальное замедление скорости передвижения (для ограничения)
        ms_bonus, // Ускорение скорости передвижения
        as_slow, // Замедление к скорости атаки
        f_as_slow, // Финальное замедление скорости атаки (для ограничения)
        as_bonus, // Ускорение скорости атаки
        final_damage; // Возвращаемый урон

    private void Awake()
    {
        unit_manager = GetComponent<UnitManager>();
    }

    #region Checks for Perks
    /// <summary>
    /// Проверяем перки, активирующиеся при атаке
    /// </summary>
    /// <param name="damage">Возвращаемый урон</param>
    /// <param name="enemy">Атакуем противник</param>
    public float CheckAttackingPerks(float damage, bool isShell, UnitManager enemy)
    {
        final_damage = damage;

        for (int i = 0; i < PerksNames.Count; i++)
        {
            switch (PerksNames[i])
            {
                // Вызываем "кровотечение" у противника
                case "Deep Cut":
                    if (RandomChance() < PerksChances[i])
                        enemy.AddEffect("Bleeding", PerksDurations[i], PerksValues[i]);
                    break;

                // Вызываем "кровотечение" у противника (только снаряд)
                case "Sharp Projectile":
                    if (isShell)
                    {
                        if (RandomChance() < PerksChances[i])
                            enemy.AddEffect("Bleeding", PerksDurations[i], PerksValues[i]);
                    }
                    break;

                // "Отравляем" противника
                case "Poison Hit":
                    if (RandomChance() < PerksChances[i])
                        enemy.AddEffect("Poisoned", PerksDurations[i], PerksValues[i]);
                    break;

                // "Казним" противника
                case "Execution":
                    if (RandomChance() < PerksChances[i])
                    {
                        enemy.Execution(); // Убиваем противника

                        unit_manager.particles_manager.SpawnParticles("Execution",
                            enemy.transform.position.x, enemy.transform.position.y + 0.2f);
                    }
                    break;

                // "Оглушаем" противника
                case "Stun":
                    if (RandomChance() < PerksChances[i])
                        enemy.AddEffect("Stunned", PerksDurations[i]);
                    break;

                // "Замедляем" скорость атаки/передвижения
                case "Sticky Web":
                case "Freezing":
                    if (RandomChance() < PerksChances[i])
                        enemy.AddEffect(PerksNames[i], PerksDurations[i], PerksValues[i]);
                    break;

                // Увеличиваем урон удара
                case "Critical Hit":
                    if (RandomChance() < PerksChances[i])
                    {
                        final_damage *= PerksValues[i]; // Форма записи: "2 = х2 урон"

                        // Создаём частицу критического попадания (красный череп)
                        unit_manager.particles_manager.SpawnParticles("Critical",
                            enemy.transform.position.x, enemy.transform.position.y + 1.2f);
                    }
                    break;

                // Заражаем вирусом "Обычного Зомби"/"Замороженного Зомби"/"Гигантского Паука"
                case "Zombie Virus":
                case "Frozen Zombie Virus":
                case "Spider Virus":
                    if (RandomChance() < PerksChances[i])
                        enemy.AddEffect(PerksNames[i], PerksDurations[i], PerksValues[i]);
                    break;
            }
        }

        return final_damage;
    }

    /// <summary>
    /// Проверяем перки, активирующиеся при получении урона
    /// </summary>
    /// <param name="damage">Урон наносимый юниту</param>
    /// <param name="isShell">true - урон от снаряда, false - урон от юнита</param>
    public float CheckDefensivePerks(float damage, bool isShell)
    {
        final_damage = damage;

        for (int i = 0; i < PerksNames.Count; i++)
        {
            switch (PerksNames[i])
            {
                // Анулируем урон и оглушаем атакующего
                case "Parry Stun":
                    if (!isShell)
                    {
                        if (RandomChance() < PerksChances[i])
                        {
                            attacking_unit.AddEffect("Stunned", PerksDurations[i]); // Добавляем эффект "оглушения" атакующему нас
                            final_damage = 0; // Возращаем 0 урона, т.к. анулировали его
                        }
                    }
                    break;

                // Анулируем урон
                case "Evasion":
                    if (RandomChance() < PerksChances[i])
                    {
                        unit_manager.AddEffect("Evasion"); // Активируем эффект "уклонения"
                        final_damage = 0; // Возращаем 0 урона, т.к. анулировали его
                    }
                    break;

                // Поглощаем часть урона
                case "Damage Reduce":
                    final_damage -= final_damage * PerksValues[i];
                    break;

                // Поглощаем часть урона снаряда
                case "Shell Reduce":
                    if (isShell)
                        final_damage -= final_damage * PerksValues[i];
                    break;

                // Анулируем урон снаряда
                case "Shell Block":
                    // Если урон от снаряда
                    if (isShell)
                    {
                        if (RandomChance() < PerksChances[i])
                        {
                            final_damage = 0;

                            // Создаём частицы блокировки снаряда
                            unit_manager.particles_manager.SpawnParticles("Shell Block",
                                transform.position.x + Random.Range(-0.15f, 0.2f), transform.position.y + Random.Range(-0.1f, 0.5f));
                        }
                    }
                    break;
            }
        }

        return final_damage; // Возвращаем урон
    }

    // Проверяем уникальные перки для снарядов
    public void CheckShellPerks(ShellManager shell)
    {
        for (int i = 0; i < PerksNames.Count; i++)
        {
            switch (PerksNames[i])
            {
                // Пробиваем несколько целей
                case "Piercing Shell":
                    // Устаналиваем здоровье равное кол-ву пробиваемых целей (1 цель минус 1 здоровье)
                    if (RandomChance() < PerksChances[i])
                        shell.Health = (int)PerksValues[i];
                    break;
            }
        }
    }
    #endregion

    #region Different Effects
    // Таймеры для эффектов перков
    public IEnumerator EffectsTimer(string code, float time)
    {
        StartCoroutine(EffectsTimer(code, time, 0));
        yield return null;
    }
    public IEnumerator EffectsTimer(string code, float time, float value)
    {
        yield return new WaitForSeconds(time);

        if (!isDead)
        {
            switch (code)
            {
                // Выключаем эффект "оглушения"
                case "Stunned":
                    isStunned = false; // Указываем что юнит не "оглушён"
                    unit_manager.animator.SetBool("stunned", false); // Отключаем анимацию "оглушения"
                    unit_manager.stun_particles.SetActive(false); // Выключаем частицы стана
                    break;

                // Наносим урон от "кровотечения"
                case "Bleeding":
                    // Если "кровотечение" включено
                    if (isBleeding > 0)
                    {
                        isBleeding--; // Отнимаем один тик
                        unit_manager.Health -= value; // Наносим урон
                        StartCoroutine(EffectsTimer(code, time, value)); // Запускаем новый тик

                        // Создаём частицы крови
                        unit_manager.particles_manager.SpawnParticles("Blood",
                            transform.position.x + Random.Range(-0.2f, 0.2f), transform.position.y + Random.Range(-0.1f, 0.22f));
                    }
                    else isBleeding = 0;
                    break;

                // Наносим урон от "отравления"
                case "Poisoned":
                    // Если "отравление" включено
                    if (isPoisoned > 0)
                    {
                        isPoisoned--; // Отнимаем один тик
                        unit_manager.Health -= value; // Наносим урон
                        StartCoroutine(EffectsTimer(code, time, value)); // Запускаем новый тик

                        // Создаём частицы яда
                        unit_manager.particles_manager.SpawnParticles("Poison",
                            transform.position.x + Random.Range(-0.23f, 0.21f), transform.position.y + Random.Range(-0.1f, 0.27f));
                    }
                    else isPoisoned = 0;
                    break;

                // Замедление скорости атаки/передвижения
                case "Freezing":
                    unit_manager.ChangeColor(""); // Возвращаем стандартный цвет
                    break;

                // Отключаем вирус "Обычного Зомби"
                case "Zombie Virus":
                    unit_manager.IsInfectedByZombie = false;
                    unit_manager.ChangeColor(""); // Возвращаем стандартный цвет
                    break;

                // Отключаем вирус "Замороженного Зомби"
                case "Frozen Zombie Virus":
                    unit_manager.IsInfectedByFrozenZombie = false;
                    unit_manager.ChangeColor(""); // Возвращаем стандартный цвет
                    break;

                // Отключаем вирус "Гигантского Паука"
                case "Spider Virus":
                    unit_manager.IsInfectedBySpider = false;
                    unit_manager.ChangeColor(""); // Возвращаем стандартный цвет
                    break;
            }
        }
    }

    /// <summary>
    /// НАВСЕГДА меняем скорость атаки/передвижения
    /// </summary>
    /// <param name="code">Что делаем: Slow MS, Boost MS, Slow AS, Boost AS</param>
    /// <param name="value">На сколько % (форма записи: 50% = 0.5)</param>
    public IEnumerator ChangeSpeed(string code, float value)
    {
        StartCoroutine(ChangeSpeed(code, 0, value));
        yield return null;
    }

    /// <summary>
    /// ВРЕМЕННО меняем скорость атаки/передвижения
    /// </summary>
    /// <param name="code">Что делаем: Slow MS, Boost MS, Slow AS, Boost AS</param>
    /// <param name="time">Длительность эффекта</param>
    /// <param name="value">На сколько % (форма записи: 50% = 0.5)</param>
    public IEnumerator ChangeSpeed(string code, float time, float value)
    {
        // Включаем эффекты
        switch (code)
        {
            case "Slow MS": ms_slow += value; break; // Добавляем замедление скорости передвижения
            case "Boost MS": ms_bonus += value; break; // Добавляем ускорение скорости передвижения
            case "Slow AS": as_slow += value; break; // Добавляем замедление скорости атаки
            case "Boost AS": as_bonus += value; break; // Добавляем ускорение скорости атаки
        }

        CalcSpeed(); // Пересчитываем скорость атаки/передвижения

        yield return new WaitForSeconds(time);

        // Если эффект НЕ бесконечный (в обратном случае надо вручную отключать эффекты)
        if (time > 0)
        {
            // Отключаем эффекты
            switch (code)
            {
                case "Slow MS": ms_slow -= value; break; // Убавляем замедление скорости передвижения
                case "Boost MS": ms_bonus -= value; break; // Убавляем ускорение скорости передвижения
                case "Slow AS": as_slow -= value; break; // Убавляем замедление скорости атаки
                case "Boost AS": as_bonus -= value; break; // Убавляем ускорение скорости атаки
            }
        }

        CalcSpeed(); // Пересчитываем скорость атаки/передвижения
    }

    // Таймер кулдауна атаки
    public IEnumerator AttackCD(float time)
    {
        yield return new WaitForSeconds(time);
        attackCD = false;
    }
    #endregion

    private void CalcSpeed()
    {
        // Ограничиваем максимальное замедление скорости передвижения
        if (ms_slow > 0.9f) f_ms_slow = 0.9f;
        else f_ms_slow = ms_slow;

        // Ограничиваем максимальное замедление скорости атаки
        if (as_slow > 0.9f) f_as_slow = 0.9f;
        else f_as_slow = as_slow;

        unit_manager.MoveSpeed = unit_manager.BasicMS * (1 + ms_bonus - f_ms_slow);
        unit_manager.AttackSpeed = unit_manager.BasicAS * (1 - as_bonus + f_as_slow);
    }

    // Возвращаем случайное число (шанс срабатывания перков) RandomChance() < *ваш_шанс*
    private int RandomChance()
    {
        return Random.Range(0, 99);
    }
}
