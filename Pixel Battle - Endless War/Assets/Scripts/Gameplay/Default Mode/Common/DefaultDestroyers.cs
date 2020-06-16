using UnityEngine;

public class DefaultDestroyers : MonoBehaviour
{
    public DefaultGameController game_controller;

    private UnitManager unit_manager;
    private AudioClip hit_sfx;
    private int damage;
    private bool isAlly;

    private void Awake()
    {
        if (name == "Ally Destroyer")
        {
            isAlly = true;
            hit_sfx = GetComponent<AudioSource>().clip;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Если вражеская база
        if (!isAlly && collision.CompareTag("Ally"))
        {
            unit_manager = collision.GetComponent<UnitManager>(); // Кэшируем скрипт

            if (unit_manager.UnitClass != "Undead")
            {
                switch (unit_manager.UnitClass)
                {
                    case "Warrior": damage = 8; break;

                    case "Paladin":
                    case "Tinker":
                    case "Knight":
                    case "Ninja": damage = 15; break;

                    case "QueenOfArchers":
                    case"Necromancer": damage = 20; break;
                    
                    case "Berserk": damage = 25; break;
                    default: damage = 10; break;
                }

                game_controller.DoDamage(damage, false);
            }

            unit_manager.Destroy(); // Уничтожаем юнита без записи статистики
        }

        // Если союзная база
        else if (isAlly && collision.CompareTag("Enemy"))
        {
            unit_manager = collision.GetComponent<UnitManager>(); // Кэшируем скрипт

            if (unit_manager.UnitClass != "Spiderling")
            {
                // Звук "удара" по базе
                if (AudioManager.instance.IsOn())
                    GetComponent<AudioSource>().PlayOneShot(hit_sfx);

                game_controller.DoDamage(10, true);
            }

            unit_manager.Destroy(); // Уничтожаем юнита без записи статистики
        }
    }
}
