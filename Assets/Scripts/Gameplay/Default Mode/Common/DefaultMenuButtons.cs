using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DefaultMenuButtons : MonoBehaviour
{
    public GameObject[] some_obj;

    private AudioSource audio_s;
    private Text txt; // Текст кнопки

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
        txt = transform.GetChild(0).GetComponent<Text>();
        audio_s = GetComponent<AudioSource>();

        // Переводим текст кнопки
        if (txt != null)
            txt.text = GlobalTranslateSystem.TranslateShortText(name.Substring(3));
    }

    private void TaskOnClick()
    {
        if (AudioManager.instance.IsOn())
            audio_s.Play();

        switch (name.Substring(3))
        {
            case "Pause":
                StartCoroutine(Timer("Pause", 0.1f));
                break;

            case "Resume":
                Time.timeScale = 1;
                StartCoroutine(Timer("Resume", 0.1f));
                break;

            // Загружаем главное меню
            case "Menu":
                Time.timeScale = 1;
                GlobalStats.SaveStatistics(); // Сохраняем статистику
                ScenesManager.scenes_manager.LoadLevel(0);
                break;

            // Загружаем сцену с игрой
            case "Restart":
                ScenesManager.scenes_manager.LoadLevel(1);
                break;

            // Загружаем инвентарь
            case "Continue":
                ScenesManager.scenes_manager.LoadLevel(2);
                break;
        }
    }

    private IEnumerator Timer(string code, float time)
    {
        yield return new WaitForSeconds(time);

        switch (code)
        {
            case "Pause":
                some_obj[0].SetActive(true);

                if (AudioManager.instance.IsMuicOn)
                    some_obj[1].GetComponent<AudioSource>().Pause(); // Ставим музыку на паузу
                Time.timeScale = 0;
                break;

            case "Resume":
                some_obj[0].SetActive(false);

                if (AudioManager.instance.IsMuicOn)
                    some_obj[1].GetComponent<AudioSource>().Play(); // Возобновляем музыку
                break;
        }
    }
}
