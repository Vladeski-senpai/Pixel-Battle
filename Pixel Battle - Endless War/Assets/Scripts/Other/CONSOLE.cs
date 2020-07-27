#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.UI;

public class CONSOLE : MonoBehaviour
{
    public Scrollbar scrollbar;
    public GameObject console_canvas;

    public Text 
        txt_info, // Текст консоли
        txt_command; // Текстовое поле команды

    private string[] commands_list = new string[] { "help", "stop", "play", "gold", "gems", "delete", "poison", 
        "bleed", "kill", "time", "damage", "setlvl", "unlock", "restart", "mana", "slow", "superslow", "load" };

    private string
        prev_command = "", // Предыдущая команда
        command = ""; // Команда записываемая в консоль

    private byte transparency; // Прозрачность команды (для анимации)
    private bool 
        active,
        animCD; // Кулдаун анимации

    private void Update()
    {
        // Включаем/выключаем консоль
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            active = !console_canvas.activeSelf;
            console_canvas.SetActive(active);
        }

        if (console_canvas.activeSelf)
        {
            scrollbar.value = 0; // Скроллим в самый вниз

            // Печатаем текст
            if (Input.anyKeyDown)
            {
                foreach (char c in Input.inputString)
                {
                    if (c != '`' && c != '\b' && c != 'ё' && c != '\r' && c != '\n')
                    {
                        command += c;
                        txt_command.text = command;
                    }
                }
            }

            // Активируем команду
            if (Input.GetKeyDown(KeyCode.Return))
            {
                CheckCommand();
            }

            // Удаляем команду
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                ClearCommand();
            }

            // Удаляем 1 символ из команды
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                RemoveChar();
            }

            // Записываем последнюю команду
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                command = prev_command;
                txt_command.text = command;
            }

            // Анимация мигания
            if (command == "" && !animCD)
            {
                animCD = true;
                StartCoroutine(Timer());

                if (transparency == 0)
                    transparency = 255;
                else
                    transparency = 0;

                txt_command.text = "_";
                txt_command.color = new Color32(255, 112, 122, transparency);
            }
            else if (command != "") txt_command.color = new Color32(255, 112, 122, 255);
        }
    }

    // Проверяем активируемую команду
    private void CheckCommand()
    {
        if (command != "")
        {
            string command_code = "error";

            // Проверяем есть ли такая команда
            for (int i = 0; i < commands_list.Length; i++)
            {
                if (command.Contains(commands_list[i]))
                    command_code = commands_list[i];
            }

            // Если команда найдена
            if (command_code != "error")
                WriteDownCommand(command_code); // Записываем команду в консоль и её функцию
            else
                txt_info.text += "\n\n" + "Error! Command \"" + command + "\" does not exist!";

            command = "";
            txt_command.text = command;
        }
    }

    // Записываем команду в консоль и её функцию
    private void WriteDownCommand(string command_code)
    {
        prev_command = command; // Сохраняем введённую команду
        txt_info.text += "\n\n" + ">" + command + "\n";

        string text = "";
        switch (command_code)
        {
            case "help": Help(); break; // Выводим список всех команд
            case "time": text = "Time fron scene loading:  " + (int)Time.timeSinceLevelLoad + "s."; break; // Время с начала загрузки уровня

            // Останавливаем время
            case "stop":
                Time.timeScale = 0;
                text = "Time was stopped.";
                break;

            // Возобновляем время
            case "play":
                Time.timeScale = 1;
                text = "Time was resumed.";
                break;

            // Замедляем время
            case "slow":
                Time.timeScale = 0.1f;
                text = "Time was slowed by 90%";
                break;

            // Сильное замедление
            case "superslow":
                Time.timeScale = 0.03f;
                text = "Time was slowed by 97%";
                break;

            // Устанавливаем золото
            case "gold":
                GlobalData.SetInt("Gold", System.Convert.ToInt32(command.Substring(5)));
                text = System.Convert.ToInt32(command.Substring(5)) + " gold was set.";
                break;

            // Устанавливаем гемы
            case "gems":
                GlobalData.SetInt("Gems", System.Convert.ToInt32(command.Substring(5)));
                text = System.Convert.ToInt32(command.Substring(5)) + " gems was set.";
                break;

            // Удаляем все сохранённые данные
            case "delete":
                PlayerPrefs.DeleteAll();
                text = "All data was deleted.";
                break;

            // Отравляем юнитов
            case "poison":
                if (command.Contains("allies"))
                {
                    AddEffectOnUnit("Ally", "Poisoned", 5, 0.1f);
                    text = "All allies was poisoned.";
                }
                else if (command.Contains("enemies"))
                {
                    AddEffectOnUnit("Enemy", "Poisoned", 5, 0.1f);
                    text = "All enemies was poisoned.";
                }
                else if (command.Contains("all"))
                {
                    AddEffectOnUnit("All", "Poisoned", 5, 0.1f);
                    text = "All units was poisoned.";
                }
                else text = "Wrong command syntax, write type of target: allies, enemies, all.";
                break;

            // Вызываем кровотечение у юнитов
            case "bleed":
                if (command.Contains("allies"))
                {
                    AddEffectOnUnit("Ally", "Bleeding", 4, 0.1f);
                    text = "All allies started bleed.";
                }
                else if (command.Contains("enemies"))
                {
                    AddEffectOnUnit("Enemy", "Bleeding", 4, 0.1f);
                    text = "All enemies started bleed.";
                }
                else if (command.Contains("all"))
                {
                    AddEffectOnUnit("All", "Bleeding", 4, 0.1f);
                    text = "All units started bleed.";
                }
                else text = "Wrong command syntax, write type of target: allies, enemies, all.";
                break;

            // Убиваем юнитов
            case "kill":
                if (command.Contains("allies"))
                {
                    KillUnits("Ally");
                    text = "All allies was killed.";
                }
                else if (command.Contains("enemies"))
                {
                    KillUnits("Enemy");
                    text = "All enemies was killed.";
                }
                else if (command.Contains("all"))
                {
                    KillUnits("All");
                    text = "All units was killed.";
                }
                else text = "Wrong command syntax, write type of target: allies, enemies, all.";
                break;

            // Наносим урон базам
            case "damage":
                if (command.Contains("ally"))
                {
                    DefaultGameController.default_controller.DoDamage(System.Convert.ToInt32(command.Substring(12)), true);
                    text = "Ally base was hitted by " + command.Substring(12) + " damage";
                }
                else if (command.Contains("enemy"))
                {
                    DefaultGameController.default_controller.DoDamage(System.Convert.ToInt32(command.Substring(13)), false);
                    text = "Enemy base was hitted by " + command.Substring(13) + " damage";
                }
                else text = "Wrong command syntax, write type of target: ally, enemy.";
                break;

            // Устанавливаем уровень игрока
            case "setlvl":
                GlobalData.SetInt("PlayerLvl", System.Convert.ToInt32(command.Substring(7)));
                text = "Player level set to " + command.Substring(7) + ".";
                break;

            // Разблокируем юнита
            case "unlock":
                GlobalData.SetInt(command.Substring(7), 1);
                text = "Unit \"" + command.Substring(7) + "\" was unlocked."; 
                break;

            // Перезагружаем сцену
            case "restart":
                ScenesManager.scenes_manager.LoadLevel(1);
                text = "Scene is restarting...";
                break;

            // Бустим ману
            case "mana":
                DefaultGameController.default_controller.AddMana();
                text = "Mana was added.";
                break;

            case "load":
                if (command.Contains("menu"))
                {
                    ScenesManager.scenes_manager.LoadLevel(0);
                    text = "Scene menu is loading...";
                }
                else text = "Wrong command syntax, write name of scene: menu.";
                break;
        }

        if (text != "") txt_info.text += text; // Записываем информацию о команде если она есть
    }

    #region Commands List
    // Возвращаем список всех команд
    private void Help()
    {
        txt_info.text += "List of commands:  ";

        // Пишем список всех команд
        for (int i = 0; i < commands_list.Length; i++)
        {
            // Если последняя команда, пишем точку
            if (commands_list.Length - 1 == i)
                txt_info.text += commands_list[i] + ".";
            else
                txt_info.text += commands_list[i] + ", ";
        }
    }

    // Добавляем различные эффекты на цель (Ally, Enemy, All)
    private void AddEffectOnUnit(string target, string effect_name, float duration, float value)
    {
        if (target != "All")
        {
            GameObject[] units = GameObject.FindGameObjectsWithTag(target);
            foreach (GameObject unit in units)
            {
                // Если юнит жив
                if (unit != null && !unit.GetComponent<UnitManager>().IsDead)
                    unit.GetComponent<UnitManager>().AddEffect(effect_name, duration, value);
            }
        }
        else
        {
            GameObject[] units = GameObject.FindGameObjectsWithTag("Ally");
            foreach (GameObject unit in units)
            {
                // Если юнит жив
                if (unit != null && !unit.GetComponent<UnitManager>().IsDead)
                    unit.GetComponent<UnitManager>().AddEffect(effect_name, duration, value);
            }

            units = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject unit in units)
            {
                // Если юнит жив
                if (unit != null && !unit.GetComponent<UnitManager>().IsDead)
                    unit.GetComponent<UnitManager>().AddEffect(effect_name, duration, value);
            }
        }
    }

    // Убиваем юнитов
    private void KillUnits(string target)
    {
        if (target != "All")
        {
            GameObject[] units = GameObject.FindGameObjectsWithTag(target);
            foreach (GameObject unit in units)
            {
                // Если юнит жив
                if (unit != null && !unit.GetComponent<UnitManager>().IsDead)
                    unit.GetComponent<UnitManager>().Death();
            }
        }
        else
        {
            GameObject[] units = GameObject.FindGameObjectsWithTag("Ally");
            foreach (GameObject unit in units)
            {
                // Если юнит жив
                if (unit != null && !unit.GetComponent<UnitManager>().IsDead)
                    unit.GetComponent<UnitManager>().Death();
            }

            units = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject unit in units)
            {
                // Если юнит жив
                if (unit != null && !unit.GetComponent<UnitManager>().IsDead)
                    unit.GetComponent<UnitManager>().Death();
            }
        }
    }
    #endregion

    #region Work with text
    // Удаляем букву из команды
    private void RemoveChar()
    {
        if (command.Length != 0)
        {
            command = command.Remove(command.Length - 1);
            txt_command.text = command;
        }
    }

    // Удаляем команду
    private void ClearCommand()
    {
        command = "";
        txt_command.text = command;
    }

    // Кулдаун анимации
    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.5f);
        animCD = false;
    }
    #endregion
}
#endif