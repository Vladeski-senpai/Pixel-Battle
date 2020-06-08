using UnityEngine;

public class GuildUnitInfo : MonoBehaviour
{
    private GuildManager guild_manager;
    private string language;

    private void Start()
    {
        guild_manager = GuildManager.guild_manager;
        language = GlobalData.GetString("Language");
    }

    // Возвращаем историю юнита
    public string GetUnitHistory()
    {
        switch (guild_manager.ChoosedUnit)
        {
            case "Warrior":
                // Если язык русский
                if (language == "ru")
                    return "Бравый воин, сражающийся на благо людей.  Имеет хорошие показатели здоровья и урона.";
                // Если язык английский
                else
                    return "A brave warrior who fights for the good of people.  Great health and damage.";

            case "Archer":
                // Если язык русский
                if (language == "ru")
                    return "Опытный лучник, способный пронзить яблоко на вашей голове.  Довольно мало здоровья, средний урон.";
                // Если язык английский
                else
                    return "An experienced Archer who can pierce the apple on your head.  Low health, medium damage.";

            case "Thief":
                // Если язык русский
                if (language == "ru")
                    return "Ловкий воришка, умело обращающийся с кинжалами.  Довольно мало здоровья и урона, компенсирует уклонениями и быстрыми атаками.";
                // Если язык английский
                else
                    return "A clever thief, skilled with daggers.  Low health and damage, compensates with evasions and quick attacks.";

            case "Knight":
                // Если язык русский
                if (language == "ru")
                    return "Отважный и сильный рыцарь, которому не страшен ни один враг.  Много здоровья и урона, тяжёлая броня поглащает часть урона.";
                // Если язык английский
                else
                    return "A brave and strong knight who is unafraid of any enemy.  High health and damage, heavy armor absorbs some of the damage.";

            case "Ninja":
                // Если язык русский
                if (language == "ru")
                    return "Настоящий ниндзя, профессионал своего дела, ещё никто не уходил от него живым...  " +
                        "Хорошие показатели здоровья и урона, метает сюрикены в начале боя.";
                // Если язык английский
                else
                    return "A real ninja, a professional in his field, no one walked alive from him...  " +
                        "Great health and damage, throws few shurikens at the beginning of the battle.";

            case "Paladin":
                // Если язык русский
                if (language == "ru")
                    return "Паладин, он же Святой рыцарь, владеющий утерянной техникой магии исцеления.  " +
                        "Много здоровья и урона, но медленные атаки, броня поглащает часть урона.";
                // Если язык английский
                else
                    return "Paladin, aka the Holy knight, who owns the lost technique of healing magic.  " +
                        "High health and damage, but slow attacks, heavy armor absorbs some of the damage.";

            case "Elf Maiden":
                // Если язык русский
                if (language == "ru")
                    return "Издавна эльфы отличаются исключительной ловкостью и отличными навыками стрельбы из лука.  " +
                        "Хороший запас здоровья и урона, высокая скорость стрельбы.";
                // Если язык английский
                else
                    return "Elves have long been distinguished by exceptional dexterity and excellent archery skills.  " +
                        "Great health and damage, high attack speed.";

            case "Necromancer":
                // Если язык русский
                if (language == "ru")
                    return "Тёмный маг, возвращающий тела павших воинов к жизни, чтобы те сражались вместо него.  " +
                        "Средний запас здоровья и небольшой урон, призывает сразу две Нежити.";
                // Если язык английский
                else
                    return "A dark mage who brings the bodies of fallen warriors back to life so that they fight instead of him.  " +
                        "Meduim health and damage, summons two Undead at once.";

            case "Gunslinger":
                // Если язык русский
                if (language == "ru")
                    return "В поисках наживы и лучшей жизни, бывший преступник решил присоединиться к борьбе с нечистью.  " +
                        "Средний запас здоровья, но неплохой урон, однако скорость стрельбы оставляет желать лучшего.";
                // Если язык английский
                else
                    return "In search of profit and a worthier life, the former criminal decided to join the fight against evil creatures.  " +
                        "Medium health, great damage, but attack speed a bit slow.";

            case "Dark Knight":
                // Если язык русский
                if (language == "ru")
                    return "Закалённый боями, пропитанный запретной магией, тёмный рыцарь выходит на очередное поле битвы...  " +
                        "Огромный запас здоровья и урона. Зачарованная броня поглащает часть урона.";
                // Если язык английский
                else
                    return "Battle-hardened, imbued with forbidden magic, the dark knight enters the following battlefield... " +
                        "Huge health and damage. Enchanted armor absorbs some of the damage.";

            case "Steel Bat":
                // Если язык русский
                if (language == "ru")
                    return "Хулиган с золотым сердцем, поклявшийся отомстить за смерть своей сестры, вступает на поле битвы!  " +
                        "Большой запас здоровья, средний урон. Благодаря Боевому духу становится сильнее с каждым полученным уроном.";
                else
                    return "A bully with a golden heart, sworn to avenge the death of his sister, enters the battlefield!  " +
                        "High health, medium damage. Thanks to the Fighting spirit, he becomes stronger with each damage received.";

            case "Tinker":
                // Если язык русский
                if (language == "ru")
                    return "Безумный изобретатель.";
                else
                    return "Crazy developer.";

            default: return "";
        }
    }

    // Возвращаем список перков
    public string GetUnitPerks()
    {
        switch (guild_manager.ChoosedUnit)
        {
            case "Warrior":
                // Если язык русский
                if (language == "ru")
                    return "Перки:\n" + "- Отражение  Снаряда\n" + "- Парирование  Атаки";
                // Если язык английский
                else
                    return "Perks:\n" + "- Shell  Block\n" + "- Parry  Stun";

            case "Arhcer":
                // Если язык русский
                if (language == "ru")
                    return "Перки:\n" + "- Критическое  Попадание";
                // Если язык английский
                else
                    return "Perks:\n" + "- Critical  Hit";

            case "Thief":
                // Если язык русский
                if (language == "ru")
                    return "Перки:\n" + "- Глубокий  Порез\n" + "- Уклонение";
                // Если язык английский
                else
                    return "Perks:\n" + "- Deep  Cut\n" + "- Evasion";

            case "Knight":
                // Если язык русский
                if (language == "ru")
                    return "Перки:\n" + "- Отражение  Снаряда\n" + "- Парирование  Атаки\n" + "- Поглощение  Урона";
                // Если язык английский
                else
                    return "Perks:\n" + "- Shell  Block\n" + "- Parry  Stun\n" + "- Damage  Reduce";

            case "Ninja":
                // Если язык русский
                if (language == "ru")
                    return "Перки:\n" + "- Глубокий  Порез\n" + "- Уклонение";
                // Если язык английский
                else
                    return "Perks:\n" + "- Deep  Cut\n" + "- Evasion";

            case "Paladin":
                // Если язык русский
                if (language == "ru")
                    return "Перки:\n" + "- Отражение  Снаряда\n" + "- Парирование  Атаки\n" + "- Поглощение  Урона\n" + "- Лечение";
                // Если язык английский
                else
                    return "Perks:\n" + "- Shell  Block\n" + "- Parry  Stun\n" + "- Damage  Reduce\n" + "- Heal";

            case "Elf Maiden":
                // Если язык русский
                if (language == "ru")
                    return "Перки:\n" + "- Критическое  Попадание\n" + "- Уклонение";
                // Если язык английский
                else
                    return "Perks:\n" + "- Critical  Hit\n" + "- Evasion";

            case "Necromancer":
                // Если язык русский
                if (language == "ru")
                    return "Перки:\n" + "- Поглощение  Урона\n" + "- Парирование  Атаки";
                // Если язык английский
                else
                    return "Perks:\n" + "- Damage  Reduce\n" + "- Parry  Stun";

            case "Gunslinger":
                // Если язык русский
                if (language == "ru")
                    return "Перки:\n" + "- Критическое  Попадание\n" + "- Тяжёлая  Пуля";
                // Если язык английский
                else
                    return "Perks:\n" + "- Critical  Hit\n" + "- Heavy  Bullet";

            case "Dark Knight":
                // Если язык русский
                if (language == "ru")
                    return "Перки:\n" + "- Отражение  Снаряда\n" + "- Парирование  Атаки\n" + "- Поглощение  Урона";
                // Если язык английский
                else
                    return "Perks:\n" + "- Shell  Block\n" + "- Parry  Stun\n" + "- Damage  Reduce";

            case "Steel Bat":
                // Если язык русский
                if (language == "ru")
                    return "Перки:\n" + "- Боевой  Дух\n" + "- Отражение  Снаряда\n" + "- Парирование  Атаки\n" + "- Поглощение  Урона";
                // Если язык английский
                else
                    return "Perks:\n" + "- Fighting  Spirit\n" + "- Shell  Block\n" + "- Parry  Stun\n" + "- Damage  Reduce";

            case "Tinker":
                // Если язык русский
                if (language == "ru")
                    return "Перки:\n" + "- Отражение  Снаряда\n" + "- Парирование  Атаки";
                // Если язык английский
                else
                    return "Perks:\n" + "- Shell  Block\n" + "- Parry  Stun";

            default: return "";
        }
    }
}
