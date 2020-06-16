using UnityEngine;
using UnityEngine.UI;

public class InventoryPlayerSlots : MonoBehaviour
{
    private ButtonClickAnimation btn_animation;
    private InventoryManager inventory_manager;
    private RectTransform rect_transform;
    private AudioSource audio_s;
    private Image avatar;

    private string choosed_unit;
    private bool isOn; // Включён ли звук

    private void Awake()
    {
        audio_s = GetComponent<AudioSource>();
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
        avatar = transform.GetChild(0).GetComponent<Image>();
        rect_transform = avatar.GetComponent<RectTransform>();
        choosed_unit = CheckSlot(); // Записываем юнита в текущем слоте
        btn_animation = GetComponent<ButtonClickAnimation>();
        btn_animation.size = 0.86f; // Меняем размер увеличенной кнопки
        btn_animation.anim_speed = 0.14f; // Меняем скорость анимации
        
        if (GlobalData.GetInt("Sound") != 0) isOn = true;
    }

    private void Start()
    {
        inventory_manager = InventoryManager.inventory_manager;
        ResizeAvatar();
    }

    private void TaskOnClick()
    {
        // Если есть выбранный юнит
        if (inventory_manager.ChoosedUnit != null)
        {
            // Если юнит не занят
            if (!CheckSlots())
            {
                choosed_unit = inventory_manager.ChoosedUnit;
                SaveSlot();
                ResizeAvatar();
                inventory_manager.ResetChoosedUnit();
            }

            inventory_manager.PlayAnim(); // Если юнит занят, анимируем кнопку и меняем цвет обводки

            // Звук нажатия
            if (isOn)
            {
                audio_s.pitch = 0.8f;
                audio_s.Play();
            }
        }

        // Если нету выбранного юнита, очищаем слот
        else
        {
            // Очищаем только, если есть хотя бы один занятый слот
            if (CheckEmptySlots() > 1)
            {
                choosed_unit = "";
                SaveSlot();
                ResizeAvatar();
            }

            // Звук нажатия
            if (isOn)
            {
                audio_s.pitch = 0.5f;
                audio_s.Play();
            }
        }
    }

    // Проверяем есть ли текущий юнит в других слотах
    private bool CheckSlots()
    {
        for (int i = 0; i < 6; i++)
        {
            if (inventory_manager.ChoosedUnit == GlobalData.GetString("Slot" + i))
                return true;
        }

        return false; // Если совпадения не найдены
    }

    // Проверяем есть ли пустые слоты
    private int CheckEmptySlots()
    {
        int value = 0;

        for (int i = 0; i < 6; i++)
        {
            if (GlobalData.GetString("Slot" + i) != "")
                value++;
        }

        return value; // Если все слоты пустые
    }

    // Меняем размер и положение аватара
    private void ResizeAvatar()
    {
        int i = 0;
        float
            size_w = 7,
            size_h = 8,
            scale_x = 1,
            scale_y = 1,
            pos_x = 0,
            pos_y = 0;

        switch (choosed_unit)
        {
            case "Warrior":
                pos_y = 0.47f;
                i = 0;
                break;

            case "Archer":
                size_w = 9;
                size_h = 7;
                pos_x = -0.89f;
                i = 1;
                break;

            case "Thief":
                size_w = 10;
                size_h = 7;
                pos_x = -0.43f;
                i = 2;
                break;

            case "Knight":
                scale_x = 1.1f;
                scale_y = 1.1f;
                pos_y = 0.79f;
                i = 3;
                break;

            case "Ninja":
                size_w = 10;
                size_h = 7;
                pos_x = -1.44f;
                i = 4;
                break;

            case "Paladin":
                size_h = 7;
                scale_x = 1.05f;
                scale_y = 1.05f;
                pos_y = 0.17f;
                i = 5;
                break;

            case "Necromancer":
                size_w = 9;
                size_h = 7;
                pos_x = -0.55f;
                i = 6;
                break;

            case "Elf Maiden":
                size_w = 9;
                size_h = 9;
                scale_x = 0.9f;
                scale_y = 0.9f;
                pos_x = -0.51f;
                pos_y = -0.35f;
                i = 7;
                break;

            case "Gunslinger":
                size_w = 10;
                scale_x = 0.95f;
                scale_y = 0.95f;
                pos_y = 0.26f;
                i = 8;
                break;

            case "Dark Knight":
                size_w = 10.18f;
                size_h = 7.27f;
                scale_x = 1.1f;
                scale_y = 1.1f;
                pos_x = -0.35f;
                pos_y = 0.49f;
                i = 9;
                break;

            case "Steel Bat":
                size_w = 9;
                scale_x = 0.95f;
                scale_y = 0.95f;
                pos_y = 0.27f;
                i = 10;
                break;

            case "Tinker":
                size_w = 9;
                pos_y = 0.48f;
                i = 11;
                break;

            default:
                size_w = 0;
                size_h = 0;
                break;
        }

        rect_transform.sizeDelta = new Vector2(size_w, size_h);
        rect_transform.localScale = new Vector2(scale_x, scale_y);
        rect_transform.localPosition = new Vector2(pos_x, pos_y);
        avatar.sprite = InventoryManager.inventory_manager.sprites[i];
    }

    // Проверяем записан ли юнит в текущем слоте
    private string CheckSlot()
    {
        return GlobalData.GetString(name.Substring(3));
    }

    // Сохраняем нового юнита в слоте
    private void SaveSlot()
    {
        GlobalData.SetString(name.Substring(3), choosed_unit);
    }

    // Воспроизводим звуки
    private void PlaySound(float pitch, float volume)
    {

    }
}
