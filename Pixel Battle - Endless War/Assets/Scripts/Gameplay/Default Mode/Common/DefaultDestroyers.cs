using UnityEngine;

public class DefaultDestroyers : MonoBehaviour
{
    public DefaultGameController game_controller;

    private UnitManager unit_manager;
    private AudioSource audio_s;
    private int damage;
    private bool isAlly = true;

    private void Awake()
    {
        if (name == "Enemy Destroyer")
            isAlly = false;

        // Если звук включён
        if (GlobalData.GetInt("Sound") != 0)
            audio_s = GetComponent<AudioSource>();
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
                    case "Warrior": damage = 7; break;

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
        }

        // Если союзная база
        else if (isAlly && collision.CompareTag("Enemy"))
        {
            unit_manager = collision.GetComponent<UnitManager>(); // Кэшируем скрипт

            if (unit_manager.UnitClass != "Spiderling")
                game_controller.DoDamage(10, true);

            // Если звук "включён"
            if (audio_s != null)
                audio_s.Play();
        }
    }
}
