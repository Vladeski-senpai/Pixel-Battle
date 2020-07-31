using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GuildExitButton : MonoBehaviour
{
    private GuildManager guild_manager;
    private ButtonClickAnimation btn_animation;
    private AudioSource audio_s;

    private bool isOn; // Включён ли звук

    private void Awake()
    {
        audio_s = GetComponent<AudioSource>();
        btn_animation = GetComponent<ButtonClickAnimation>();
        GetComponent<Button>().onClick.AddListener(TaskOnClick);

        if (GlobalData.GetInt("Sound") != 0) isOn = true;
    }

    private void Start()
    {
        guild_manager = GuildManager.guild_manager;
    }

    private void TaskOnClick()
    {
        if (gameObject.activeSelf)
        {
            // Звук нажатия
            if (isOn) audio_s.Play();

            StartCoroutine(CloseMenu());
        }
    }

    private IEnumerator CloseMenu()
    {
        yield return new WaitForSeconds(0.2f);
        guild_manager.CloseMenu();
    }

    // Сбрасываем анимацию при отключении объекта
    private void OnEnable()
    {
        btn_animation.ResetAnimation();
    }
}
