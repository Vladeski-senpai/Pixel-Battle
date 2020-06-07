using UnityEngine;
using UnityEngine.UI;

public class DefaultManaButton : MonoBehaviour
{
    private DefaultGameController game_controller;
    private ButtonClickAnimation btn_animation;
    private AudioSource audio_s;
    private Button button;
    private Text txt_spent_mana; // Текст потраченной маны на создание юнита

    private float
        startX, // Начальная х координата текста маны
        startY, // Начальная у координата текста маны
        currentY; // Текущая у координата текста маны

    private bool
        isActive = true, // Включена ли кнопка
        isAnimated; // Анимируется ли текст

    private void Awake()
    {
        txt_spent_mana = transform.GetChild(3).GetComponent<Text>(); // Текст затраченной маны
        btn_animation = GetComponent<ButtonClickAnimation>();
        button = GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);

        // Если звук включён
        if (GlobalData.GetInt("Sound") != 0)
            audio_s = GetComponent<AudioSource>();
    }

    private void Start()
    {
        game_controller = DefaultGameController.default_controller; // Кэшируем скрипт
        startX = txt_spent_mana.transform.localPosition.x; // Записываем x координату текста затраченной маны на создание юнита
        startY = txt_spent_mana.transform.localPosition.y; // Записываем у координату текста затраченной маны на создание юнита
        currentY = startY;
        ButtonCondition(); // Проверяем состояние кнопки
    }

    private void Update()
    {
        // Если хватает маны на апгрейд, активируем кнопку
        if (game_controller.CurrentMana >= game_controller.UpgradeCost)
        {
            if (!isActive)
            {
                // Воспроизводим звук когда кнопка становится активной
                if (audio_s != null) PlaySound(0.8f, 0.8f);

                ButtonCondition();
                btn_animation.isAnimated = true;
            }
        }
        else if (isActive) ButtonCondition();

        // Анимируем текст потраченной маны
        if (isAnimated)
        {
            currentY += 4f;

            if (currentY >= 210)
            {
                isAnimated = false;
                currentY = startY;
                txt_spent_mana.text = "";
            }

            txt_spent_mana.transform.localPosition = new Vector2(startX, currentY);
        }
    }

    // Действия при нажатии на кнопку
    private void TaskOnClick()
    {
        // Если звук "включён"
        if (audio_s != null) PlaySound(1, 1);

        AnimateText(game_controller.UpgradeCost);
        game_controller.BoostMana(); // Апгрейдим ману
    }

    // Активируем/деактивируем кнопку
    private void ButtonCondition()
    {
        isActive = !isActive;
        button.interactable = isActive; // Меняем состояние кнопки
    }

    // Анимирует текст затраченной маны
    public void AnimateText(float spent_count)
    {
        txt_spent_mana.text = "-" + spent_count;
        isAnimated = true;
    }

    // Воспроизводим звук
    private void PlaySound(float volume, float pitch)
    {
        audio_s.volume = volume;
        audio_s.pitch = pitch;
        audio_s.Play();
    }
}
