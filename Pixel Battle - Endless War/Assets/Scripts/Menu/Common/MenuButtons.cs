using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    private Text txt; // Текст кнопки

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    private void TaskOnClick()
    {
        switch (name.Substring(3))
        {
            // Загружаем сцену с Главным меню
            case "Menu":
                ScenesManager.scenes_manager.LoadLevel(0);
                break;

            // Загружаем сцену с Игрой
            case "Play":
                ScenesManager.scenes_manager.LoadLevel(1);
                break;

            // Загружаем сцену с Инвентарём
            case "Inventory":
                ScenesManager.scenes_manager.LoadLevel(2);
                break;

            // Загружаем сцену с Гильдией
            case "Guild":
                ScenesManager.scenes_manager.LoadLevel(3);
                break;
        }
    }
}
