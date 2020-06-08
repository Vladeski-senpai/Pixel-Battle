using UnityEngine;
using UnityEngine.UI;

public class SettingsLanguageButton : MonoBehaviour
{
    public Sprite[] sprites;
    public SettingsLanguageButton other_button;

    private Image image;

    private string language;

    private void Awake()
    {
        image = GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
        CheckButtonCondition();
    }

    private void TaskOnClick()
    {
        switch (name.Substring(3))
        {
            case "English": if (language != "en") GlobalData.SetString("Language", "en"); break; // Меняем на Английский
            case "Russian": if (language != "ru") GlobalData.SetString("Language", "ru"); break; // Меняем на Русский
        }

        CheckButtonCondition();
        other_button.CheckButtonCondition(); // Обновляем другую кнопку (языка)
        ScenesManager.scenes_manager.LoadLevel(0); // Перезагружаем сцену меню
    }

    public void CheckButtonCondition()
    {
        int id = 0; // Если 0, то картинка "выключена"
        language = GlobalData.GetString("Language");

        switch (name.Substring(3))
        {
            case "English": if (language == "en") id = 1; break; // Если язык Английский
            case "Russian": if (language == "ru") id = 1; break; // Если язык Русский
        }

        image.sprite = sprites[id];
    }
}
