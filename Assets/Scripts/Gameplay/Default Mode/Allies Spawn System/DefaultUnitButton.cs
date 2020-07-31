using UnityEngine;
using UnityEngine.UI;

public class DefaultUnitButton : MonoBehaviour
{
    [HideInInspector]
    public GameObject outline; // Обводка

    private DefaultGameController game_controller;
    private ButtonClickAnimation btn_animation; // Для анимации
    private AudioManager audio_manager;
    private AudioSource audio_s;
    private Button button;
    private Image unit_avatar; // Картинка с изображением юнита
    private Text txt_unit_cost; // Текст с текущим кол-вом маны для юнита

    private string unit_name; // Имя юнита в данном "слоте"
    private int unit_cost; // Стоимость текущего юнита в мане
    private bool 
        isOn, // Включён ли звук
        isActive = true;

    private void Awake()
    {
        outline = transform.GetChild(0).gameObject;
        unit_avatar = transform.GetChild(1).GetComponent<Image>();
        txt_unit_cost = transform.GetChild(2).GetComponent<Text>();
        btn_animation = GetComponent<ButtonClickAnimation>();
        audio_s = GetComponent<AudioSource>();
        button = GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
        unit_name = CheckSlot(); // Берём имя юнита в текущем "слоте"
        unit_cost = GetCost(); // Берём его стоимость в мане
        txt_unit_cost.text = unit_cost.ToString(); // Меняем текст стоимости юнита
        btn_animation.size = 0.31f; // Меняем размер увеличенной кнопки
        btn_animation.anim_speed = 0.06f; // Меняем скорость анимации

        // Выключаем кнопку если слот пустой
        if (unit_name == "" || unit_name == null)
        {
            txt_unit_cost.text = "";
            button.interactable = false;
            enabled = false; // Отключаем скрипт
        }
    }

    private void Start()
    {
        game_controller = DefaultGameController.default_controller;
        audio_manager = AudioManager.instance;
        isOn = audio_manager.IsOn(); // Проверяем включён ли звук

        LoadImage(); // Меняем аватар кнопки
        ButtonCondition(); // Проверяем состояние кнопки
    }

    private void Update()
    {
        if (game_controller.CurrentMana >= unit_cost)
        {
            if (!isActive)
            {
                if (isOn) PlaySound(0.7f, 0.6f); // Воспроизводим звук когда кнопка становится активной
                ButtonCondition(); // Активируем кнопку
            }
        }
        else if (isActive) ButtonCondition(); // Деактивируем кнопку
    }

    private void TaskOnClick()
    {
        if (isActive)
        {
            // Если звук "включён"
            if (isOn) PlaySound(1, 1);

            // Записываем выбранного юнита с проверкой не выбран ли он уже
            if (game_controller.ChoosedUnit != unit_name)
                game_controller.PrepareAllyUnit(unit_name, unit_cost, this);

            // Если обводка выключена, включаем её
            if (!outline.activeSelf)
            {
                outline.SetActive(true);
                game_controller.SpawnButtonsCondition(true); // Включаем кнопки спавна (метод выше не включит кнопки спавна того же юнита)
            }
            else
            {
                outline.SetActive(false);
                game_controller.SpawnButtonsCondition(false);
            }
        }
    }

    // Проверяем можно ли активировать/деактивировать кнопку
    private void ButtonCondition()
    {
        isActive = !isActive;

        // Если деактивируем кнопку
        if (!isActive)
        {
            ChangeColor(120, 120, 120, 255); // Меняем цвет на тёмный
            outline.SetActive(isActive); // Отключаем обводку
        }
        // Если активируем кнопку
        else
        {
            ChangeColor(255, 255, 255, 255);
            PlayAnim(); // Включаем анимацию
        }

        button.interactable = isActive; // Меняем состояние кнопки
    }

    // Устанавливаем картинку и размер выбранного юнита
    private void LoadImage()
    {
        RectTransform rect_transform = unit_avatar.GetComponent<RectTransform>();

        float
            scale_x = 1,
            scale_y = 1,
            pos_x = 0,
            pos_y = 17.5f;

        int
            size_w = 90, // Ширина картинки
            size_h = 70, // Высота картинки
            i = 0;

        // Меняем под каждого юнита индивидуально
        switch (unit_name)
        {
            case "Warrior":
                size_w = 70;
                size_h = 80;
                pos_y = 15.6f;
                i = 0;
                break;

            case "Archer":
                pos_x = -7f;
                pos_y = 11f;
                i = 1;
                break;

            case "Thief":
                size_w = 100;
                scale_x = 0.95f;
                scale_y = 0.95f;
                pos_y = 10;
                i = 2;
                break;

            case "Knight":
                size_w = 70;
                size_h = 80;
                pos_y = 16.5f;
                i = 3;
                break;

            case "Ninja":
                size_w = 100;
                pos_x = -12;
                pos_y = 10.9f;
                i = 4;
                break;

            case "Paladin":
                size_w = 70;
                scale_x = 1.1f;
                scale_y = 1.1f;
                pos_y = 14;
                i = 5;
                break;

            case "Necromancer":
                size_w = 100;
                scale_x = 1.05f;
                scale_y = 1.05f;
                pos_x = -4.5f;
                pos_y = 13.5f;
                i = 6;
                break;

            case "Elf Maiden":
                size_w = 100;
                size_h = 100;
                scale_x = 0.9f;
                scale_y = 0.9f;
                pos_x = -4.7f;
                pos_y = 11.4f;
                i = 7;
                break;

            case "Gunslinger":
                size_w = 100;
                size_h = 80;
                pos_y = 15.7f;
                i = 8;
                break;

            case "Dark Knight":
                size_w = 140;
                size_h = 100;
                scale_x = 0.8f;
                scale_y = 0.8f;
                pos_y = 15.4f;
                i = 9;
                break;

            case "Steel Bat":
                size_h = 80;
                pos_y = 15.6f;
                i = 10;
                break;

            case "Tinker":
                size_h = 80;
                pos_y = 15.9f;
                i = 11;
                break;

            case "Shieldman":
                size_h = 100;
                scale_x = 0.95f;
                scale_y = 0.95f;
                pos_x = -5.8f;
                pos_y = 22.45f;
                i = 12;
                break;

            default:
                size_w = 0;
                size_h = 0;
                break;
        }

        rect_transform.sizeDelta = new Vector2(size_w, size_h);
        rect_transform.localScale = new Vector2(scale_x, scale_y);
        rect_transform.localPosition = new Vector2(pos_x, pos_y);

        unit_avatar.sprite = game_controller.units_avatars[i];
    }

    // Меняем цвет элементов
    private void ChangeColor(byte r, byte g, byte b, byte a)
    {
        unit_avatar.color = new Color32(r, g, b, a);
        txt_unit_cost.color = new Color32(r, g, b, a);
    }

    // Берём записанного юнита текущем "слоте"
    private string CheckSlot()
    {
        return GlobalData.GetString(name);
    }

    // Берём стоимость юнита в мане
    private int GetCost()
    {
        switch (unit_name)
        {
            case "Warrior":
                return 8;

            case "Thief":
                return 10;

            case "Archer":
                return 12;

            case "Knight":
            case "Ninja":
                return 15;

            case "Paladin":
            case "Gunslinger":
            case "Steel Bat":
                return 20;

            case "Elf Maiden":
            case "Shieldman":
                return 18;

            case "Necromancer":
            case "Tinker":
                return 22;

            case "Dark Knight":
                return 26;

            default:
                return 0;
        }
    }

    // Воспроизводим анимацию увеличения кнопки
    public void PlayAnim()
    {
        btn_animation.isAnimated = true;
    }

    // Воспроизводим звук
    private void PlaySound(float volume, float pitch)
    {
        audio_s.volume = volume;
        audio_s.pitch = pitch;
        audio_s.Play();
    }

}
