using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DefaultSpawnButtons : MonoBehaviour
{
    private DefaultGameController game_controller;
    private ButtonClickAnimation btn_animation;
    private AudioSource audio_s;
    private Button button;
    private byte button_id;

    private void Awake()
    {
        btn_animation = GetComponent<ButtonClickAnimation>();
        button = GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
        button_id = System.Convert.ToByte(name.Substring(8));

        // Если звук включён
        if (GlobalData.GetInt("Sound") != 0)
            audio_s = GetComponent<AudioSource>();
    }

    private void Start()
    {
        game_controller = DefaultGameController.default_controller;
    }

    private void TaskOnClick()
    {
        // Если звук "включён"
        if (audio_s != null)
            audio_s.Play();

        game_controller.CreateAllyyUnit(button_id); // Посылаем запрос на создание юнита
        if (gameObject.activeSelf) StartCoroutine(CoolDown()); // Выключаем кнопку и запускаем кулдаун включения
    }

    // Сбрасываем анимацию при отключении объекта
    private void OnEnable()
    {
        button.interactable = true;
        btn_animation.ResetAnimation();
    }

    // Кулдаун между нажатиями
    private IEnumerator CoolDown()
    {
        button.interactable = false;
        yield return new WaitForSeconds(0.2f);
        button.interactable = true;
    }
}
