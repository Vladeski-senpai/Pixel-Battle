using UnityEngine;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    private AudioSource audio_s;
    private bool isOn; // Включён ли звук

    private void Awake()
    {
        audio_s = GetComponent<AudioSource>();
        GetComponent<Button>().onClick.AddListener(TaskOnClick);

        if (GlobalData.GetInt("Sound") != 0) isOn = true;
    }

    private void TaskOnClick()
    {
        // Звук нажатия
        if (isOn) audio_s.Play();

        switch (name.Substring(3))
        {
            // Загружаем сцену с Главным меню
            case "Menu": ScenesManager.scenes_manager.LoadLevel(0); break;

            // Загружаем сцену с Игрой
            case "Play": ScenesManager.scenes_manager.LoadLevel(1); break;

            // Загружаем сцену с Инвентарём
            case "Inventory": ScenesManager.scenes_manager.LoadLevel(2); break;

            // Загружаем сцену с Гильдией
            case "Guild": ScenesManager.scenes_manager.LoadLevel(3); break;

            // Загружаем сцену со Статистикой
            case "Stats": ScenesManager.scenes_manager.LoadLevel(4); break;

            // Загружаем сцену с Пожертвованиями
            case "Donation": ScenesManager.scenes_manager.LoadLevel(5); break;
        }
    }
}
